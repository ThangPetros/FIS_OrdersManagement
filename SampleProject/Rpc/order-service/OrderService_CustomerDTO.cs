using SampleProject.Entities;
using SampleProject.Rpc.customer;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace SampleProject.Rpc.order_service
{
	public class OrderService_CustomerDTO:DataDTO
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public long StatusId { get; set; }
		public bool Used { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdateAt { get; set; }
		public Customer_StatusDTO Status { get; set; }

		public OrderService_CustomerDTO() { }
		public OrderService_CustomerDTO(Customer Customer) {
			this.Id = Customer.Id;
			this.Code = Customer.Code;
			this.Name = Customer.Name;
			this.Address = Customer.Address;
			this.Phone = Customer.Phone;
			this.StatusId = Customer.StatusId;
			this.Used = Customer.Used;
			this.CreatedAt = Customer.CreatedAt;
			this.UpdateAt = Customer.UpdatedAt;
			this.Status = Customer.Status == null ? null : new Customer_StatusDTO(Customer.Status);
			this.Errors = Customer.Errors;
		}
	}
	public class OrderService_CustomerFilterDTO : FilterDTO
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public StringFilter Name { get; set; }
		public StringFilter Phone { get; set; }
		public StringFilter Address { get; set; }
		public IdFilter StatusId { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<CustomerFilter> OrFilter { get; set; }
		public CustomerOrder OrderBy { get; set; }
		public CustomerSelect Selects { get; set; }
	}
}
