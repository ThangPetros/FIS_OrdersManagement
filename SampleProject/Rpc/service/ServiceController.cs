using TrueSight.Common;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Services.MStatus;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SampleProject.Services.MService;
using SampleProject.Services.MUnitOfMeasure;

namespace SampleProject.Rpc.service
{
	public class ServiceController: RpcController
	{
		private IStatusService StatusService;
		private IUnitOfMeasureService UnitOfMeasureService; 
		private IServiceService ServiceService;
		private ICurrentContext CurrentContext;
		public ServiceController(
		    IStatusService StatusService,
		    IUnitOfMeasureService UnitOfMeasureService,
		    IServiceService ServiceService,
		    ICurrentContext CurrentContext)
		{
			this.StatusService = StatusService;
			this.UnitOfMeasureService = UnitOfMeasureService;
			this.ServiceService = ServiceService;
			this.CurrentContext = CurrentContext;
		}

		[Route(ServiceRoute.Count),HttpPost]
		public async Task<int> Count([FromBody] Service_ServiceFilterDTO Service_ServiceFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			ServiceFilter ServiceFilter = ConvertFilterDTOToFilterEntity(Service_ServiceFilterDTO);
			int count = await ServiceService.Count(ServiceFilter);
			return count;
		}

		[Route(ServiceRoute.List),HttpPost]
		public async Task<List<Service_ServiceDTO>> List([FromBody] Service_ServiceFilterDTO Service_ServiceFilterDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			ServiceFilter ServiceFilter = ConvertFilterDTOToFilterEntity(Service_ServiceFilterDTO);
			List<Service> Services = await ServiceService.List(ServiceFilter);
			List<Service_ServiceDTO> Service_ServiceDTOs = Services.Select(x=> new Service_ServiceDTO(x)).ToList();
			return Service_ServiceDTOs;
		}

		[Route(ServiceRoute.Get),HttpPost]
		public async Task<ActionResult<Service_ServiceDTO>> Get([FromBody] Service_ServiceDTO Service_ServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);
			if (!await HasPermission(Service_ServiceDTO.Id))
				return Forbid();

			Service Service = await ServiceService.Get(Service_ServiceDTO.Id);
			return new Service_ServiceDTO(Service);

		}

		[Route(ServiceRoute.Create), HttpPost]
		public async Task<ActionResult<Service_ServiceDTO>> Create([FromBody] Service_ServiceDTO Service_ServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Service_ServiceDTO.Id))
				return Forbid();
			Service Service = ConvertDTOToEntity(Service_ServiceDTO);

			Service = await ServiceService.Create(Service);
			Service_ServiceDTO = new Service_ServiceDTO(Service);
			if (Service.IsValidated)
				return Service_ServiceDTO;
			else
				return BadRequest(Service_ServiceDTO);
		}

		[Route(ServiceRoute.Update), HttpPost]
		public async Task<ActionResult<Service_ServiceDTO>> Update([FromBody] Service_ServiceDTO Service_ServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Service_ServiceDTO.Id))
				return Forbid();

			Service Service = ConvertDTOToEntity(Service_ServiceDTO);

			Service = await ServiceService.Update(Service);
			Service_ServiceDTO = new Service_ServiceDTO(Service);
			if (Service.IsValidated)
				return Service_ServiceDTO;
			else
				return BadRequest(Service_ServiceDTO);
		}

		[Route(ServiceRoute.Delete), HttpPost]
		public async Task<ActionResult<Service_ServiceDTO>> Delete([FromBody] Service_ServiceDTO Service_ServiceDTO)
		{
			if (!ModelState.IsValid)
				throw new BindException(ModelState);

			if (!await HasPermission(Service_ServiceDTO.Id))
				return Forbid();
			Service Service = ConvertDTOToEntity(Service_ServiceDTO);

			Service = await ServiceService.Delete(Service);
			Service_ServiceDTO = new Service_ServiceDTO(Service);
			if (Service.IsValidated)
				return Service_ServiceDTO;
			else
				return BadRequest(Service_ServiceDTO);
		}
		private async Task<bool> HasPermission(long Id)
		{
			ServiceFilter ServiceFilter = new ServiceFilter();
			if (Id == 0)
			{

			}
			else
			{
				ServiceFilter.Id = new IdFilter { Equal = Id };
				int count = await ServiceService.Count(ServiceFilter);
				if (count == 0)
					return false;
			}
			return true;
		}
		private Service ConvertDTOToEntity(Service_ServiceDTO Service_ServiceDTO)
		{
			Service Service = new Service();
			Service.Id = Service_ServiceDTO.Id;
			Service.Code = Service_ServiceDTO.Code;
			Service.Name = Service_ServiceDTO.Name;
			Service.UnitOfMeasureId = Service_ServiceDTO.UnitOfMeasureId;
			Service.Price = Service_ServiceDTO.Price;
			Service.StatusId = Service_ServiceDTO.StatusId;
			Service.Status = Service_ServiceDTO.Status == null ? null : new Status
			{
				Id = Service_ServiceDTO.Status.Id,
				Code = Service_ServiceDTO.Status.Code,
				Name = Service_ServiceDTO.Status.Name
			};
			Service.UnitOfMeasure = Service_ServiceDTO.UnitOfMeasure == null ? null : new UnitOfMeasure
			{
				Id = Service_ServiceDTO.UnitOfMeasure.Id,
				Code = Service_ServiceDTO.UnitOfMeasure.Code,
				Name = Service_ServiceDTO.Status.Name,
				StatusId = Service_ServiceDTO.UnitOfMeasure.StatusId
			};

			Service.BaseLanguage = CurrentContext.Language;

			return Service;
		}
		private ServiceFilter ConvertFilterDTOToFilterEntity(Service_ServiceFilterDTO Service_ServiceFilterDTO)
		{
			ServiceFilter ServiceFilter = new ServiceFilter();
			//ProductFilter.Selects = ProductSelect.Code | ProductSelect.Name
			//    | ProductSelect.Id | ProductSelect.ProductProductGroupingMapping | ProductSelect.ProductType
			//    | ProductSelect.Status | ProductSelect.UsedVariation | ProductSelect.Category | ProductSelect.Brand;
			ServiceFilter.Selects = ServiceSelect.ALL;
			ServiceFilter.Skip = Service_ServiceFilterDTO.Skip;
			ServiceFilter.Take = Service_ServiceFilterDTO.Take;
			ServiceFilter.OrderBy = Service_ServiceFilterDTO.OrderBy;
			ServiceFilter.OrderType = Service_ServiceFilterDTO.OrderType;

			ServiceFilter.Id = Service_ServiceFilterDTO.Id;
			ServiceFilter.Code = Service_ServiceFilterDTO.Code;
			ServiceFilter.Name = Service_ServiceFilterDTO.Name;
			ServiceFilter.UnitOfMeasureId = Service_ServiceFilterDTO.UnitOfMeasureId;
			ServiceFilter.Price = Service_ServiceFilterDTO.Price;
			ServiceFilter.StatusId = Service_ServiceFilterDTO.StatusId;
			ServiceFilter.UpdateTime = Service_ServiceFilterDTO.UpdatedAt;

			return ServiceFilter;
		}
	}
}
