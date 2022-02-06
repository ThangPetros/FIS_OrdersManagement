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
	public interface ICustomerRepository
	{
		Task<int> Count(CustomerFilter CustomerFilter);
		Task<List<Customer>> List(CustomerFilter CustomerFilter);
		Task<List<Customer>> List(List<long> Ids);
		Task<Customer> Get(long Id);
		Task<bool> Create(Customer Customer);
		Task<bool> Update(Customer Customer);
		Task<bool> Delete(Customer Customer);
		Task<bool> BulkMerge(List<Customer> Customers);
		Task<bool> BulkDelete(List<Customer> Customers);
		Task<bool> Used(List<long> Ids);
	}

	public class CustomerRepository : ICustomerRepository
	{
		private DataContext DataContext;
		public CustomerRepository(DataContext dataContext)
		{
			this.DataContext = dataContext;
		}
		private IQueryable<CustomerDAO> DynamicFilter(IQueryable<CustomerDAO> query, CustomerFilter filter)
		{
			if (filter == null)
				return query.Where(q => false);
			query = query.Where(q => !q.DeleteAt.HasValue);
			query = query.Where(q => q.Id, filter.Id);
			query = query.Where(q => q.Code, filter.Code);
			query = query.Where(q => q.Name, filter.Name);
			query = query.Where(q => q.Phone, filter.Phone);
			query = query.Where(q => q.Address, filter.Address);
			query = query.Where(q => q.StatusId, filter.StatusId);
			query = query.Where(q => q.UpdatedAt, filter.UpdateTime);

			query = OrFilter(query, filter);
			return query;
		}
		private IQueryable<CustomerDAO> OrFilter(IQueryable<CustomerDAO> query, CustomerFilter filter)
		{
			if (filter.OrFilter == null || filter.OrFilter.Count == 0)
				return query;
			IQueryable<CustomerDAO> initQuery = query.Where(q => false);
			foreach (CustomerFilter CustomerFilter in filter.OrFilter)
			{
				IQueryable<CustomerDAO> queryable = query;
				queryable = queryable.Where(q => q.Id, CustomerFilter.Id);
				queryable = queryable.Where(q => q.Code, CustomerFilter.Code);
				queryable = queryable.Where(q => q.Name, CustomerFilter.Name);
				queryable = queryable.Where(q => q.Phone, CustomerFilter.Phone);
				queryable = queryable.Where(q => q.Address, CustomerFilter.Address);
				queryable = queryable.Where(q => q.StatusId, CustomerFilter.StatusId);
				initQuery = initQuery.Union(queryable);
			}
			return initQuery;
		}
		private IQueryable<CustomerDAO> DynamicOrder(IQueryable<CustomerDAO> query, CustomerFilter filter)
		{
			switch (filter.OrderType)
			{
				case OrderType.ASC:
					switch (filter.OrderBy)
					{
						case CustomerOrder.Id:
							query = query.OrderBy(q => q.Id);
							break;
						case CustomerOrder.Code:
							query = query.OrderBy(q => q.Code);
							break;
						case CustomerOrder.Name:
							query = query.OrderBy(q => q.Name);
							break;
						case CustomerOrder.Address:
							query = query.OrderBy(q => q.Address);
							break;
						case CustomerOrder.Status:
							query = query.OrderBy(q => q.StatusId);
							break;
						case CustomerOrder.UpdateAt:
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
						case CustomerOrder.Id:
							query = query.OrderByDescending(q => q.Id);
							break;
						case CustomerOrder.Code:
							query = query.OrderByDescending(q => q.Code);
							break;
						case CustomerOrder.Name:
							query = query.OrderByDescending(q => q.Name);
							break;
						case CustomerOrder.Address:
							query = query.OrderByDescending(q => q.Address);
							break;
						case CustomerOrder.Status:
							query = query.OrderByDescending(q => q.StatusId);
							break;
						case CustomerOrder.UpdateAt:
							query = query.OrderByDescending(q => q.UpdatedAt);
							break;
					}
					break;
			}
			query = query.Skip(filter.Skip).Take(filter.Take);
			return query;
		}
		private async Task<List<Customer>> DynamicSelect(IQueryable<CustomerDAO> query, CustomerFilter filter)
		{
			List<Customer> Customers = await query.Select(q => new Customer()
			{
				Id = filter.Selects.Contains(CustomerSelect.Id) ? q.Id : default(long),
				Code = filter.Selects.Contains(CustomerSelect.Code) ? q.Code : default(string),
				Name = filter.Selects.Contains(CustomerSelect.Name) ? q.Name : default(string),
				Address = filter.Selects.Contains(CustomerSelect.Address) ? q.Address : default(string),
				StatusId = filter.Selects.Contains(CustomerSelect.Status) ? q.StatusId : default(long),
				Status = filter.Selects.Contains(CustomerSelect.Status) && q.Status != null ? new Status
				{
					Id = q.Status.Id,
					Code = q.Status.Code,
					Name = q.Status.Name,
				} : null,
				Used = q.Used,
				CreatedAt = q.CreatedAt,
				UpdatedAt = q.UpdatedAt,
				DeleteAt = q.DeleteAt,
			}).ToListAsync();
			return Customers;
		}
		public async Task<int> Count(CustomerFilter filter)
		{
			IQueryable<CustomerDAO> Customers = DataContext.Customer;
			Customers = DynamicFilter(Customers, filter);
			return await Customers.CountAsync();
		}

		public async Task<List<Customer>> List(CustomerFilter filter)
		{
			if (filter == null) return new List<Customer>();
			IQueryable<CustomerDAO> CustomerDAOs = DataContext.Customer.AsNoTracking();
			CustomerDAOs = DynamicFilter(CustomerDAOs, filter);
			CustomerDAOs = DynamicOrder(CustomerDAOs, filter);
			List<Customer> Customers = await DynamicSelect(CustomerDAOs, filter);
			return Customers;
		}

		public async Task<List<Customer>> List(List<long> Ids)
		{
			List<Customer> Customers = await DataContext.Customer.AsNoTracking()
		.Where(x => Ids.Contains(x.Id)).Select(x => new Customer()
		{
			CreatedAt = x.CreatedAt,
			UpdatedAt = x.UpdatedAt,
			DeleteAt = x.DeleteAt,
			Id = x.Id,
			Code = x.Code,
			Name = x.Name,
			StatusId = x.StatusId,
			Address = x.Address,
			Used = x.Used,
			Status = x.Status == null ? null : new Status
			{
				Id = x.Status.Id,
				Code = x.Status.Code,
				Name = x.Status.Name,
			},
		}).ToListAsync();

			return Customers;
		}
		public async Task<Customer> Get(long Id)
		{
			Customer Customer = await DataContext.Customer.AsNoTracking()
		    .Where(x => x.Id == Id).Select(x => new Customer()
		    {
			    Id = x.Id,
			    Code = x.Code,
			    Name = x.Name,
			    Address = x.Address,
			    StatusId = x.StatusId,
			    Used = x.Used,
			    CreatedAt = x.CreatedAt,
			    UpdatedAt = x.UpdatedAt,
			    DeleteAt = x.DeleteAt,
			    Status = x.Status == null ? null : new Status
			    {
				    Id = x.Status.Id,
				    Code = x.Status.Code,
				    Name = x.Status.Name,
			    },
		    }).FirstOrDefaultAsync();

			if (Customer == null)
				return null;

			return Customer;
		}
		public async Task<bool> Create(Customer Customer)
		{
			CustomerDAO CustomerDAO = new CustomerDAO();
			CustomerDAO.Id = Customer.Id;
			CustomerDAO.Code = Customer.Code;
			CustomerDAO.Name = Customer.Name;
			CustomerDAO.Address = Customer.Address;
			CustomerDAO.StatusId = Customer.StatusId;
			CustomerDAO.CreatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			CustomerDAO.UpdatedAt = DateTime.Now;//StaticParams.DateTimeNow;
			CustomerDAO.Used = false;
			DataContext.Customer.Add(CustomerDAO);
			await DataContext.SaveChangesAsync();
			Customer.Id = CustomerDAO.Id;
			await SaveReference(Customer);
			return true;
		}

		public async Task<bool> Update(Customer Customer)
		{
			CustomerDAO CustomerDAO = DataContext.Customer.Where(x => x.Id == Customer.Id).FirstOrDefault();
			if (CustomerDAO == null)
				return false;
			CustomerDAO.Id = Customer.Id;
			CustomerDAO.Code = Customer.Code;
			CustomerDAO.Name = Customer.Name;
			CustomerDAO.Name = Customer.Address;
			CustomerDAO.StatusId = Customer.StatusId;
			CustomerDAO.UpdatedAt = DateTime.Now;
			await DataContext.SaveChangesAsync();
			await SaveReference(Customer);
			return true;
		}
		public async Task<bool> Delete(Customer Customer)
		{
			await DataContext.Customer.Where(x => x.Id == Customer.Id).UpdateFromQueryAsync(x => new CustomerDAO { DeleteAt = DateTime.Now });
			return true;
		}
		public async Task<bool> BulkDelete(List<Customer> Customers)
		{
			List<CustomerDAO> CustomerDAOs = new List<CustomerDAO>();
			foreach (Customer Customer in Customers)
			{
				CustomerDAO CustomerDAO = new CustomerDAO();
				CustomerDAO.Id = Customer.Id;
				CustomerDAO.Code = Customer.Code;
				CustomerDAO.Name = Customer.Name;
				CustomerDAO.StatusId = Customer.StatusId;
				CustomerDAO.CreatedAt = DateTime.Now;
				CustomerDAO.UpdatedAt = DateTime.Now;
				CustomerDAOs.Add(CustomerDAO);
			}
			await DataContext.BulkMergeAsync(CustomerDAOs);
			return true;
		}
		public async Task<bool> BulkMerge(List<Customer> Customers)
		{
			List<long> Ids = Customers.Select(x => x.Id).ToList();
			await DataContext.Customer
			    .Where(x => Ids.Contains(x.Id))
			    .UpdateFromQueryAsync(x => new CustomerDAO { DeleteAt = DateTime.Now });
			return true;
		}
		public async Task<bool> Used(List<long> Ids)
		{
			await DataContext.Customer.Where(x => Ids.Contains(x.Id))
		    .UpdateFromQueryAsync(x => new CustomerDAO { Used = true });
			return true;
		}
		private async Task SaveReference(Customer customer)
		{ }
	}
}
