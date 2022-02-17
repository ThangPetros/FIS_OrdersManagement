using SampleProject.Entities;
using System;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service_content
{
	public class OrderServiceContent_OrderServiceContentDTO: DataDTO
	{
            public long Id { get; set; }
            public long ServiceId { get; set; }
            public long OrderServiceId { get; set; }
            public long PrimaryUnitOfMeasureId { get; set; }
            public long UnitOfMeasureId { get; set; }
            public long Quantity { get; set; }
            public long RequestQuantity { get; set; }
            public decimal Price { get; set; }
            public decimal Amount { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public OrderServiceContent_ServiceDTO Service { get; set; }
            public OrderServiceContent_OrderServiceDTO OrderService { get; set; }
            public OrderServiceContent_UnitOfMeasureDTO PrimaryUnitOfMeasure { get; set; }
            public OrderServiceContent_UnitOfMeasureDTO UnitOfMeasure { get; set; }
            public OrderServiceContent_OrderServiceContentDTO() { }
            public OrderServiceContent_OrderServiceContentDTO(OrderServiceContent OrderServiceContent) {
                  this.Id = OrderServiceContent.Id;
                  this.ServiceId = OrderServiceContent.ServiceId;
                  this.OrderServiceId = OrderServiceContent.OrderServiceId;
                  this.PrimaryUnitOfMeasureId = OrderServiceContent.PrimaryUnitOfMeasureId;
                  this.UnitOfMeasureId = OrderServiceContent.UnitOfMeasureId;
                  this.Quantity = OrderServiceContent.Quantity;
                  this.RequestQuantity = OrderServiceContent.RequestQuantity;
                  this.Price = OrderServiceContent.Price;
                  this.Amount = OrderServiceContent.Amount;
                  this.CreatedAt = OrderServiceContent.CreatedAt;
                  this.UpdatedAt = OrderServiceContent.UpdatedAt;
                  this.Service = OrderServiceContent.Service == null ? null : new OrderServiceContent_ServiceDTO(OrderServiceContent.Service);
                  this.OrderService = OrderServiceContent.OrderService == null ? null : new OrderServiceContent_OrderServiceDTO(OrderServiceContent.OrderService);
                  this.PrimaryUnitOfMeasure = OrderServiceContent.PrimaryUnitOfMeasure == null ? null : new OrderServiceContent_UnitOfMeasureDTO(OrderServiceContent.PrimaryUnitOfMeasure);
                  this.UnitOfMeasure = OrderServiceContent.UnitOfMeasure == null ? null : new OrderServiceContent_UnitOfMeasureDTO(OrderServiceContent.UnitOfMeasure);
                  this.Errors = OrderServiceContent.Errors;
            }
      }
      public class OrderServiceContent_OrderServiceContentFilterDTO : FilterDTO
	{
            public IdFilter Id { get; set; }
            public IdFilter ServiceId { get; set; }
            public IdFilter OrderServiceId { get; set; }
            public IdFilter UnitOfMeasureId { get; set; }
            public IdFilter PrimaryUnitOfMeasureId { get; set; }
            public LongFilter Quantity { get; set; }
            public LongFilter RequestQuantity { get; set; }
            public DecimalFilter Price { get; set; }
            public DecimalFilter Amount { get; set; }
            public DateFilter UpdateTime { get; set; }
            public List<OrderServiceContentFilter> OrFilter { get; set; }
            public OrderServiceContentOrder OrderBy { get; set; }
            public OrderServiceContentSelect Selects { get; set; }
      }
}
