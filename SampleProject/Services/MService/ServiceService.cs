using TrueSight.Common;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Repositories;
using SampleProject.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleProject.Enums;
using SampleProject.Handlers.Configuration;
using System.Linq;
namespace SampleProject.Services.MService
{
	public interface IServiceService : IServiceScoped
	{
		Task<int> Count(ServiceFilter ServiceFilter);
		Task<List<Service>> List(ServiceFilter ServiceFilter);
		Task<Service> Get(long Id);
		Task<Service> Create(Service Service);
		Task<Service> Update(Service Service);
		Task<Service> Delete(Service Service);
		//Task<List<Service>> BulkDelete(List<Service> Services);
		Task<List<Service>> BulkMerge(List<Service> Services);
		//Task<List<Service>> Import(List<Service> Services);
	}
	public class ResourceService : IServiceService
	{
		private IUOW UOW;
		private ILogging Logging;
		private ICurrentContext CurrentContext;
		private IServiceValidator ServiceValidator;
		private IRabbitManager RabbitManager;

		public ResourceService(
		    IUOW UOW,
		    ILogging Logging,
		    ICurrentContext CurrentContext,
		    IServiceValidator ServiceValidator,
		    IRabbitManager RabbitManager
		)
		{
			this.UOW = UOW;
			this.Logging = Logging;
			this.CurrentContext = CurrentContext;
			this.ServiceValidator = ServiceValidator;
			this.RabbitManager = RabbitManager;
		}
		public async Task<int> Count(ServiceFilter ServiceFilter)
		{
			try
			{
				int result = await UOW.ServiceRepository.Count(ServiceFilter);
				return result;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return 0;
		}

		public async Task<List<Service>> List(ServiceFilter ServiceFilter)
		{
			try
			{
				List<Service> Services = await UOW.ServiceRepository.List(ServiceFilter);
				return Services;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}
		public async Task<Service> Get(long Id)
		{
			Service Service = await UOW.ServiceRepository.Get(Id);
			if (Service == null)
				return null;
			return Service;
		}

		public async Task<Service> Create(Service Service)
		{
			if (!await ServiceValidator.Create(Service))
				return Service;

			try
			{
				await UOW.ServiceRepository.Create(Service);
				List<Service> Services = await UOW.ServiceRepository.List(new List<long> { Service.Id });

				Sync(Services);
				Service = Services.FirstOrDefault();
				Logging.CreateAuditLog(Service, new { }, nameof(ResourceService));
				return Service;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<Service> Update(Service Service)
		{
			if (!await ServiceValidator.Update(Service))
				return Service;
			try
			{
				var oldData = await UOW.ServiceRepository.Get(Service.Id);
				await UOW.ServiceRepository.Update(Service);
				List<Service> Services = await UOW.ServiceRepository.List(new List<long> { Service.Id });
				Sync(Services);
				Service = Services.FirstOrDefault();

				Logging.CreateAuditLog(Service, oldData, nameof(ResourceService));
				return Service;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<Service> Delete(Service Service)
		{
			if (!await ServiceValidator.Delete(Service))
				return Service;

			try
			{
				await UOW.ServiceRepository.Delete(Service);
				List<Service> Services = await UOW.ServiceRepository.List(new List<long> { Service.Id });
				Sync(Services);
				Service = Services.FirstOrDefault();
				Logging.CreateAuditLog(new { }, Service, nameof(ResourceService));
				return Service;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		#region BulkDelete
		/*public async Task<List<Customer>> BulkDelete(List<Customer> Customers)
		{
			if (!await CustomerValidator.BulkDelete(Customers))
				return Customers;

			try
			{
				await UOW.CustomerRepository.BulkDelete(Customers);
				List<long> Ids = Customers.Select(x => x.Id).ToList();
				Customers = await UOW.CustomerRepository.List(Ids);
				Sync(Customers);

				Logging.CreateAuditLog(new { }, Customers, nameof(ResourceService));
				return Customers;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}*/
		#endregion

		public async Task<List<Service>> BulkMerge(List<Service> Services)
		{
			if (!await ServiceValidator.BulkMerge(Services))
				return Services;
			try
			{
				await UOW.ServiceRepository.BulkMerge(Services);
				List<long> Ids = Services.Select(x => x.Id).ToList();
				Services = await UOW.ServiceRepository.List(Ids);
				Sync(Services);
				Logging.CreateAuditLog(Services, new { }, nameof(ResourceService));
				return Services;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		#region Import
		/*public async Task<List<Customer>> Import(List<Customer> Customers)
		{
			if (!await CustomerValidator.Import(Customers))
				return Customers;

			try
			{
				await UOW.CustomerRepository.BulkMerge(Customers);
				List<long> Ids = Customers.Select(x => x.Id).ToList();
				Customers = await UOW.CustomerRepository.List(Ids);
				Sync(Customers);

				Logging.CreateAuditLog(Customers, new { }, nameof(ResourceService));
				return Customers;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}*/
		#endregion

		private void Sync(List<Service> Services)
		{
			/*foreach (var Customer in Customers)
			{
				Customer.AppUserId = CurrentContext.UserId;
			}*/
			RabbitManager.PublishList(Services, RoutingKeyEnum.ServiceSync.Code);
		}
	}
}
