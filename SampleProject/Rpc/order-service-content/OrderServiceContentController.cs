using Microsoft.AspNetCore.Mvc;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Services.MOrderService;
using SampleProject.Services.MOrderServiceContent;
using SampleProject.Services.MUnitOfMeasure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service_content
{
	public class OrderServiceContentController:RpcController
	{
		private IUnitOfMeasureService UnitOfMeasureService;
		private IOrderServiceService OrderServiceService;
		private IOrderServiceContentService OrderServiceContentService;
		private ICurrentContext CurrentContext;
		public OrderServiceContentController(
		    IUnitOfMeasureService UnitOfMeasureService,
		    IOrderServiceService OrderServiceService,
		    IOrderServiceContentService OrderServiceContentService,
		    ICurrentContext CurrentContext)
		{
			this.UnitOfMeasureService = UnitOfMeasureService;
			this.OrderServiceService = OrderServiceService;
			this.OrderServiceContentService = OrderServiceContentService;
			this.CurrentContext = CurrentContext;
		}

		[Route(OrderServiceContentRoute.Count), HttpPost]
		public async Task<int> Count([FromBody] OrderServiceContent_OrderServiceContentFilterDTO OrderServiceContent_OrderServiceContentFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			OrderServiceContentFilter OrderServiceContentFilter = ConvertFilterDTOToFilterEntity(OrderServiceContent_OrderServiceContentFilterDTO);
			int count = await OrderServiceContentService.Count(OrderServiceContentFilter);
			return count;
		}

		[Route(OrderServiceContentRoute.List), HttpPost]
		public async Task<List<OrderServiceContent_OrderServiceContentDTO>> List([FromBody] OrderServiceContent_OrderServiceContentFilterDTO OrderServiceContent_OrderServiceContentFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			OrderServiceContentFilter OrderServiceContentFilter = ConvertFilterDTOToFilterEntity(OrderServiceContent_OrderServiceContentFilterDTO);
			List<OrderServiceContent> OrderServiceContents = await OrderServiceContentService.List(OrderServiceContentFilter);
			List<OrderServiceContent_OrderServiceContentDTO> OrderServiceContent_OrderServiceContentDTOs = OrderServiceContents.Select(x => new OrderServiceContent_OrderServiceContentDTO(x)).ToList();
			return OrderServiceContent_OrderServiceContentDTOs;
		}

		[Route(OrderServiceContentRoute.Get), HttpPost]
		public async Task<ActionResult<OrderServiceContent_OrderServiceContentDTO>> Get([FromBody] OrderServiceContent_OrderServiceContentDTO OrderServiceContent_OrderServiceContentDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			if (!await HasPermission(OrderServiceContent_OrderServiceContentDTO.Id))
				return Forbid();

			OrderServiceContent OrderServiceContent = await OrderServiceContentService.Get(OrderServiceContent_OrderServiceContentDTO.Id);
			return new OrderServiceContent_OrderServiceContentDTO(OrderServiceContent);

		}

		[Route(OrderServiceContentRoute.Create), HttpPost]
		public async Task<ActionResult<OrderServiceContent_OrderServiceContentDTO>> Create([FromBody] OrderServiceContent_OrderServiceContentDTO OrderServiceContent_OrderServiceContentDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(OrderServiceContent_OrderServiceContentDTO.Id))
				return Forbid();
			OrderServiceContent OrderServiceContent = ConvertDTOToEntity(OrderServiceContent_OrderServiceContentDTO);

			OrderServiceContent = await OrderServiceContentService.Create(OrderServiceContent);
			OrderServiceContent_OrderServiceContentDTO = new OrderServiceContent_OrderServiceContentDTO(OrderServiceContent);
			if (OrderServiceContent.IsValidated)
				return OrderServiceContent_OrderServiceContentDTO;
			else
				return BadRequest(OrderServiceContent_OrderServiceContentDTO);
		}

		[Route(OrderServiceContentRoute.Update), HttpPost]
		public async Task<ActionResult<OrderServiceContent_OrderServiceContentDTO>> Update([FromBody] OrderServiceContent_OrderServiceContentDTO OrderServiceContent_OrderServiceContentDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(OrderServiceContent_OrderServiceContentDTO.Id))
				return Forbid();

			OrderServiceContent OrderServiceContent = ConvertDTOToEntity(OrderServiceContent_OrderServiceContentDTO);

			OrderServiceContent = await OrderServiceContentService.Update(OrderServiceContent);
			OrderServiceContent_OrderServiceContentDTO = new OrderServiceContent_OrderServiceContentDTO(OrderServiceContent);
			if (OrderServiceContent.IsValidated)
				return OrderServiceContent_OrderServiceContentDTO;
			else
				return BadRequest(OrderServiceContent_OrderServiceContentDTO);
		}

		private async Task<bool> HasPermission(long Id)
		{
			OrderServiceContentFilter OrderServiceContentFilter = new OrderServiceContentFilter();
			if (Id == 0)
			{

			}
			else
			{
				OrderServiceContentFilter.Id = new IdFilter { Equal = Id };
				int count = await OrderServiceContentService.Count(OrderServiceContentFilter);
				if (count == 0)
					return false;
			}
			return true;
		}
		private OrderServiceContent ConvertDTOToEntity(OrderServiceContent_OrderServiceContentDTO OrderServiceContent_OrderServiceContentDTO)
		{
			OrderServiceContent OrderServiceContent = new OrderServiceContent();
			OrderServiceContent.Id = OrderServiceContent_OrderServiceContentDTO.Id;
			OrderServiceContent.ServiceId = OrderServiceContent_OrderServiceContentDTO.ServiceId;
			OrderServiceContent.OrderServiceId = OrderServiceContent_OrderServiceContentDTO.OrderServiceId;
			OrderServiceContent.PrimaryUnitOfMeasureId = OrderServiceContent_OrderServiceContentDTO.PrimaryUnitOfMeasureId;
			OrderServiceContent.UnitOfMeasureId = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasureId;
			OrderServiceContent.Price = OrderServiceContent_OrderServiceContentDTO.Price;
			OrderServiceContent.RequestQuantity = OrderServiceContent_OrderServiceContentDTO.RequestQuantity;
			OrderServiceContent.Quantity = OrderServiceContent_OrderServiceContentDTO.Quantity;
			OrderServiceContent.Amount = OrderServiceContent_OrderServiceContentDTO.Amount;

			OrderServiceContent.Service = OrderServiceContent_OrderServiceContentDTO.Service == null ? null : new Service
			{
				Id = OrderServiceContent_OrderServiceContentDTO.Service.Id,
				Code = OrderServiceContent_OrderServiceContentDTO.Service.Code,
				Name = OrderServiceContent_OrderServiceContentDTO.Service.Name,
				UnitOfMeasureId = OrderServiceContent_OrderServiceContentDTO.Service.UnitOfMeasureId,
				Price = OrderServiceContent_OrderServiceContentDTO.Service.Price,
				StatusId = OrderServiceContent_OrderServiceContentDTO.Service.StatusId
			};
			OrderServiceContent.OrderService = OrderServiceContent_OrderServiceContentDTO.OrderService == null ? null : new OrderService
			{
				Id = OrderServiceContent_OrderServiceContentDTO.OrderService.Id,
				Code = OrderServiceContent_OrderServiceContentDTO.OrderService.Code,
				OrderDate = OrderServiceContent_OrderServiceContentDTO.OrderService.OrderDate,
				CustomerId = OrderServiceContent_OrderServiceContentDTO.OrderService.CustomerId,
				Total = OrderServiceContent_OrderServiceContentDTO.OrderService.Total
			};

			OrderServiceContent.UnitOfMeasure = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure == null ? null : new UnitOfMeasure
			{
				Id = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Id,
				Code = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Code,
				Name = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Name,
				StatusId = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.StatusId
			};
			OrderServiceContent.PrimaryUnitOfMeasure = OrderServiceContent_OrderServiceContentDTO.PrimaryUnitOfMeasure == null ? null : new UnitOfMeasure
			{
				Id = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Id,
				Code = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Code,
				Name = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.Name,
				StatusId = OrderServiceContent_OrderServiceContentDTO.UnitOfMeasure.StatusId
			};

			OrderServiceContent.BaseLanguage = CurrentContext.Language;

			return OrderServiceContent;
		}
		private OrderServiceContentFilter ConvertFilterDTOToFilterEntity(OrderServiceContent_OrderServiceContentFilterDTO OrderServiceContent_OrderServiceContentFilterDTO)
		{
			OrderServiceContentFilter OrderServiceContentFilter = new OrderServiceContentFilter();
			OrderServiceContentFilter.Selects = OrderServiceContentSelect.ALL;
			OrderServiceContentFilter.Skip = OrderServiceContent_OrderServiceContentFilterDTO.Skip;
			OrderServiceContentFilter.Take = OrderServiceContent_OrderServiceContentFilterDTO.Take;
			OrderServiceContentFilter.OrderBy = OrderServiceContent_OrderServiceContentFilterDTO.OrderBy;
			OrderServiceContentFilter.OrderType = OrderServiceContent_OrderServiceContentFilterDTO.OrderType;

			OrderServiceContentFilter.Id = OrderServiceContent_OrderServiceContentFilterDTO.Id;
			OrderServiceContentFilter.ServiceId = OrderServiceContent_OrderServiceContentFilterDTO.ServiceId;
			OrderServiceContentFilter.OrderServiceId = OrderServiceContent_OrderServiceContentFilterDTO.OrderServiceId;
			OrderServiceContentFilter.PrimaryUnitOfMeasureId = OrderServiceContent_OrderServiceContentFilterDTO.PrimaryUnitOfMeasureId;
			OrderServiceContentFilter.UnitOfMeasureId = OrderServiceContent_OrderServiceContentFilterDTO.UnitOfMeasureId;
			OrderServiceContentFilter.Price = OrderServiceContent_OrderServiceContentFilterDTO.Price;
			OrderServiceContentFilter.RequestQuantity = OrderServiceContent_OrderServiceContentFilterDTO.RequestQuantity;
			OrderServiceContentFilter.Quantity = OrderServiceContent_OrderServiceContentFilterDTO.Quantity;
			OrderServiceContentFilter.Amount = OrderServiceContent_OrderServiceContentFilterDTO.Amount;
			OrderServiceContentFilter.UpdateTime = OrderServiceContent_OrderServiceContentFilterDTO.UpdateTime;

			return OrderServiceContentFilter;
		}
	}
}
