using SampleProject.Entities;
using SampleProject.Enums;
using SampleProject.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueSight.Common;
using System;

namespace SampleProject.Services.MOrderServiceContent
{
	public interface IOrderServiceContentValidator : IServiceScoped
	{
		Task<bool> Create(OrderServiceContent OrderServiceContent);
		Task<bool> Update(OrderServiceContent OrderServiceContent);
	}
	public class OrderServiceContentValidator : IOrderServiceContentValidator
	{
		public enum ErrorCode
		{
			IdNotExisted,
			ServiceEmpty,
			ServiceNotExisted,
			OrderServiceEmpty,
			OrderServiceNotExisted,
			PrimaryUnitOfMeasureEmpty,
			PrimaryUnitOfMeasureNotExisted,
			UnitOfMeasureEmpty,
			UnitOfMeasureNotExisted,
			QuantityInvalid,
			RequestQuantityInvalid,
			PriceInvalid,
			AmountInvalid,
		}

		private IUOW UOW;

		public OrderServiceContentValidator(IUOW UOW)
		{
			this.UOW = UOW;
		}

		public async Task<bool> ValidateId(OrderServiceContent OrderServiceContent)
		{
			OrderServiceContentFilter OrderServiceContentFilter = new OrderServiceContentFilter
			{
				Skip = 0,
				Take = 10,
				Id = new IdFilter { Equal = OrderServiceContent.Id },
				Selects = OrderServiceContentSelect.Id
			};

			int count = await UOW.OrderServiceContentRepository.Count(OrderServiceContentFilter);
			if (count == 0)
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.Id), ErrorCode.IdNotExisted);
			return count == 1;
		}


		private async Task<bool> ValidateService(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.ServiceId == 0)
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.ServiceId), ErrorCode.ServiceEmpty);
			else
			{
				ServiceFilter ServiceFilter = new ServiceFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = OrderServiceContent.ServiceId },
					Selects = ServiceSelect.Id
				};

				int count = await UOW.ServiceRepository.Count(ServiceFilter);
				if (count == 0)
					OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.Service), ErrorCode.ServiceNotExisted);
			}
			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidateOrderService(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.ServiceId == 0)
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.ServiceId), ErrorCode.OrderServiceEmpty);
			else
			{
				OrderServiceFilter OrderServiceFilter = new OrderServiceFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = OrderServiceContent.ServiceId },
					Selects = OrderServiceSelect.Id
				};

				int count = await UOW.OrderServiceRepository.Count(OrderServiceFilter);
				if (count == 0)
					OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.OrderService), ErrorCode.OrderServiceNotExisted);
			}
			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidateUnitOfMeasure(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.UnitOfMeasureId == 0)
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.UnitOfMeasureId), ErrorCode.UnitOfMeasureEmpty);
			else
			{
				UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = OrderServiceContent.UnitOfMeasureId },
					Selects = UnitOfMeasureSelect.Id
				};

				int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
				if (count == 0)
					OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.UnitOfMeasure), ErrorCode.UnitOfMeasureNotExisted);
			}

			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidatePrimaryUnitOfMeasure(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.PrimaryUnitOfMeasureId == 0)
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.PrimaryUnitOfMeasureId), ErrorCode.PrimaryUnitOfMeasureEmpty);
			else
			{
				UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
				{
					Skip = 0,
					Take = 10,
					Id = new IdFilter { Equal = OrderServiceContent.PrimaryUnitOfMeasureId },
					Selects = UnitOfMeasureSelect.Id
				};

				int count = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
				if (count == 0)
					OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.UnitOfMeasure), ErrorCode.PrimaryUnitOfMeasureNotExisted);
			}

			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidateQuantity(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.Quantity <= 0)
			{
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.Quantity), ErrorCode.QuantityInvalid);
			}
			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidateRequestQuantity(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.RequestQuantity <= 0)
			{
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.RequestQuantity), ErrorCode.RequestQuantityInvalid);
			}
			return OrderServiceContent.IsValidated;
		}

		private async Task<bool> ValidatePrice(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.Price <= 0)
			{
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.Price), ErrorCode.PriceInvalid);
			}
			return OrderServiceContent.IsValidated;
		}
		private async Task<bool> ValidateAmount(OrderServiceContent OrderServiceContent)
		{
			if (OrderServiceContent.Amount <= 0)
			{
				OrderServiceContent.AddError(nameof(OrderServiceContentValidator), nameof(OrderServiceContent.Amount), ErrorCode.AmountInvalid);
			}
			return OrderServiceContent.IsValidated;
		}

		public async Task<bool> Create(OrderServiceContent OrderServiceContent)
		{
			await ValidateService(OrderServiceContent);
			await ValidateOrderService(OrderServiceContent);
			await ValidatePrimaryUnitOfMeasure(OrderServiceContent);
			await ValidateUnitOfMeasure(OrderServiceContent);
			await ValidateQuantity(OrderServiceContent);
			await ValidateRequestQuantity(OrderServiceContent);
			await ValidatePrice(OrderServiceContent);
			await ValidateAmount(OrderServiceContent);
			return OrderServiceContent.IsValidated;
		}

		public async Task<bool> Update(OrderServiceContent OrderServiceContent)
		{
			if (await ValidateId(OrderServiceContent))
			{
				await ValidateService(OrderServiceContent);
				await ValidateOrderService(OrderServiceContent);
				await ValidatePrimaryUnitOfMeasure(OrderServiceContent);
				await ValidateUnitOfMeasure(OrderServiceContent);
				await ValidateQuantity(OrderServiceContent);
				await ValidateRequestQuantity(OrderServiceContent);
				await ValidatePrice(OrderServiceContent);
				await ValidateAmount(OrderServiceContent);
			}
			return OrderServiceContent.IsValidated;
		}
	}
}
