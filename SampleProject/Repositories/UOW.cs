using TrueSight.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using SampleProject.Models;

namespace SampleProject.Repositories
{
	public interface IUOW : IServiceScoped, IDisposable
	{
		ICustomerRepository CustomerRepository { get; }
		IOrderServiceRepository OrderServiceRepository { get; }
		IOrderServiceContentRepository OrderServiceContentRepository { get; }
		IServiceRepository ServiceRepository { get; }
		IStatusRepository StatusRepository { get; }
		IUnitOfMeasureRepository UnitOfMeasureRepository { get; }
	}

	public class UOW : IUOW
	{
		private DataContext DataContext;
		public ICustomerRepository CustomerRepository { get; private set; }
		public IOrderServiceContentRepository OrderServiceContentRepository { get; private set;}
		public IOrderServiceRepository OrderServiceRepository { get; private set; }
		public IServiceRepository ServiceRepository { get; private set; }
		public IStatusRepository StatusRepository { get; private set; }
		public IUnitOfMeasureRepository UnitOfMeasureRepository { get; private set; }

		public UOW(DataContext DataContext)
		{
			this.DataContext = DataContext;
			CustomerRepository = new CustomerRepository(DataContext);
			OrderServiceContentRepository = new OrderServiceContentRepository(DataContext);
			OrderServiceRepository = new OrderServiceRepository(DataContext);
			ServiceRepository = new ServiceRepository(DataContext);
			StatusRepository = new StatusRepository(DataContext);
			UnitOfMeasureRepository = new UnitOfMeasureRepository(DataContext);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (this.DataContext == null)
			{
				return;
			}

			this.DataContext.Dispose();
			this.DataContext = null;
		}
	}
}