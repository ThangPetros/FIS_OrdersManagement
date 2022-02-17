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
namespace SampleProject.Services.MOrderService
{
	public interface IOrderServiceService : IServiceScoped
	{
		Task<int> Count(OrderServiceFilter OrderServiceFilter);
		Task<List<OrderService>> List(OrderServiceFilter OrderServiceFilter);
		Task<OrderService> Get(long Id);
		Task<OrderService> Create(OrderService OrderService);
		Task<OrderService> Update(OrderService OrderService);
		Task<OrderService> Delete(OrderService OrderService);
		//Task<List<OrderService>> BulkDelete(List<OrderService> OrderServices);
		Task<List<OrderService>> BulkMerge(List<OrderService> OrderServices);
		//Task<List<OrderService>> Import(List<OrderService> OrderServices);
	}
	public class ResourceService : IOrderServiceService
	{
		private IUOW UOW;
		//private ILogging Logging;
		private ICurrentContext CurrentContext;
		private IOrderServiceValidator OrderServiceValidator;
		//private IRabbitManager RabbitManager;

		public ResourceService(
		    IUOW UOW,
		    //ILogging Logging,
		    ICurrentContext CurrentContext,
		    IOrderServiceValidator OrderServiceValidator
		    //IRabbitManager RabbitManager
		)
		{
			this.UOW = UOW;
			//this.Logging = Logging;
			this.CurrentContext = CurrentContext;
			this.OrderServiceValidator = OrderServiceValidator;
			//this.RabbitManager = RabbitManager;
		}
		public async Task<int> Count(OrderServiceFilter OrderServiceFilter)
		{
			try
			{
				int result = await UOW.OrderServiceRepository.Count(OrderServiceFilter);
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return 0;
		}

		public async Task<List<OrderService>> List(OrderServiceFilter OrderServiceFilter)
		{
			try
			{
				List<OrderService> OrderServices = await UOW.OrderServiceRepository.List(OrderServiceFilter);
				return OrderServices;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}
		public async Task<OrderService> Get(long Id)
		{
			OrderService OrderService = await UOW.OrderServiceRepository.Get(Id);
			if (OrderService == null)
				return null;
			return OrderService;
		}

		public async Task<OrderService> Create(OrderService OrderService)
		{
			if (!await OrderServiceValidator.Create(OrderService))
				return OrderService;

			try
			{
				await UOW.OrderServiceRepository.Create(OrderService);
				List<OrderService> OrderServices = await UOW.OrderServiceRepository.List(new List<long> { OrderService.Id });

				//Sync(OrderServices);
				OrderService = OrderServices.FirstOrDefault();
				//Logging.CreateAuditLog(OrderService, new { }, nameof(ResourceService));
				return OrderService;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<OrderService> Update(OrderService OrderService)
		{
			if (!await OrderServiceValidator.Update(OrderService))
				return OrderService;
			try
			{
				var oldData = await UOW.OrderServiceRepository.Get(OrderService.Id);
				await UOW.OrderServiceRepository.Update(OrderService);
				List<OrderService> OrderServices = await UOW.OrderServiceRepository.List(new List<long> { OrderService.Id });
				//Sync(OrderServices);
				OrderService = OrderServices.FirstOrDefault();

				//Logging.CreateAuditLog(OrderService, oldData, nameof(ResourceService));
				return OrderService;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<OrderService> Delete(OrderService OrderService)
		{
			if (!await OrderServiceValidator.Delete(OrderService))
				return OrderService;

			try
			{
				await UOW.OrderServiceRepository.Delete(OrderService);
				List<OrderService> OrderServices = await UOW.OrderServiceRepository.List(new List<long> { OrderService.Id });
				//Sync(OrderServices);
				OrderService = OrderServices.FirstOrDefault();
				//Logging.CreateAuditLog(new { }, OrderService, nameof(ResourceService));
				return OrderService;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
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

		public async Task<List<OrderService>> BulkMerge(List<OrderService> OrderServices)
		{
			if (!await OrderServiceValidator.BulkMerge(OrderServices))
				return OrderServices;
			try
			{
				await UOW.OrderServiceRepository.BulkMerge(OrderServices);
				List<long> Ids = OrderServices.Select(x => x.Id).ToList();
				OrderServices = await UOW.OrderServiceRepository.List(Ids);
				//Sync(OrderServices);
				//Logging.CreateAuditLog(OrderServices, new { }, nameof(ResourceService));
				return OrderServices;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(ResourceService));
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

		/*private void Sync(List<OrderService> OrderServices)
		{
			*//*foreach (var Customer in Customers)
			{
				Customer.AppUserId = CurrentContext.UserId;
			}*//*
			RabbitManager.PublishList(OrderServices, RoutingKeyEnum.OrderServiceSync.Code);
		}*/
	}
}
