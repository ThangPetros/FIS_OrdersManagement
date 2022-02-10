using System.Text;

namespace SampleProject.Helper
{
	public static class Utils
	{
		//public static T Clone<T>(this T obj)
		//{
		//    var json = JsonConvert.SerializeObject(obj);
		//    T newObj = JsonConvert.DeserializeObject<T>(json);
		//    return newObj;
		//}

		public static bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		public static string ConvertAmountTostring(long Number)
		{
			StringBuilder wordAmount = new StringBuilder();
			long OriginalNumber = Number;
			if (Number / 1000000000 > 0)
			{
				long billions = Number / 1000000000;
				long unitBillion = (billions % 10) / 1;
				long tenthBillion = (billions % 100) / 10;
				long hundredBillion = (billions % 1000) / 100;
				if (hundredBillion > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundredBillion)).Append("trăm ");
				if (hundredBillion > 0 && tenthBillion == 0 && unitBillion > 0)
					wordAmount = wordAmount.Append("linh ");
				else if (tenthBillion == 1)
					wordAmount = wordAmount.Append("mười ");
				else if (tenthBillion > 1)
					wordAmount = wordAmount.Append(ConvertDigitTostring(tenthBillion)).Append("mươi ");

				if (unitBillion > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(unitBillion));
				wordAmount = wordAmount.Append("tỷ ");
				Number = Number % 1000000000;
			}

			if (Number / 1000000 > 0)
			{
				long millions = Number / 1000000;
				long unitMillion = (millions % 10) / 1;
				long tenthMillion = (millions % 100) / 10;
				long hundredMillion = (millions % 1000) / 100;
				if (hundredMillion > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundredMillion)).Append("trăm ");
				else if (hundredMillion == 0 && tenthMillion > 0 && (OriginalNumber / 1000000000) > 0)
				{
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundredMillion)).Append("trăm ");
				}
				if (hundredMillion > 0 && tenthMillion == 0 && unitMillion > 0)
					wordAmount = wordAmount.Append("linh ");
				else if (tenthMillion == 1)
					wordAmount = wordAmount.Append("mười ");
				else if (tenthMillion > 1)
					wordAmount = wordAmount.Append(ConvertDigitTostring(tenthMillion)).Append("mươi ");

				if (unitMillion > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(unitMillion));
				wordAmount = wordAmount.Append("triệu ");
				Number = Number % 1000000;
			}


			if (Number / 1000 > 0)
			{
				long thousands = Number / 1000;
				long unitThousand = (thousands % 10) / 1;
				long tenthThousand = (thousands % 100) / 10;
				long hundredThousand = (thousands % 1000) / 100;

				if (hundredThousand > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundredThousand)).Append("trăm ");
				else if (hundredThousand == 0 && tenthThousand > 0 && ((OriginalNumber / 1000000000) > 0 || (OriginalNumber / 1000000) > 0))
				{
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundredThousand)).Append("trăm ");
				}
				if (hundredThousand > 0 && tenthThousand == 0 && unitThousand > 0)
					wordAmount = wordAmount.Append("linh ");
				else if (tenthThousand == 1)
					wordAmount = wordAmount.Append("mười ");
				else if (tenthThousand > 1)
					wordAmount = wordAmount.Append(ConvertDigitTostring(tenthThousand)).Append("mươi ");

				if (unitThousand > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(unitThousand));
				wordAmount = wordAmount.Append("nghìn ");
				Number = Number % 1000;
			}

			if (Number / 1 > 0)
			{
				long unit = (Number % 10) / 1;
				long tenth = (Number % 100) / 10;
				long hundred = (Number % 1000) / 100;
				if (hundred > 0)
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundred)).Append("trăm ");
				else if (hundred == 0 && unit > 0 && ((Number / 1000000000) > 0 || (Number / 1000000) > 0 || (Number / 1000) > 0))
				{
					wordAmount = wordAmount.Append(ConvertDigitTostring(hundred)).Append("trăm ");
				}
				if (tenth == 0 && unit > 0)
					wordAmount = wordAmount.Append("linh ");
				else if (tenth == 1)
					wordAmount = wordAmount.Append("mười ");
				else if (tenth > 1)
					wordAmount = wordAmount.Append(ConvertDigitTostring(tenth)).Append("mươi ");

				if (unit > 1)
					wordAmount = wordAmount.Append(ConvertDigitTostring(unit));
			}
			wordAmount = wordAmount.Append("đồng");
			wordAmount[0] = char.ToUpper(wordAmount[0]);

			return wordAmount.ToString();
		}

		private static string ConvertDigitTostring(long Digit)
		{
			switch (Digit)
			{
				case 0:
					return "không ";
				case 1:
					return "một ";
				case 2:
					return "hai ";
				case 3:
					return "ba ";
				case 4:
					return "bốn ";
				case 5:
					return "năm ";
				case 6:
					return "sáu ";
				case 7:
					return "bảy ";
				case 8:
					return "tám ";
				case 9:
					return "chín ";
				default:
					return "";
			}
		}
	}
}
