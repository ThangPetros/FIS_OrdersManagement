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
	public interface IOrderServiceRepository
	{
		Task<int> Count(OrderServiceFilter OrderServiceFilter);
		Task<List<OrderService>> List(OrderServiceFilter OrderServiceFilter);
		Task<List<OrderService>> List(List<long> Ids);
		Task<OrderService> Get(long Id);
		Task<bool> Create(OrderService OrderService);
		Task<bool> Update(OrderService OrderService);
		Task<bool> Delete(OrderService OrderService);
		Task<bool> BulkMerge(List<OrderService> OrderServices);
		Task<bool> BulkDelete(List<OrderService> OrderServices);
		Task<bool> Used(List<long> Ids);
	}
	public class OrderServiceRepository: IOrderServiceRepository
	{
		private DataContext DataContext;
		public OrderServiceRepository(DataContext dataContext)
		{
			this.DataContext = dataContext;
		}
		private IQueryable<OrderServiceDAO> DynamicFilter(IQueryable<OrderServiceDAO> query, OrderServiceFilter filter)
		{
			if (filter == null)
				return query.Where(q => false);
			query = query.Where(q => !q.DeletedAt.HasValue);
			query = query.Where(q => q.Id, filter.Id);
			query = query.Where(q => q.Code, filter.Code);
			query = query.Where(q => q.OrderDate, filter.OrderDate);
			query = query.Where(q => q.Total, filter.Total);
			query = query.Where(q => q.UpdatedAt, filter.UpdateTime);

			query = OrFilter(query, filter);
			return query;
		}
		private IQueryable<OrderServiceDAO> OrFilter(IQueryable<OrderServiceDAO> query, OrderServiceFilter filter)
		{
			if (filter.OrFilter == null || filter.OrFilter.Count == 0)
				return query;
			IQueryable<OrderServiceDAO> initQuery = query.Where(q => false);
			foreach (OrderServiceFilter OrderServiceFilter in filter.OrFilter)
			{
				IQueryable<OrderServiceDAO> queryable = query;
				queryable = queryable.Where(q => q.Id, OrderServiceFilter.Id);
				queryable = queryable.Where(q => q.Code, OrderServiceFilter.Code);
				queryable = queryable.Where(q => q.OrderDate, OrderServiceFilter.OrderDate);
				queryable = queryable.Where(q => q.Total, OrderServiceFilter.Total);
				initQuery = initQuery.Union(queryable);
			}
			return initQuery;
		}
		private IQueryable<OrderServiceDAO> DynamicOrder(IQueryable<OrderServiceDAO> query, OrderServiceFilter filter)
		{
			switch (filter.OrderType)
			{
				case OrderType.ASC:
					switch (filter.OrderBy)
					{
						case OrderServiceOrder.Id:
							query = query.OrderBy(q => q.Id);
							break;
						case OrderServiceOrder.Code:
							query = query.OrderBy(q => q.Code);
							break;
						case OrderServiceOrder.OrderDate:
							query = query.OrderBy(q => q.OrderDate);
							break;
						case OrderServiceOrder.Total:
							query = query.OrderBy(q => q.Total);
							break;
						case OrderServiceOrder.UpdateAt:
							query = query.OrderBy(q => q.UpdatedAt);
							break;
						default:
							query = query.OrderBy(q => q.CreatedAt);
							break;
					}
					break;
				case OrderType.DESC:
					switch (filter.OrderBy)
					{
						case OrderServiceOrder.Id:
							query = query.OrderByDescending(q => q.Id);
							break;
						case OrderServiceOrder.Code:
							query = query.OrderByDescending(q => q.Code);
							break;
						case OrderServiceOrder.OrderDate:
							query = query.OrderByDescending(q => q.OrderDate);
							break;
						case OrderServiceOrder.Total:
							query = query.OrderByDescending(q => q.Total);
							break;
						case OrderServiceOrder.UpdateAt:
							query = query.OrderByDescending(q => q.UpdatedAt);
							break;
					}
					break;
			}
			query = query.Skip(filter.Skip).Take(filter.Take);
			return query;
		}
		private async Task<List<OrderService>> DynamicSelect(IQueryable<OrderServiceDAO> query, OrderServiceFilter filter)
		{
			List<OrderService> OrderServices = await query.Select(q => new OrderService()
			{
				Id = filter.Selects.Contains(OrderServiceSelect.Id) ? q.Id : default(long),
				Code = filter.Selects.Contains(OrderServiceSelect.Code) ? q.Code : default(string),
				OrderDate = filter.Selects.Contains(OrderServiceSelect.OrderDate) ? q.OrderDate : default(DateTime),
				Total = filter.Selects.Contains(OrderServiceSelect.Total) ? q.Total : default(long),
				Used = q.Used,
				CreatedAt = q.CreatedAt,
				UpdatedAt = q.UpdatedAt,
				DeletedAt = q.DeletedAt,
			}).ToListAsync();
			return OrderServices;
		}
		public async Task<int> Count(OrderServiceFilter filter)
		{
			IQueryable<OrderServiceDAO> OrderServices = DataContext.OrderService;
			OrderServices = DynamicFilter(OrderServices, filter);
			return await OrderServices.CountAsync();
		}

		public async Task<List<OrderService>> List(OrderServiceFilter filter)
		{
			if (filter == null) return new List<OrderService>();
			IQueryable<OrderServiceDAO> OrderServiceDAOs = DataContext.OrderService.AsNoTracking();
			OrderServiceDAOs = DynamicFilter(OrderServiceDAOs, filter);
			OrderServiceDAOs = DynamicOrder(OrderServiceDAOs, filter);
			List<OrderService> OrderServices = await DynamicSelect(OrderServiceDAOs, filter);
			return OrderServices;
		}

		public async Task<List<OrderService>> List(List<long> Ids)
		{
			List<OrderService> OrderServices = await DataContext.OrderService.AsNoTracking()
		.Where(x => Ids.Contains(x.Id)).Select(x => new OrderService()
		{
			CreatedAt = x.CreatedAt,
			UpdatedAt = x.UpdatedAt,
			DeletedAt = x.DeletedAt,
			Id = x.Id,
			Code = x.Code,
			OrderDate = x.OrderDate,
			Total = x.Total,
			Used = x.Used,
		}).ToListAsync();

			return OrderServices;
		}
		public async Task<OrderService> Get(long Id)
		{
			OrderService OrderService = await DataContext.OrderService.AsNoTracking()
		    .Where(x => x.Id == Id).Select(x => new OrderService()
		    {
			    Id = x.Id,
			    Code = x.Code,
			    OrderDate = x.OrderDate,
			    Total = x.Total,
			    Used = x.Used,
			    CreatedAt = x.CreatedAt,
			    UpdatedAt = x.UpdatedAt,
			    DeletedAt = x.DeletedAt,
		    }).FirstOrDefaultAsync();

			if (OrderService == null)
				return null;

			return OrderService;
		}
		public async Task<bool> Create(OrderService OrderService)
		{
			OrderServiceDAO OrderServiceDAO = new OrderServiceDAO();

			OrderServiceDAO.Code = OrderService.Code;
			OrderServiceDAO.OrderDate = OrderService.OrderDate;
			OrderServiceDAO.Total = OrderService.Total;
			OrderServiceDAO.CreatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			OrderServiceDAO.UpdatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			OrderServiceDAO.Used = false;

			DataContext.OrderService.Add(OrderServiceDAO);
			await DataContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Update(OrderService OrderService)
		{
			OrderServiceDAO OrderServiceDAO = DataContext.OrderService.Where(x => x.Id == OrderService.Id).FirstOrDefault();
			if (OrderServiceDAO == null)
				return false;

			OrderServiceDAO.Code = OrderService.Code;
			OrderServiceDAO.OrderDate = OrderService.OrderDate;
			OrderServiceDAO.Total = OrderService.Total;
			OrderServiceDAO.UpdatedAt = OrderService.UpdatedAt;
			OrderServiceDAO.Used = OrderService.Used;

			await DataContext.SaveChangesAsync();
			return true;
		}
		public async Task<bool> Delete(OrderService OrderService)
		{
			OrderService.DeletedAt = DateTime.Now;
			await Update(OrderService);
			//await DataContext.OrderService.Where(x => x.Id == OrderService.Id).UpdateFromQueryAsync(x => new OrderServiceDAO { DeletedAt = DateTime.Now });
			return true;
		}
		public async Task<bool> BulkMerge(List<OrderService> OrderServices)
		{
			List<OrderServiceDAO> OrderServiceDAOs = new List<OrderServiceDAO>();
			foreach (OrderService OrderService in OrderServices)
			{
				OrderServiceDAO OrderServiceDAO = new OrderServiceDAO();
				OrderServiceDAO.Id = OrderService.Id;
				OrderServiceDAO.Code = OrderService.Code;
				OrderServiceDAO.OrderDate = OrderService.OrderDate;
				OrderServiceDAO.CustomerId = OrderService.CustomerId;
				OrderServiceDAO.Total = OrderService.Total;
				OrderServiceDAO.CreatedAt = DateTime.Now;
				OrderServiceDAO.UpdatedAt = DateTime.Now;
				OrderServiceDAOs.Add(OrderServiceDAO);
			}
			await DataContext.BulkMergeAsync(OrderServiceDAOs);
			return true;
		}
		public async Task<bool> BulkDelete(List<OrderService> OrderServices)
		{
			List<long> Ids = OrderServices.Select(x => x.Id).ToList();
			await DataContext.OrderService
			    .Where(x => Ids.Contains(x.Id))
			    .UpdateFromQueryAsync(x => new OrderServiceDAO { DeletedAt = DateTime.Now });
			return true;
		}
		public async Task<bool> Used(List<long> Ids)
		{
			await DataContext.OrderService.Where(x => Ids.Contains(x.Id))
		    .UpdateFromQueryAsync(x => new OrderServiceDAO { Used = true });
			return true;
		}
		private async Task SaveReference(OrderService orderService)
		{ }
	}
}
