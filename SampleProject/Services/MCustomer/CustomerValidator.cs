using SampleProject.Entities;
using SampleProject.Enums;
using SampleProject.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueSight.Common;

namespace SampleProject.Services.MCustomer
{
	public interface ICustomerValidator : IServiceScoped
	{
		Task<bool> Create(Customer Customer);
		Task<bool> Update(Customer Customer);
		Task<bool> Delete(Customer Customer);
		//Task<bool> BulkDelete(List<Customer> Customers);
		Task<bool> BulkMerge(List<Customer> Customers);
		//Task<bool> Import(List<Customer> Customers);
	}
	public class CustomerValidator : ICustomerValidator
	{
		public enum ErrorCode
		{
			IdNotExisted,
			CodeEmpty,
			CodeExisted,
			CodeHasSpecialCharacter,
			NameEmpty,
			NameOverLength,
			PhoneIsValidate,
			AddressOverLength,
			StatusNotExisted,
			CustomerInUsed
		}

		private IUOW UOW;

		public CustomerValidator(IUOW UOW)
		{
			this.UOW = UOW;
		}

		public async Task<bool> ValidateId(Customer Customer)
		{
			CustomerFilter CustomerFilter = new CustomerFilter
			{
				Skip = 0,
				Take = 10,
				Id = new IdFilter { Equal = Customer.Id },
				Selects = CustomerSelect.Id
			};

			int count = await UOW.CustomerRepository.Count(CustomerFilter);
			if (count == 0)
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Id), ErrorCode.IdNotExisted);
			return count == 1;
		}

		private async Task<bool> ValidateCode(Customer Customer)
		{
			if (string.IsNullOrWhiteSpace(Customer.Code))
			{
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Code), ErrorCode.CodeEmpty);
			}
			else
			{
				var Code = Customer.Code;
				if (Customer.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Customer.Code))
				{
					Customer.AddError(nameof(CustomerValidator), nameof(Customer.Code), ErrorCode.CodeHasSpecialCharacter);
				}

				CustomerFilter CustomerFilter = new CustomerFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { NotEqual = Customer.Id },
					Code = new StringFilter { Equal = Customer.Code },
					Selects = CustomerSelect.Code
				};

				int count = await UOW.CustomerRepository.Count(CustomerFilter);
				if (count != 0)
					Customer.AddError(nameof(CustomerValidator), nameof(Customer.Code), ErrorCode.CodeExisted);
			}

			return Customer.IsValidated;
		}

		private async Task<bool> ValidateName(Customer Customer)
		{
			if (string.IsNullOrWhiteSpace(Customer.Name))
			{
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Name), ErrorCode.NameEmpty);
			}
			else if (Customer.Name.Length > 500)
			{
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Name), ErrorCode.NameOverLength);
			}
			return Customer.IsValidated;
		}

		private async Task<bool> ValidatePhone(Customer Customer)
		{
			Regex phonenumber = new Regex(@"\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})");

			if (!string.IsNullOrWhiteSpace(Customer.Phone) && phonenumber.IsMatch(Customer.Phone.Trim()))
			{
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Phone), ErrorCode.PhoneIsValidate);
			}
			return Customer.IsValidated;
		}
		private async Task<bool> ValidateAddress(Customer Customer)
		{
			if (!string.IsNullOrWhiteSpace(Customer.Address) && Customer.Address.Length>500)
			{
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Address), ErrorCode.AddressOverLength);
			}
			return Customer.IsValidated;
		}
		private async Task<bool> ValidateStatus(Customer Customer)
		{
			if (StatusEnum.ACTIVE.Id != Customer.StatusId && StatusEnum.INACTIVE.Id != Customer.StatusId)
				Customer.AddError(nameof(CustomerValidator), nameof(Customer.Status), ErrorCode.StatusNotExisted);
			return Customer.IsValidated;
		}

		public async Task<bool> Create(Customer Customer)
		{
			await ValidateCode(Customer);
			await ValidateName(Customer);
			await ValidatePhone(Customer);
			await ValidateAddress(Customer);
			await ValidateStatus(Customer);
			return Customer.IsValidated;
		}

		public async Task<bool> Update(Customer Customer)
		{
			if (await ValidateId(Customer))
			{
				await ValidateCode(Customer);
				await ValidateName(Customer);
				await ValidatePhone(Customer);
				await ValidateAddress(Customer);
				await ValidateStatus(Customer);
			}
			return Customer.IsValidated;
		}

		public async Task<bool> Delete(Customer Customer)
		{
			if (await ValidateId(Customer))
			{
				var oldData = await UOW.CustomerRepository.Get(Customer.Id);
				if (oldData.Used)
				{
					Customer.AddError(nameof(CustomerValidator), nameof(Customer.Id), ErrorCode.CustomerInUsed);
				}
			}
			return Customer.IsValidated;
		}

		/*public async Task<bool> BulkDelete(List<Customer> Customers)
		{
			var Ids = Customer.Select(x => x.Id).ToList();

			var listInDB = await UOW.CustomerRepository.List(Ids);

			foreach (var Customer in Customers)
			{
				var BrandInDb = listInDB.Where(x => x.Id == Customer.Id).FirstOrDefault();
				if (BrandInDb != null && BrandInDb.Used)
				{
					Customer.AddError(nameof(CustomerValidator), nameof(Customer.Id), ErrorCode.CustomerInUsed);
				}
			}
			return false;
		}*/

		/*public async Task<bool> Import(List<Customer> Customers)
		{
			var listCodeInDB = (await UOW.CustomerRepository.List(new CustomerFilter
			{
				Skip = 0,
				Take = int.MaxValue,
				Selects = CustomerSelect.Code
			})).Select(e => e.Code);

			foreach (var Customer in Customers)
			{
				if (listCodeInDB.Contains(Customer.Code))
				{
					Customer.AddError(nameof(CustomerValidator), nameof(Customer.Code), ErrorCode.CodeExisted);
				}

				await (ValidateName(Customer));
				await (ValidatePhone(Customer));
				await (ValidateAddress(Customer));
			}

			return Customers.Any(o => !o.IsValidated) ? false : true;
		}*/
		public async Task<bool> BulkMerge(List<Customer> Customers)
		{
			return true;
		}
	}
}
