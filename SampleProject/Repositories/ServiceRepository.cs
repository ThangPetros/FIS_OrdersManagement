using Microsoft.EntityFrameworkCore;
using SampleProject.Entities;
using SampleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace SampleProject.Repositories
{
	public interface IServiceRepository
	{
		Task<int> Count(ServiceFilter ServiceFilter);
		Task<List<Service>> List(ServiceFilter ServiceFilter);
		Task<List<Service>> List(List<long> Ids);
		Task<Service> Get(long Id);
		Task<bool> Create(Service Service);
		Task<bool> Update(Service Service);
		Task<bool> Delete(Service Service);
		Task<bool> BulkMerge(List<Service> Services);
		Task<bool> BulkDelete(List<Service> Services);
		Task<bool> Used(List<long> Ids);
	}
	public class ServiceRepository : IServiceRepository
	{
		private DataContext DataContext;
		public ServiceRepository(DataContext dataContext)
		{
			this.DataContext = dataContext;
		}
		private IQueryable<ServiceDAO> DynamicFilter(IQueryable<ServiceDAO> query, ServiceFilter filter)
		{
			if (filter == null)
				return query.Where(q => false);
			query = query.Where(q => !q.DeletedAt.HasValue);
			query = query.Where(q => q.Id, filter.Id);
			query = query.Where(q => q.Code, filter.Code);
			query = query.Where(q => q.Name, filter.Name);
			query = query.Where(q => q.UnitOfMeasureId, filter.UnitOfMeasureId);
			query = query.Where(q => q.Price, filter.Price);
			query = query.Where(q => q.StatusId, filter.StatusId);
			query = query.Where(q => q.UpdatedAt, filter.UpdateTime);

			query = OrFilter(query, filter);
			return query;
		}
		private IQueryable<ServiceDAO> OrFilter(IQueryable<ServiceDAO> query, ServiceFilter filter)
		{
			if (filter.OrFilter == null || filter.OrFilter.Count == 0)
				return query;
			IQueryable<ServiceDAO> initQuery = query.Where(q => false);

			foreach (ServiceFilter ServiceFilter in filter.OrFilter)
			{
				IQueryable<ServiceDAO> queryable = query;
				queryable = queryable.Where(q => q.Id, ServiceFilter.Id);
				queryable = queryable.Where(q => q.Code, ServiceFilter.Code);
				queryable = queryable.Where(q => q.Name, ServiceFilter.Name);
				queryable = queryable.Where(q => q.UnitOfMeasureId, ServiceFilter.UnitOfMeasureId);
				queryable = queryable.Where(q => q.Price, ServiceFilter.Price);
				queryable = queryable.Where(q => q.StatusId, ServiceFilter.StatusId);
				initQuery = initQuery.Union(queryable);
			}
			return initQuery;
		}
		private IQueryable<ServiceDAO> DynamicOrder(IQueryable<ServiceDAO> query, ServiceFilter filter)
		{
			switch (filter.OrderType)
			{
				case OrderType.ASC:
					switch (filter.OrderBy)
					{
						case ServiceOrder.Id:
							query = query.OrderBy(q => q.Id);
							break;
						case ServiceOrder.Code:
							query = query.OrderBy(q => q.Code);
							break;
						case ServiceOrder.Name:
							query = query.OrderBy(q => q.Name);
							break;
						case ServiceOrder.Price:
							query = query.OrderBy(q => q.Price);
							break;
						case ServiceOrder.Status:
							query = query.OrderBy(q => q.StatusId);
							break;
						case ServiceOrder.UpdateAt:
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
						case ServiceOrder.Id:
							query = query.OrderByDescending(q => q.Id);
							break;
						case ServiceOrder.Code:
							query = query.OrderByDescending(q => q.Code);
							break;
						case ServiceOrder.Name:
							query = query.OrderByDescending(q => q.Name);
							break;
						case ServiceOrder.Price:
							query = query.OrderByDescending(q => q.Price);
							break;
						case ServiceOrder.Status:
							query = query.OrderByDescending(q => q.StatusId);
							break;
						case ServiceOrder.UpdateAt:
							query = query.OrderByDescending(q => q.UpdatedAt);
							break;
					}
					break;
			}
			query = query.Skip(filter.Skip).Take(filter.Take);
			return query;
		}
		private async Task<List<Service>> DynamicSelect(IQueryable<ServiceDAO> query, ServiceFilter filter)
		{
			List<Service> Services = await query.Select(q => new Service()
			{
				Id = filter.Selects.Contains(ServiceSelect.Id) ? q.Id : default(long),
				Code = filter.Selects.Contains(ServiceSelect.Code) ? q.Code : default(string),
				Name = filter.Selects.Contains(ServiceSelect.Name) ? q.Name : default(string),
				UnitOfMeasureId = filter.Selects.Contains(ServiceSelect.UnitOfMeasure) ? q.UnitOfMeasureId : default(long),
				Price = filter.Selects.Contains(ServiceSelect.Price) ? q.Price : default(long),
				StatusId = filter.Selects.Contains(ServiceSelect.Status) ? q.StatusId : default(long),
				Status = filter.Selects.Contains(ServiceSelect.Status) && q.Status != null ? new Status
				{
					Id = q.Status.Id,
					Code = q.Status.Code,
					Name = q.Status.Name,
				} : null,
				Used = q.Used,
				CreatedAt = q.CreatedAt,
				UpdatedAt = q.UpdatedAt,
				DeletedAt = q.DeletedAt,
			}).ToListAsync();
			return Services;
		}
		public async Task<int> Count(ServiceFilter filter)
		{
			IQueryable<ServiceDAO> Services = DataContext.Service;
			Services = DynamicFilter(Services, filter);
			return await Services.CountAsync();
		}

		public async Task<List<Service>> List(ServiceFilter filter)
		{
			if (filter == null) return new List<Service>();
			IQueryable<ServiceDAO> ServiceDAOs = DataContext.Service.AsNoTracking();
			ServiceDAOs = DynamicFilter(ServiceDAOs, filter);
			ServiceDAOs = DynamicOrder(ServiceDAOs, filter);
			List<Service> Services = await DynamicSelect(ServiceDAOs, filter);
			return Services;
		}

		public async Task<List<Service>> List(List<long> Ids)
		{
			List<Service> Services = await DataContext.Service.AsNoTracking()
		.Where(x => Ids.Contains(x.Id)).Select(x => new Service()
		{
			CreatedAt = x.CreatedAt,
			UpdatedAt = x.UpdatedAt,
			DeletedAt = x.DeletedAt,
			Id = x.Id,
			Code = x.Code,
			Name = x.Name,
			UnitOfMeasureId = x.UnitOfMeasureId,
			Price = x.Price,
			StatusId = x.StatusId,
			Used = x.Used,
			Status = x.Status == null ? null : new Status
			{
				Id = x.Status.Id,
				Code = x.Status.Code,
				Name = x.Status.Name,
			},
		}).ToListAsync();

			return Services;
		}
		public async Task<Service> Get(long Id)
		{
			Service Service = await DataContext.Service.AsNoTracking()
		    .Where(x => x.Id == Id).Select(x => new Service()
		    {
			    Id = x.Id,
			    Code = x.Code,
			    Name = x.Name,
			    UnitOfMeasureId = x.UnitOfMeasureId,
			    Price = x.Price,
			    StatusId = x.StatusId,
			    Used = x.Used,
			    CreatedAt = x.CreatedAt,
			    UpdatedAt = x.UpdatedAt,
			    DeletedAt = x.DeletedAt,
			    Status = x.Status == null ? null : new Status
			    {
				    Id = x.Status.Id,
				    Code = x.Status.Code,
				    Name = x.Status.Name,
			    },
		    }).FirstOrDefaultAsync();

			if (Service == null)
				return null;

			return Service;
		}
		public async Task<bool> Create(Service Service)
		{
			ServiceDAO ServiceDAO = new ServiceDAO();

			ServiceDAO.Code = Service.Code;
			ServiceDAO.Name = Service.Name;
			ServiceDAO.UnitOfMeasureId = Service.UnitOfMeasureId;
			ServiceDAO.Price = Service.Price;
			ServiceDAO.StatusId = Service.StatusId;
			ServiceDAO.CreatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			ServiceDAO.UpdatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			ServiceDAO.Used = false;

			DataContext.Service.Add(ServiceDAO);
			await DataContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Update(Service Service)
		{
			ServiceDAO ServiceDAO = DataContext.Service.Where(x => x.Id == Service.Id).FirstOrDefault();
			if (ServiceDAO == null)
				return false;

			ServiceDAO.Code = Service.Code;
			ServiceDAO.Name = Service.Name;
			ServiceDAO.UnitOfMeasureId = Service.UnitOfMeasureId;
			ServiceDAO.Price = Service.Price;
			ServiceDAO.StatusId = Service.StatusId;
			ServiceDAO.UpdatedAt = Service.UpdatedAt;
			ServiceDAO.Used = Service.Used;

			await DataContext.SaveChangesAsync();
			return true;
		}
		public async Task<bool> Delete(Service Service)
		{
			Service.DeletedAt = DateTime.Now;
			await Update(Service);
			//await DataContext.Service.Where(x => x.Id == Service.Id).UpdateFromQueryAsync(x => new ServiceDAO { DeletedAt = DateTime.Now });
			return true;
		}
		public async Task<bool> BulkMerge(List<Service> Services)
		{
			List<ServiceDAO> ServiceDAOs = new List<ServiceDAO>();
			foreach (Service Service in Services)
			{
				ServiceDAO ServiceDAO = new ServiceDAO();
				ServiceDAO.Id = Service.Id;
				ServiceDAO.Code = Service.Code;
				ServiceDAO.Name = Service.Name;
				ServiceDAO.UnitOfMeasureId = Service.UnitOfMeasureId;
				ServiceDAO.Price = Service.Price;
				ServiceDAO.StatusId = Service.StatusId;
				ServiceDAO.CreatedAt = DateTime.Now;
				ServiceDAO.UpdatedAt = DateTime.Now;
				ServiceDAOs.Add(ServiceDAO);
			}
			await DataContext.BulkMergeAsync(ServiceDAOs);
			return true;
		}
		public async Task<bool> BulkDelete(List<Service> Services)
		{
			List<long> Ids = Services.Select(x => x.Id).ToList();
			await DataContext.Service
			    .Where(x => Ids.Contains(x.Id))
			    .UpdateFromQueryAsync(x => new ServiceDAO { DeletedAt = DateTime.Now });
			return true;
		}
		public async Task<bool> Used(List<long> Ids)
		{
			await DataContext.Customer.Where(x => Ids.Contains(x.Id))
		    .UpdateFromQueryAsync(x => new ServiceDAO { Used = true });
			return true;
		}
	}
}
