using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Services.MCustomer;
using SampleProject.Services.MOrderService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service
{
	public class OrderServiceController:RpcController
	{
		private ICustomerService CustomerService;
		private IOrderServiceService OrderServiceService;
		private ICurrentContext CurrentContext;
		public OrderServiceController(
		    ICustomerService CustomerService,
		    IOrderServiceService OrderServiceService,
		    ICurrentContext CurrentContext)
		{
			this.CustomerService = CustomerService;
			this.OrderServiceService = OrderServiceService;
			this.CurrentContext = CurrentContext;
		}
		[Route(OrderServiceRoute.Count),HttpPost]
		public async Task <int> Count([FromBody] OrderService_OrderServiceFilterDTO OrderService_OrderServiceFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			OrderServiceFilter OrderServiceFilter = ConvertFilterDTOToFilterEntity(OrderService_OrderServiceFilterDTO);
			int count = await OrderServiceService.Count(OrderServiceFilter);
			return count;
		}

		[Route(OrderServiceRoute.List), HttpPost]
		public async Task<List<OrderService_OrderServiceDTO>> List([FromBody] OrderService_OrderServiceFilterDTO OrderService_OrderServiceFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			OrderServiceFilter OrderServiceFilter = ConvertFilterDTOToFilterEntity(OrderService_OrderServiceFilterDTO);
			List<OrderService> OrderServices = await OrderServiceService.List(OrderServiceFilter);
			List<OrderService_OrderServiceDTO> OrderService_OrderServiceDTOs = OrderServices.Select(x => new OrderService_OrderServiceDTO(x)).ToList();
			return OrderService_OrderServiceDTOs;
		}

		[Route(OrderServiceRoute.Get), HttpPost]
		public async Task<ActionResult<OrderService_OrderServiceDTO>> Get([FromBody] OrderService_OrderServiceDTO OrderService_OrderServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			if (!await HasPermission(OrderService_OrderServiceDTO.Id))
				return Forbid();

			OrderService OrderService = await OrderServiceService.Get(OrderService_OrderServiceDTO.Id);
			return new OrderService_OrderServiceDTO(OrderService);

		}

		[Route(OrderServiceRoute.Create), HttpPost]
		public async Task<ActionResult<OrderService_OrderServiceDTO>> Create([FromBody] OrderService_OrderServiceDTO OrderService_OrderServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(OrderService_OrderServiceDTO.Id))
				return Forbid();
			OrderService OrderService = ConvertDTOToEntity(OrderService_OrderServiceDTO);

			OrderService = await OrderServiceService.Create(OrderService);
			OrderService_OrderServiceDTO = new OrderService_OrderServiceDTO(OrderService);
			if (OrderService.IsValidated)
				return OrderService_OrderServiceDTO;
			else
				return BadRequest(OrderService_OrderServiceDTO);
		}

		[Route(OrderServiceRoute.Update), HttpPost]
		public async Task<ActionResult<OrderService_OrderServiceDTO>> Update([FromBody] OrderService_OrderServiceDTO OrderService_OrderServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(OrderService_OrderServiceDTO.Id))
				return Forbid();

			OrderService OrderService = ConvertDTOToEntity(OrderService_OrderServiceDTO);

			OrderService = await OrderServiceService.Update(OrderService);
			OrderService_OrderServiceDTO = new OrderService_OrderServiceDTO(OrderService);
			if (OrderService.IsValidated)
				return OrderService_OrderServiceDTO;
			else
				return BadRequest(OrderService_OrderServiceDTO);
		}

		[Route(OrderServiceRoute.Delete), HttpPost]
		public async Task<ActionResult<OrderService_OrderServiceDTO>> Delete([FromBody] OrderService_OrderServiceDTO OrderService_OrderServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(OrderService_OrderServiceDTO.Id))
				return Forbid();
			OrderService OrderService = ConvertDTOToEntity(OrderService_OrderServiceDTO);

			OrderService = await OrderServiceService.Delete(OrderService);
			OrderService_OrderServiceDTO = new OrderService_OrderServiceDTO(OrderService);
			if (OrderService.IsValidated)
				return OrderService_OrderServiceDTO;
			else
				return BadRequest(OrderService_OrderServiceDTO);
		}

		private async Task<bool> HasPermission(long Id)
		{
			OrderServiceFilter OrderServiceFilter = new OrderServiceFilter();
			if (Id == 0)
			{

			}
			else
			{
				OrderServiceFilter.Id = new IdFilter { Equal = Id };
				int count = await OrderServiceService.Count(OrderServiceFilter);
				if (count == 0)
					return false;
			}
			return true;
		}
		private OrderService ConvertDTOToEntity(OrderService_OrderServiceDTO OrderService_OrderServiceDTO)
		{
			OrderService OrderService = new OrderService();
			OrderService.Id = OrderService_OrderServiceDTO.Id;
			OrderService.Code = OrderService_OrderServiceDTO.Code;
			OrderService.OrderDate = OrderService_OrderServiceDTO.OrderDate;
			OrderService.Total = OrderService_OrderServiceDTO.Total;
			OrderService.CustomerId = OrderService_OrderServiceDTO.CustomerId;
			OrderService.Used = OrderService_OrderServiceDTO.Used;
			OrderService.Customer = OrderService_OrderServiceDTO.Customer == null ? null : new Customer
			{
				Id = OrderService_OrderServiceDTO.Customer.Id,
				Code = OrderService_OrderServiceDTO.Customer.Code,
				Name = OrderService_OrderServiceDTO.Customer.Name,
				Phone = OrderService_OrderServiceDTO.Customer.Phone,
				Address = OrderService_OrderServiceDTO.Customer.Address,
				StatusId = OrderService_OrderServiceDTO.Customer.StatusId,
				Used = OrderService_OrderServiceDTO.Customer.Used,
			};
			
			OrderService.BaseLanguage = CurrentContext.Language;

			return OrderService;
		}
		private OrderServiceFilter ConvertFilterDTOToFilterEntity(OrderService_OrderServiceFilterDTO OrderService_OrderServiceFilterDTO)
		{
			OrderServiceFilter OrderServiceFilter = new OrderServiceFilter();
			//ProductFilter.Selects = ProductSelect.Code | ProductSelect.Name
			//    | ProductSelect.Id | ProductSelect.ProductProductGroupingMapping | ProductSelect.ProductType
			//    | ProductSelect.Status | ProductSelect.UsedVariation | ProductSelect.Category | ProductSelect.Brand;
			OrderServiceFilter.Selects = OrderServiceSelect.ALL;
			OrderServiceFilter.Skip = OrderService_OrderServiceFilterDTO.Skip;
			OrderServiceFilter.Take = OrderService_OrderServiceFilterDTO.Take;
			OrderServiceFilter.OrderBy = OrderService_OrderServiceFilterDTO.OrderBy;
			OrderServiceFilter.OrderType = OrderService_OrderServiceFilterDTO.OrderType;

			OrderServiceFilter.Id = OrderService_OrderServiceFilterDTO.Id;
			OrderServiceFilter.Code = OrderService_OrderServiceFilterDTO.Code;
			OrderServiceFilter.OrderDate = OrderService_OrderServiceFilterDTO.OrderDate;
			OrderServiceFilter.CustomerId = OrderService_OrderServiceFilterDTO.CustomerId;
			OrderServiceFilter.Total = OrderService_OrderServiceFilterDTO.Total;
			OrderServiceFilter.UpdateTime = OrderService_OrderServiceFilterDTO.UpdateTime;

			return OrderServiceFilter;
		}
	}
}
