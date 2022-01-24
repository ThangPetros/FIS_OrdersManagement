using TrueSight.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using SampleProject.Models;

namespace SampleProject.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        IUnitOfMeasureRepository UnitOfMeasureRepository { get; }
    }

    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IUnitOfMeasureRepository UnitOfMeasureRepository { get; private set; }

        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;
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