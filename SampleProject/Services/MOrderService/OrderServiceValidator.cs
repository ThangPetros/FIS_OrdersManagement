using SampleProject.Entities;
using SampleProject.Enums;
using SampleProject.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueSight.Common;
using System;
namespace SampleProject.Services.MOrderService
{
	public interface IOrderServiceValidator : IServiceScoped
	{
		Task<bool> Create(OrderService OrderService);
		Task<bool> Update(OrderService OrderService);
		Task<bool> Delete(OrderService OrderService);
		Task<bool> BulkDelete(List<OrderService> OrderServices);
		Task<bool> BulkMerge(List<OrderService> OrderServices);
		//Task<bool> Import(List<Service> Services);
	}
	public class OrderServiceValidator : IOrderServiceValidator
	{
		public enum ErrorCode
		{
			IdNotExisted,
			CodeEmpty,
			CodeExisted,
			CodeHasSpecialCharacter,
			CustomerEmpty,
			CustomerNotExisted,
			TotalInvalid,
			OrderServiceInUsed
		}

		private IUOW UOW;

		public OrderServiceValidator(IUOW UOW)
		{
			this.UOW = UOW;
		}

		public async Task<bool> ValidateId(OrderService OrderService)
		{
			OrderServiceFilter OrderServiceFilter = new OrderServiceFilter
			{
				Skip = 0,
				Take = 10,
				Id = new IdFilter { Equal = OrderService.Id },
				Selects = OrderServiceSelect.Id
			};

			int count = await UOW.OrderServiceRepository.Count(OrderServiceFilter);
			if (count == 0)
				OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Id), ErrorCode.IdNotExisted);
			return count == 1;
		}

		private async Task<bool> ValidateCode(OrderService OrderService)
		{
			if (string.IsNullOrWhiteSpace(OrderService.Code))
			{
				OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Code), ErrorCode.CodeEmpty);
			}
			else
			{
				var Code = OrderService.Code;
				if (OrderService.Code.Contains(" ") || !Code.ChangeToEnglishChar().Equals(OrderService.Code))
				{
					OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Code), ErrorCode.CodeHasSpecialCharacter);
				}

				OrderServiceFilter OrderServiceFilter = new OrderServiceFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { NotEqual = OrderService.Id },
					Code = new StringFilter { Equal = OrderService.Code },
					Selects = OrderServiceSelect.Code
				};

				int count = await UOW.OrderServiceRepository.Count(OrderServiceFilter);
				if (count != 0)
					OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Code), ErrorCode.CodeExisted);
			}

			return OrderService.IsValidated;
		}

		private async Task<bool> ValidateCustomer(OrderService OrderService)
		{
			if (OrderService.CustomerId == 0)
				OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.CustomerId), ErrorCode.CustomerEmpty);
			else
			{
				CustomerFilter CustomerFilter = new CustomerFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = OrderService.CustomerId },
					Selects = CustomerSelect.Id
				};

				int count = await UOW.CustomerRepository.Count(CustomerFilter);
				if (count == 0)
					OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Customer), ErrorCode.CustomerNotExisted);
			}

			return OrderService.IsValidated;
		}
		private async Task<bool> ValidateTotal(OrderService OrderService)
		{
			if (OrderService.Total <= 0)
			{
				OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Total), ErrorCode.TotalInvalid);
			}
			return OrderService.IsValidated;
		}

		public async Task<bool> Create(OrderService OrderService)
		{
			await ValidateCode(OrderService);
			await ValidateCustomer(OrderService);
			await ValidateTotal(OrderService);
			return OrderService.IsValidated;
		}

		public async Task<bool> Update(OrderService OrderService)
		{
			if (await ValidateId(OrderService))
			{
				await ValidateCode(OrderService);
				await ValidateCustomer(OrderService);
				await ValidateTotal(OrderService);
			}
			return OrderService.IsValidated;
		}

		public async Task<bool> Delete(OrderService OrderService)
		{
			if (await ValidateId(OrderService))
			{
				var oldData = await UOW.OrderServiceRepository.Get(OrderService.Id);
				if (oldData.Used)
				{
					OrderService.AddError(nameof(OrderServiceValidator), nameof(OrderService.Id), ErrorCode.OrderServiceInUsed);
				}
			}
			return OrderService.IsValidated;
		}

		public async Task<bool> BulkDelete(List<OrderService> OrderServices)
		{
			foreach (OrderService OrderService in OrderServices)
			{
				await Delete(OrderService);
			}
			return OrderServices.All(st => st.IsValidated);
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
		public async Task<bool> BulkMerge(List<OrderService> OrderServices)
		{
			foreach (OrderService OrderService in OrderServices)
			{
				await ValidateId(OrderService);
			}
			return OrderServices.All(x => x.IsValidated);
		}
	}
}
