using TrueSight.Common;
using SampleProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleProject.Helper;
using System;
using SampleProject.Entities;

namespace SampleProject.Repositories
{
	public interface IOrderServiceContentRepository
	{
		Task<int> Count(OrderServiceContentFilter OrderServiceContentFilter);
		Task<List<OrderServiceContent>> List(OrderServiceContentFilter OrderServiceContentFilter);
		Task<List<OrderServiceContent>> List(List<long> Ids);
		Task<OrderServiceContent> Get(long Id);
		/*Task<bool> Create(OrderServiceContent OrderServiceContent);
		Task<bool> Update(OrderServiceContent OrderServiceContent);
		Task<bool> Delete(OrderServiceContent OrderServiceContent);
		Task<bool> BulkMerge(List<OrderServiceContent> OrderServiceContents);
		Task<bool> BulkDelete(List<OrderServiceContent> OrderServiceContents);*/
	}
	public class OrderServiceContentRepository : IOrderServiceContentRepository
	{
		private DataContext DataContext;
		public OrderServiceContentRepository(DataContext dataContext)
		{
			this.DataContext = dataContext;
		}
		private IQueryable<OrderServiceContentDAO> DynamicFilter(IQueryable<OrderServiceContentDAO> query, OrderServiceContentFilter filter)
		{
			if (filter == null)
				return query.Where(q => false);
			//query = query.Where(q => !q.DeleteAt.HasValue);
			//query = query.Where(q => q.Id, filter.Id);
			query = query.Where(q => q.ServiceId, filter.ServiceId);
			query = query.Where(q => q.OrderServiceId, filter.OrderServiceId);
			query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
			query = query.Where(q => q.PrimaryUnitOfMeasureId, filter.PrimaryUnitOfMeasureId);
			query = query.Where(q => q.Quantity, filter.Quantity);
			query = query.Where(q => q.RequestQuantity, filter.RequestQuantity);
			query = query.Where(q => q.Prive, filter.Prive);
			query = query.Where(q => q.Amount, filter.Amount);

			query = OrFilter(query, filter);
			return query;
		}
		private IQueryable<OrderServiceContentDAO> OrFilter(IQueryable<OrderServiceContentDAO> query, OrderServiceContentFilter filter)
		{
			if (filter.OrFilter == null || filter.OrFilter.Count == 0)
				return query;
			IQueryable<OrderServiceContentDAO> initQuery = query.Where(q => false);
			foreach (OrderServiceContentFilter OrderServiceFilter in filter.OrFilter)
			{
				IQueryable<OrderServiceContentDAO> queryable = query;
				//queryable = queryable.Where(q => q.Id, OrderServiceFilter.Id);
				queryable = queryable.Where(q => q.ServiceId, filter.ServiceId);
				queryable = queryable.Where(q => q.OrderServiceId, filter.OrderServiceId);
				queryable = queryable.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
				queryable = queryable.Where(q => q.PrimaryUnitOfMeasureId, filter.PrimaryUnitOfMeasureId);
				queryable = queryable.Where(q => q.Quantity, filter.Quantity);
				queryable = queryable.Where(q => q.RequestQuantity, filter.RequestQuantity);
				queryable = queryable.Where(q => q.Prive, filter.Prive);
				queryable = queryable.Where(q => q.Amount, filter.Amount);
				initQuery = initQuery.Union(queryable);
			}
			return initQuery;
		}
		private IQueryable<OrderServiceContentDAO> DynamicOrder(IQueryable<OrderServiceContentDAO> query, OrderServiceContentFilter filter)
		{
			switch (filter.OrderType)
			{
				case OrderType.ASC:
					switch (filter.OrderBy)
					{
						case OrderServiceContentOrder.Service:
							query = query.OrderBy(q => q.ServiceId);
							break;
						case OrderServiceContentOrder.OrderService:
							query = query.OrderBy(q => q.OrderServiceId);
							break;
						case OrderServiceContentOrder.UnitOfMeasure:
							query = query.OrderBy(q => q.UnitOfMeasureId);
							break;
						case OrderServiceContentOrder.PrimaryUnitOfMeasure:
							query = query.OrderBy(q => q.PrimaryUnitOfMeasureId);
							break;
						case OrderServiceContentOrder.Quantity:
							query = query.OrderBy(q => q.Quantity);
							break;
						case OrderServiceContentOrder.Prive:
							query = query.OrderBy(q => q.Prive);
							break;
						default:
							query = query.OrderBy(q => q.Id);
							break;
					}
					break;
				case OrderType.DESC:
					switch (filter.OrderBy)
					{
						case OrderServiceContentOrder.Service:
							query = query.OrderByDescending(q => q.ServiceId);
							break;
						case OrderServiceContentOrder.OrderService:
							query = query.OrderByDescending(q => q.OrderServiceId);
							break;
						case OrderServiceContentOrder.UnitOfMeasure:
							query = query.OrderByDescending(q => q.UnitOfMeasureId);
							break;
						case OrderServiceContentOrder.PrimaryUnitOfMeasure:
							query = query.OrderByDescending(q => q.PrimaryUnitOfMeasureId);
							break;
						case OrderServiceContentOrder.Quantity:
							query = query.OrderByDescending(q => q.Quantity);
							break;
						case OrderServiceContentOrder.Prive:
							query = query.OrderByDescending(q => q.Prive);
							break;
						default:
							query = query.OrderByDescending(q => q.Id);
							break;
					}
					break;
			}
			query = query.Skip(filter.Skip).Take(filter.Take);
			return query;
		}
		private async Task<List<OrderServiceContent>> DynamicSelect(IQueryable<OrderServiceContentDAO> query, OrderServiceContentFilter filter)
		{
			List<OrderServiceContent> OrderServiceContents = await query.Select(q => new OrderServiceContent()
			{
				//Id = filter.Selects.Contains(OrderServiceContentSelect.Id) ? q.Id : default(long),
				ServiceId = filter.Selects.Contains(OrderServiceContentSelect.Service) ? q.ServiceId : default(long),
				OrderServiceId = filter.Selects.Contains(OrderServiceContentSelect.OrderService) ? q.OrderServiceId : default(long),
				UnitOfMeasureId = filter.Selects.Contains(OrderServiceContentSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
				PrimaryUnitOfMeasureId = filter.Selects.Contains(OrderServiceContentSelect.PrimaryUnitOfMeasure) ? q.PrimaryUnitOfMeasureId : default(long),
				Quantity = filter.Selects.Contains(OrderServiceContentSelect.Quantity) ? q.Quantity : default(long),
				Prive = filter.Selects.Contains(OrderServiceContentSelect.Prive) ? q.Prive : default(long),
				
			}).ToListAsync();
			return OrderServiceContents;
		}
		public async Task<int> Count(OrderServiceContentFilter filter)
		{
			IQueryable<OrderServiceContentDAO> OrderServiceContents = DataContext.OrderServiceContent;
			OrderServiceContents = DynamicFilter(OrderServiceContents, filter);
			return await OrderServiceContents.CountAsync();
		}

		public async Task<List<OrderServiceContent>> List(OrderServiceContentFilter filter)
		{
			if (filter == null) return new List<OrderServiceContent>();
			IQueryable<OrderServiceContentDAO> OrderServiceContentDAOs = DataContext.OrderServiceContent.AsNoTracking();
			OrderServiceContentDAOs = DynamicFilter(OrderServiceContentDAOs, filter);
			OrderServiceContentDAOs = DynamicOrder(OrderServiceContentDAOs, filter);
			List<OrderServiceContent> OrderServiceContents = await DynamicSelect(OrderServiceContentDAOs, filter);
			return OrderServiceContents;
		}

		public async Task<List<OrderServiceContent>> List(List<long> Ids)
		{
			List<OrderServiceContent> OrderServiceContents = await DataContext.OrderServiceContent.AsNoTracking()
		.Where(x => Ids.Contains(x.Id)).Select(x => new OrderServiceContent()
		{
			Id = x.Id,
			ServiceId = x.ServiceId,
			OrderServiceId = x.OrderServiceId,
			UnitOfMeasureId = x.UnitOfMeasureId,
			PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
			Quantity = x.Quantity,
			Prive = x.Prive,
			RequestQuantity = x.RequestQuantity,
			Amount = x.Amount
		}).ToListAsync();

			return OrderServiceContents;
		}
		public async Task<OrderServiceContent> Get(long Id)
		{
			OrderServiceContent OrderServiceContent = await DataContext.OrderServiceContent.AsNoTracking()
		    .Where(x => x.Id == Id).Select(x => new OrderServiceContent()
		    {
			    Id = x.Id,
			    ServiceId = x.ServiceId,
			    OrderServiceId = x.OrderServiceId,
			    UnitOfMeasureId = x.UnitOfMeasureId,
			    PrimaryUnitOfMeasureId = x.PrimaryUnitOfMeasureId,
			    Quantity = x.Quantity,
			    Prive = x.Prive,
			    RequestQuantity = x.RequestQuantity,
			    Amount = x.Amount
		    }).FirstOrDefaultAsync();

			if (OrderServiceContent == null)
				return null;

			return OrderServiceContent;
		}

		private async Task SaveReference(OrderServiceContent orderServiceContent)
		{ }
	}
}
