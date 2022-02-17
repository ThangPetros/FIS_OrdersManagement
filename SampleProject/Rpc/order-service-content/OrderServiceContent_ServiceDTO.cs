using SampleProject.Entities;
using SampleProject.Rpc.service;
using System;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service_content
{
	public class OrderServiceContent_ServiceDTO:DataDTO
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public long UnitOfMeasureId { get; set; }
		public decimal Price { get; set; }
		public long StatusId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool Used { get; set; }
		public Service_StatusDTO Status { get; set; }
		public OrderServiceContent_UnitOfMeasureDTO UnitOfMeasure { get; set; }

		public OrderServiceContent_ServiceDTO() { }
		public OrderServiceContent_ServiceDTO(Service Service) {
			this.Id = Service.Id;
			this.Code = Service.Code;
			this.Name = Service.Name;
			this.UnitOfMeasureId = Service.UnitOfMeasureId;
			this.Price = Service.Price;
			this.StatusId = Service.StatusId;
			this.CreatedAt = Service.CreatedAt;
			this.UpdatedAt = Service.UpdatedAt;
			this.Used = Service.Used;
			this.Status = Service.Status == null ? null : new Service_StatusDTO(Service.Status);
			this.UnitOfMeasure = Service.UnitOfMeasure == null ? null : new OrderServiceContent_UnitOfMeasureDTO(Service.UnitOfMeasure);
			this.Errors = Service.Errors;
		}
	}
	public class OrderServiceContent_ServiceFilterDTO: FilterDTO
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public StringFilter Name { get; set; }
		public IdFilter UnitOfMeasureId { get; set; }
		public IdFilter StatusId { get; set; }
		public DecimalFilter Price { get; set; }
		public DateFilter CreatedAt { get; set; }
		public DateFilter UpdatedAt { get; set; }
		public ServiceOrder OrderBy { get; set; }
	}
}
