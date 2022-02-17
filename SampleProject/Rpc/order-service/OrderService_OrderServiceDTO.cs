using SampleProject.Entities;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service
{
	public class OrderService_OrderServiceDTO:DataDTO
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public DateTime OrderDate { get; set; }
		public long CustomerId { get; set; }
		public decimal Total { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool Used { get; set; }
		public OrderService_CustomerDTO Customer { get; set; }

		public OrderService_OrderServiceDTO() { }
		public OrderService_OrderServiceDTO(OrderService OrderService)
		{
			this.Id = OrderService.Id;
			this.Code = OrderService.Code;
			this.OrderDate = OrderService.OrderDate;
			this.CustomerId = OrderService.CustomerId;
			this.Total = OrderService.Total;
			this.CreatedAt = OrderService.CreatedAt;
			this.UpdatedAt = OrderService.UpdatedAt;
			this.Used = OrderService.Used;
			this.Customer = OrderService.Customer == null ? null : new OrderService_CustomerDTO(OrderService.Customer);
			this.Errors = OrderService.Errors;
		}
	}
	public class OrderService_OrderServiceFilterDTO : FilterDTO
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public DateFilter OrderDate { get; set; }
		public IdFilter CustomerId { get; set; }
		public DecimalFilter Total { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<OrderServiceFilter> OrFilter { get; set; }
		public OrderServiceOrder OrderBy { get; set; }
		public OrderServiceSelect Selects { get; set; }
	}
}
