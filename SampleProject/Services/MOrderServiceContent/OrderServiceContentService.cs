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

namespace SampleProject.Services.MOrderServiceContent
{
	public interface IOrderServiceContentService : IServiceScoped
	{
		Task<int> Count(OrderServiceContentFilter OrderServiceContentFilter);
		Task<List<OrderServiceContent>> List(OrderServiceContentFilter OrderServiceContentFilter);
		Task<OrderServiceContent> Get(long Id);
		Task<OrderServiceContent> Create(OrderServiceContent OrderServiceContent);
		Task<OrderServiceContent> Update(OrderServiceContent OrderServiceContent);
	}
	public class ResourceService : IOrderServiceContentService
	{
		private IUOW UOW;
		private ILogging Logging;
		private ICurrentContext CurrentContext;
		private IOrderServiceContentValidator OrderServiceContentValidator;
		private IRabbitManager RabbitManager;

		public ResourceService(
		    IUOW UOW,
		    ILogging Logging,
		    ICurrentContext CurrentContext,
		    IOrderServiceContentValidator OrderServiceContentValidator,
		    IRabbitManager RabbitManager
		)
		{
			this.UOW = UOW;
			this.Logging = Logging;
			this.CurrentContext = CurrentContext;
			this.OrderServiceContentValidator = OrderServiceContentValidator;
			this.RabbitManager = RabbitManager;
		}
		public async Task<int> Count(OrderServiceContentFilter OrderServiceContentFilter)
		{
			try
			{
				int result = await UOW.OrderServiceContentRepository.Count(OrderServiceContentFilter);
				return result;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return 0;
		}

		public async Task<List<OrderServiceContent>> List(OrderServiceContentFilter OrderServiceContentFilter)
		{
			try
			{
				List<OrderServiceContent> OrderServiceContents = await UOW.OrderServiceContentRepository.List(OrderServiceContentFilter);
				return OrderServiceContents;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}
		public async Task<OrderServiceContent> Get(long Id)
		{
			OrderServiceContent OrderServiceContent = await UOW.OrderServiceContentRepository.Get(Id);
			if (OrderServiceContent == null)
				return null;
			return OrderServiceContent;
		}

		public async Task<OrderServiceContent> Create(OrderServiceContent OrderServiceContent)
		{
			if (!await OrderServiceContentValidator.Create(OrderServiceContent))
				return OrderServiceContent;

			try
			{
				await UOW.OrderServiceContentRepository.Create(OrderServiceContent);
				List<OrderServiceContent> OrderServiceContents = await UOW.OrderServiceContentRepository.List(new List<long> { OrderServiceContent.Id });

				Sync(OrderServiceContents);
				OrderServiceContent = OrderServiceContents.FirstOrDefault();
				Logging.CreateAuditLog(OrderServiceContent, new { }, nameof(ResourceService));
				return OrderServiceContent;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		public async Task<OrderServiceContent> Update(OrderServiceContent OrderServiceContent)
		{
			if (!await OrderServiceContentValidator.Update(OrderServiceContent))
				return OrderServiceContent;
			try
			{
				var oldData = await UOW.OrderServiceContentRepository.Get(OrderServiceContent.Id);
				await UOW.OrderServiceContentRepository.Update(OrderServiceContent);
				List<OrderServiceContent> OrderServiceContents = await UOW.OrderServiceContentRepository.List(new List<long> { OrderServiceContent.Id });
				Sync(OrderServiceContents);
				OrderServiceContent = OrderServiceContents.FirstOrDefault();

				Logging.CreateAuditLog(OrderServiceContent, oldData, nameof(ResourceService));
				return OrderServiceContent;
			}
			catch (Exception ex)
			{
				Logging.CreateSystemLog(ex, nameof(ResourceService));
			}
			return null;
		}

		private void Sync(List<OrderServiceContent> OrderServiceContents)
		{
			RabbitManager.PublishList(OrderServiceContents, RoutingKeyEnum.ServiceSync.Code);
		}
	}
}
