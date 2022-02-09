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
namespace SampleProject.Services.MCustomer
{
	public interface ICustomerService : IServiceScoped
	{
		Task<int> Count(CustomerFilter BrandFilter);
		Task<List<Customer>> List(CustomerFilter BrandFilter);
		Task<Customer> Get(long Id);
		Task<Customer> Create(Customer Customer);
		Task<Customer> Update(Customer Customer);
		Task<Customer> Delete(Customer Customer);
		//Task<List<Customer>> BulkDelete(List<Customer> Customers);
		Task<List<Customer>> BulkMerge(List<Customer> Customers);
		//Task<List<Customer>> Import(List<Customer> Customers);
	}
	public class ResourceService : ICustomerService
	{
		private IUOW UOW;
		private ILogging Logging;
		private ICurrentContext CurrentContext;
		private ICustomerValidator CustomerValidator;
		private IRabbitManager RabbitManager;

		public ResourceService(
		    IUOW UOW,
		    ILogging Logging,
		    ICurrentContext CurrentContext,
		    ICustomerValidator CustomerValidator,
		    IRabbitManager RabbitManager
		)
		{
			this.UOW = UOW;
			this.Logging = Logging;
			this.CurrentContext = CurrentContext;
			this.CustomerValidator = CustomerValidator;
			this.RabbitManager = RabbitManager;
		}
		public async Task<int> Count(CustomerFilter CustomerFilter)
		{
			try
			{
				int result = await UOW.CustomerRepository.Count(CustomerFilter);
				return result;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return 0;
		}

		public async Task<List<Customer>> List(CustomerFilter CustomerFilter)
		{
			try
			{
				List<Customer> Customers = await UOW.CustomerRepository.List(CustomerFilter);
				return Customers;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}
		public async Task<Customer> Get(long Id)
		{
			Customer Customer = await UOW.CustomerRepository.Get(Id);
			if (Customer == null)
				return null;
			return Customer;
		}

		public async Task<Customer> Create(Customer Customer)
		{
			if (!await CustomerValidator.Create(Customer))
				return Customer;

			try
			{
				await UOW.CustomerRepository.Create(Customer);
				List<Customer> Customers = await UOW.CustomerRepository.List(new List<long> { Customer.Id });

				Sync(Customers);
				Customer = Customers.FirstOrDefault();
				Logging.CreateAuditLog(Customer, new { }, nameof(ResourceService));
				return Customer;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<Customer> Update(Customer Customer)
		{
			if (!await CustomerValidator.Update(Customer))
				return Customer;
			try
			{
				var oldData = await UOW.CustomerRepository.Get(Customer.Id);
				await UOW.CustomerRepository.Update(Customer);
				List<Customer> Customers = await UOW.CustomerRepository.List(new List<long> { Customer.Id });
				Sync(Customers);
				Customer = Customers.FirstOrDefault();

				Logging.CreateAuditLog(Customer, oldData, nameof(ResourceService));
				return Customer;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<Customer> Delete(Customer Customer)
		{
			if (!await CustomerValidator.Delete(Customer))
				return Customer;

			try
			{
				await UOW.CustomerRepository.Delete(Customer);
				List<Customer> Customers = await UOW.CustomerRepository.List(new List<long> { Customer.Id });
				Sync(Customers);
				Customer = Customers.FirstOrDefault();
				Logging.CreateAuditLog(new { }, Customer, nameof(ResourceService));
				return Customer;
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

		public async Task<List<Customer>> BulkMerge(List<Customer> Customers)
		{
			if (!await CustomerValidator.BulkMerge(Customers))
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

		private void Sync(List<Customer> Customers)
		{
			/*foreach (var Customer in Customers)
			{
				Customer.AppUserId = CurrentContext.UserId;
			}*/
			RabbitManager.PublishList(Customers, RoutingKeyEnum.CustomerSync.Code);
		}
	}
}
