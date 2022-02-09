using SampleProject.Entities;
using SampleProject.Enums;
using SampleProject.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueSight.Common;
using System;

namespace SampleProject.Services.MService
{
	public interface IServiceValidator : IServiceScoped
	{
		Task<bool> Create(Service Service);
		Task<bool> Update(Service Service);
		Task<bool> Delete(Service Service);
		Task<bool> BulkDelete(List<Service> Services);
		Task<bool> BulkMerge(List<Service> Services);
		//Task<bool> Import(List<Service> Services);
	}
	public class ServiceValidator : IServiceValidator
	{
		public enum ErrorCode
		{
			IdNotExisted,
			CodeEmpty,
			CodeExisted,
			CodeHasSpecialCharacter,
			NameEmpty,
			NameOverLength,
			PriceInvalid,
			UnitOfMeasureEmpty,
			UnitOfMeasureNotExisted,
			StatusNotExisted,
			ServiceInUsed
		}

		private IUOW UOW;

		public ServiceValidator(IUOW UOW)
		{
			this.UOW = UOW;
		}

		public async Task<bool> ValidateId(Service Service)
		{
			ServiceFilter ServiceFilter = new ServiceFilter
			{
				Skip = 0,
				Take = 10,
				Id = new IdFilter { Equal = Service.Id },
				Selects = ServiceSelect.Id
			};

			int count = await UOW.ServiceRepository.Count(ServiceFilter);
			if (count == 0)
				Service.AddError(nameof(ServiceValidator), nameof(Service.Id), ErrorCode.IdNotExisted);
			return count == 1;
		}

		private async Task<bool> ValidateCode(Service Service)
		{
			if (string.IsNullOrWhiteSpace(Service.Code))
			{
				Service.AddError(nameof(ServiceValidator), nameof(Service.Code), ErrorCode.CodeEmpty);
			}
			else
			{
				var Code = Service.Code;
				if (Service.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(Service.Code))
				{
					Service.AddError(nameof(ServiceValidator), nameof(Service.Code), ErrorCode.CodeHasSpecialCharacter);
				}

				ServiceFilter ServiceFilter = new ServiceFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { NotEqual = Service.Id },
					Code = new StringFilter { Equal = Service.Code },
					Selects = ServiceSelect.Code
				};

				int count = await UOW.ServiceRepository.Count(ServiceFilter);
				if (count != 0)
					Service.AddError(nameof(ServiceValidator), nameof(Service.Code), ErrorCode.CodeExisted);
			}

			return Service.IsValidated;
		}

		private async Task<bool> ValidateName(Service Service)
		{
			if (string.IsNullOrWhiteSpace(Service.Name))
			{
				Service.AddError(nameof(ServiceValidator), nameof(Service.Name), ErrorCode.NameEmpty);
			}
			else if (Service.Name.Length > 500)
			{
				Service.AddError(nameof(ServiceValidator), nameof(Service.Name), ErrorCode.NameOverLength);
			}
			return Service.IsValidated;
		}

		private async Task<bool> ValidatePrice(Service Service)
		{
			if (Service.Price <= 0)
			{
				Service.AddError(nameof(ServiceValidator), nameof(Service.Price), ErrorCode.PriceInvalid);
			}
			return Service.IsValidated;
		}
		private async Task<bool> ValidateUnitOfMeasure(Service Service)
		{
			if (Service.UnitOfMeasureId == 0)
				Service.AddError(nameof(ServiceValidator), nameof(Service.UnitOfMeasureId), ErrorCode.UnitOfMeasureEmpty);
			else
			{
				UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = Service.UnitOfMeasureId },
					Selects = UnitOfMeasureSelect.Id
				};

				int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
				if (count == 0)
					Service.AddError(nameof(ServiceValidator), nameof(Service.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
			}

			return Service.IsValidated;
		}
		private async Task<bool> ValidateStatus(Service Service)
		{
			if (StatusEnum.ACTIVE.Id != Service.StatusId && StatusEnum.INACTIVE.Id != Service.StatusId)
				Service.AddError(nameof(ServiceValidator), nameof(Service.Status), ErrorCode.StatusNotExisted);
			return Service.IsValidated;
		}

		public async Task<bool> Create(Service Service)
		{
			await ValidateCode(Service);
			await ValidateName(Service);
			await ValidatePrice(Service);
			await ValidateUnitOfMeasure(Service);
			await ValidateStatus(Service);
			return Service.IsValidated;
		}

		public async Task<bool> Update(Service Service)
		{
			if (await ValidateId(Service))
			{
				await ValidateCode(Service);
				await ValidateName(Service);
				await ValidatePrice(Service);
				await ValidateUnitOfMeasure(Service);
				await ValidateStatus(Service);
			}
			return Service.IsValidated;
		}

		public async Task<bool> Delete(Service Service)
		{
			if (await ValidateId(Service))
			{
				var oldData = await UOW.CustomerRepository.Get(Service.Id);
				if (oldData.Used)
				{
					Service.AddError(nameof(ServiceValidator), nameof(Service.Id), ErrorCode.ServiceInUsed);
				}
			}
			return Service.IsValidated;
		}

		public async Task<bool> BulkDelete(List<Service> Services)
		{
			foreach (Service Service in Services)
			{
				await Delete(Service);
			}
			return Services.All(st => st.IsValidated);
		}

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
		public async Task<bool> BulkMerge(List<Service> Services)
		{
			foreach (Service Service in Services)
			{
				await ValidateId(Service);
			}
			return Services.All(x => x.IsValidated);
		}
	}
}
