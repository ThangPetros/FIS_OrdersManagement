using TrueSight.Common;
using SampleProject.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleProject.Rpc.service
{
	[DisplayName("Dịch vụ")]
	public class ServiceRoute : Root
	{
		public const string Parent = Module + "/service";
		public const string Master = Module + "/service/service-master";
		public const string Detail = Module + "/service/service-detail/*";
		private const string Default = Rpc + Module + "/service";
		public const string Count = Default + "/count";
		public const string List = Default + "/list";
		public const string Get = Default + "/get";
		public const string Create = Default + "/create";
		public const string Update = Default + "/update";
		public const string Delete = Default + "/delete";
	}
}
