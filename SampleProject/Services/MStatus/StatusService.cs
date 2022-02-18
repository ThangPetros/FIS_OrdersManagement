using TrueSight.Common;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Repositories;
using SampleProject.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleProject.Services.MStatus
{
	public interface IStatusService : IServiceScoped
	{
		Task<int> Count(StatusFilter StatusFilter);
		Task<List<Status>> List(StatusFilter StatusFilter);
	}

	public class StatusService : IStatusService //BaseService, 
	{
		private IUOW UOW;
		//private ILogging Logging;
		private ICurrentContext CurrentContext;

		public StatusService(
		    IUOW UOW,
		    //ILogging Logging,
		    ICurrentContext CurrentContext
		)
		{
			this.UOW = UOW;
			//this.Logging = Logging;
			//this.CurrentContext = CurrentContext;
		}
		public async Task<int> Count(StatusFilter StatusFilter)
		{
			try
			{
				int result = await UOW.StatusRepository.Count(StatusFilter);
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(StatusService));
			}
			return 0;
		}

		public async Task<List<Status>> List(StatusFilter StatusFilter)
		{
			try
			{
				List<Status> Statuss = await UOW.StatusRepository.List(StatusFilter);
				return Statuss;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Logging.CreateSystemLog(ex, nameof(StatusService));
			}
			return null;
		}
	}
}
