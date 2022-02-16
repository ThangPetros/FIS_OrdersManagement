using TrueSight.Common;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Services.MCustomer;
using SampleProject.Services.MStatus;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SampleProject.Rpc.customer
{
	public class CustomerController : RpcController
	{
		private IStatusService StatusService;
		private ICustomerService CustomerService;
		private ICurrentContext CurrentContext;
		public CustomerController(
		    IStatusService StatusService,
		    ICustomerService CustomerService,
		    ICurrentContext CurrentContext)
		{
			this.StatusService = StatusService;
			this.CustomerService = CustomerService;
			this.CurrentContext = CurrentContext;
		}
		[Route(CustomerRoute.Count), HttpPost]
		public async Task<ActionResult<int>> Count([FromBody] Customer_CustomerFilterDTO Customer_CustomerFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			CustomerFilter CustomerFilter = ConvertFilterDTOToFilterEntity(Customer_CustomerFilterDTO);
			//CustomerFilter = CustomerService.ToFilter(CustomerFilter);
			int count = await CustomerService.Count(CustomerFilter);
			return count;
		}
		[Route(CustomerRoute.List), HttpPost]
		public async Task<ActionResult<List<Customer_CustomerDTO>>> List([FromBody] Customer_CustomerFilterDTO Customer_CustomerFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			CustomerFilter CustomerFilter = ConvertFilterDTOToFilterEntity(Customer_CustomerFilterDTO);
			List<Customer> Customers = await CustomerService.List(CustomerFilter);

			List<Customer_CustomerDTO> Customer_CustomerDTOs = Customers.Select(x => new Customer_CustomerDTO(x)).ToList();

			return Customer_CustomerDTOs;
		}
		[Route(CustomerRoute.Get), HttpPost]
		public async Task<ActionResult<Customer_CustomerDTO>> Get([FromBody] Customer_CustomerDTO Customer_CustomerDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			if (!await HasPermission(Customer_CustomerDTO.Id))
				return Forbid();
			Customer Customer = await CustomerService.Get(Customer_CustomerDTO.Id);
			return new Customer_CustomerDTO(Customer);
		}

		[Route(CustomerRoute.Create),HttpPost]
		public async Task<ActionResult<Customer_CustomerDTO>> Create([FromBody] Customer_CustomerDTO Customer_CustomerDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Customer_CustomerDTO.Id))
				return Forbid();
			Customer Customer = ConvertDTOToEntity(Customer_CustomerDTO);

			Customer = await CustomerService.Create(Customer);
			Customer_CustomerDTO = new Customer_CustomerDTO(Customer);
			if (Customer.IsValidated)
				return Customer_CustomerDTO;
			else
				return BadRequest(Customer_CustomerDTO);
		}

		[Route(CustomerRoute.Update), HttpPost]
		public async Task<ActionResult<Customer_CustomerDTO>> Update([FromBody] Customer_CustomerDTO Customer_CustomerDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Customer_CustomerDTO.Id))
				return Forbid();

			Customer Customer = ConvertDTOToEntity(Customer_CustomerDTO);

			Customer = await CustomerService.Update(Customer);
			Customer_CustomerDTO = new Customer_CustomerDTO(Customer);
			if (Customer.IsValidated)
				return Customer_CustomerDTO;
			else
				return BadRequest(Customer_CustomerDTO);
		}

		[Route(CustomerRoute.Delete), HttpPost]
		public async Task<ActionResult<Customer_CustomerDTO>> Delete([FromBody] Customer_CustomerDTO Customer_CustomerDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Customer_CustomerDTO.Id))
				return Forbid();
			Customer Customer = ConvertDTOToEntity(Customer_CustomerDTO);

			Customer = await CustomerService.Delete(Customer);
			Customer_CustomerDTO = new Customer_CustomerDTO(Customer);
			if (Customer.IsValidated)
				return Customer_CustomerDTO;
			else
				return BadRequest(Customer_CustomerDTO);
		}


		private async Task<bool> HasPermission(long Id)
		{
			CustomerFilter CustomerFilter = new CustomerFilter();
			if (Id == 0)
			{

			}
			else
			{
				CustomerFilter.Id = new IdFilter { Equal = Id };
				int count = await CustomerService.Count(CustomerFilter);
				if (count == 0)
					return false;
			}
			return true;
		}

		private Customer ConvertDTOToEntity(Customer_CustomerDTO Customer_CustomerDTO)
		{
			Customer Customer = new Customer();
			Customer.Id = Customer_CustomerDTO.Id;
			Customer.Code = Customer_CustomerDTO.Code;
			Customer.Name = Customer_CustomerDTO.Name;
			Customer.Phone = Customer_CustomerDTO.Phone;
			Customer.Address = Customer_CustomerDTO.Address;
			Customer.StatusId = Customer_CustomerDTO.StatusId;
			Customer.UpdatedAt = Customer_CustomerDTO.UpdateAt;
			Customer.Status = Customer_CustomerDTO.Status == null ? null : new Status
			{
				Id = Customer_CustomerDTO.Status.Id,
				Code = Customer_CustomerDTO.Status.Code,
				Name = Customer_CustomerDTO.Status.Name,
			};
			Customer.BaseLanguage = CurrentContext.Language;
			return Customer;
		}
		private CustomerFilter ConvertFilterDTOToFilterEntity(Customer_CustomerFilterDTO Customer_CustomerFilterDTO)
		{
			CustomerFilter CustomerFilter = new CustomerFilter();
			//ProductFilter.Selects = ProductSelect.Code | ProductSelect.Name
			//    | ProductSelect.Id | ProductSelect.ProductProductGroupingMapping | ProductSelect.ProductType
			//    | ProductSelect.Status | ProductSelect.UsedVariation | ProductSelect.Category | ProductSelect.Brand;
			CustomerFilter.Selects = CustomerSelect.ALL;
			CustomerFilter.Skip = Customer_CustomerFilterDTO.Skip;
			CustomerFilter.Take = Customer_CustomerFilterDTO.Take;
			CustomerFilter.OrderBy = Customer_CustomerFilterDTO.OrderBy;
			CustomerFilter.OrderType = Customer_CustomerFilterDTO.OrderType;

			CustomerFilter.Id = Customer_CustomerFilterDTO.Id;
			CustomerFilter.Code = Customer_CustomerFilterDTO.Code;
			CustomerFilter.Name = Customer_CustomerFilterDTO.Name;
			CustomerFilter.Phone = Customer_CustomerFilterDTO.Phone;
			CustomerFilter.Address = Customer_CustomerFilterDTO.Address;
			CustomerFilter.StatusId = Customer_CustomerFilterDTO.StatusId;
			CustomerFilter.UpdateTime = Customer_CustomerFilterDTO.UpdateTime;
			return CustomerFilter;
		}
	}
}
