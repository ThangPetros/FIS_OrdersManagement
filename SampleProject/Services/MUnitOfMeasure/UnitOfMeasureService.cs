using TrueSight.Common;
using SampleProject.Common;
using SampleProject.Entities;
using SampleProject.Repositories;
using SampleProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleProject.Handlers.Configuration;
using SampleProject.Enums;
namespace SampleProject.Services.MUnitOfMeasure
{
      public interface IUnitOfMeasureService : IServiceScoped
      {
            Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter);
            Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter);
            Task<UnitOfMeasure> Get(long Id);
            Task<UnitOfMeasure> Create(UnitOfMeasure UnitOfMeasure);
            Task<UnitOfMeasure> Update(UnitOfMeasure UnitOfMeasure);
            Task<UnitOfMeasure> Delete(UnitOfMeasure UnitOfMeasure);
            Task<List<UnitOfMeasure>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures);
            Task<List<UnitOfMeasure>> BulkDelete(List<UnitOfMeasure> UnitOfMeasures);
      }

      public class UnitOfMeasureService : IUnitOfMeasureService
      {
            private IUOW UOW;
            private ILogging Logging;
            private ICurrentContext CurrentContext;
            private IUnitOfMeasureValidator UnitOfMeasureValidator;
            private IRabbitManager RabbitManager;

            public UnitOfMeasureService(
                IUOW UOW,
                ILogging Logging,
                ICurrentContext CurrentContext,
                IUnitOfMeasureValidator UnitOfMeasureValidator,
                IRabbitManager RabbitManager
            )
            {
                  this.UOW = UOW;
                  this.Logging = Logging;
                  this.CurrentContext = CurrentContext;
                  this.UnitOfMeasureValidator = UnitOfMeasureValidator;
                  this.RabbitManager = RabbitManager;
            }
            public async Task<int> Count(UnitOfMeasureFilter UnitOfMeasureFilter)
            {
                  try
                  {
                        int result = await UOW.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);
                        return result;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return 0;
            }

            public async Task<List<UnitOfMeasure>> List(UnitOfMeasureFilter UnitOfMeasureFilter)
            {
                  try
                  {
                        List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(UnitOfMeasureFilter);
                        return UnitOfMeasures;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }
            public async Task<UnitOfMeasure> Get(long Id)
            {
                  UnitOfMeasure UnitOfMeasure = await UOW.UnitOfMeasureRepository.Get(Id);
                  if (UnitOfMeasure == null)
                        return null;
                  return UnitOfMeasure;
            }
            public async Task<UnitOfMeasure> Create(UnitOfMeasure UnitOfMeasure)
            {
                  if (!await UnitOfMeasureValidator.Create(UnitOfMeasure))
                        return UnitOfMeasure;

                  try
                  {
                        await UOW.UnitOfMeasureRepository.Create(UnitOfMeasure);
                        List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(new List<long> { UnitOfMeasure.Id });
                        Sync(UnitOfMeasures);
                        UnitOfMeasure = UnitOfMeasures.FirstOrDefault();
                        Logging.CreateAuditLog(UnitOfMeasure, new { }, nameof(UnitOfMeasureService));
                        return await UOW.UnitOfMeasureRepository.Get(UnitOfMeasure.Id);
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }

            public async Task<UnitOfMeasure> Update(UnitOfMeasure UnitOfMeasure)
            {
                  if (!await UnitOfMeasureValidator.Update(UnitOfMeasure))
                        return UnitOfMeasure;
                  try
                  {
                        var oldData = await UOW.UnitOfMeasureRepository.Get(UnitOfMeasure.Id);
                        await UOW.UnitOfMeasureRepository.Update(UnitOfMeasure);
                        List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(new List<long> { UnitOfMeasure.Id });
                        Sync(UnitOfMeasures);
                        UnitOfMeasure = UnitOfMeasures.FirstOrDefault();
                        Logging.CreateAuditLog(UnitOfMeasure, oldData, nameof(UnitOfMeasureService));
                        return UnitOfMeasure;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }

            public async Task<UnitOfMeasure> Delete(UnitOfMeasure UnitOfMeasure)
            {
                  if (!await UnitOfMeasureValidator.Delete(UnitOfMeasure))
                        return UnitOfMeasure;

                  try
                  {

                        await UOW.UnitOfMeasureRepository.Delete(UnitOfMeasure);

                        List<UnitOfMeasure> UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(new List<long> { UnitOfMeasure.Id });
                        Sync(UnitOfMeasures);
                        UnitOfMeasure = UnitOfMeasures.FirstOrDefault();
                        Logging.CreateAuditLog(new { }, UnitOfMeasure, nameof(UnitOfMeasureService));
                        return UnitOfMeasure;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }

            public async Task<List<UnitOfMeasure>> BulkDelete(List<UnitOfMeasure> UnitOfMeasures)
            {
                  if (!await UnitOfMeasureValidator.BulkDelete(UnitOfMeasures))
                        return UnitOfMeasures;

                  try
                  {

                        await UOW.UnitOfMeasureRepository.BulkDelete(UnitOfMeasures);

                        List<long> Ids = UnitOfMeasures.Select(x => x.Id).ToList();
                        UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(Ids);
                        Sync(UnitOfMeasures);
                        Logging.CreateAuditLog(new { }, UnitOfMeasures, nameof(UnitOfMeasureService));
                        return UnitOfMeasures;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }

            public async Task<List<UnitOfMeasure>> BulkMerge(List<UnitOfMeasure> UnitOfMeasures)
            {
                  if (!await UnitOfMeasureValidator.BulkMerge(UnitOfMeasures))
                        return UnitOfMeasures;
                  try
                  {

                        List<UnitOfMeasure> dbUnitOfMeasures = await UOW.UnitOfMeasureRepository.List(new UnitOfMeasureFilter
                        {
                              Skip = 0,
                              Take = int.MaxValue,
                              Selects = UnitOfMeasureSelect.Id | UnitOfMeasureSelect.Code,
                        });
                        foreach (UnitOfMeasure UnitOfMeasure in UnitOfMeasures)
                        {
                              long UnitOfMeasureId = dbUnitOfMeasures.Where(x => x.Code == UnitOfMeasure.Code)
                                  .Select(x => x.Id).FirstOrDefault();
                              UnitOfMeasure.Id = UnitOfMeasureId;
                        }
                        await UOW.UnitOfMeasureRepository.BulkMerge(UnitOfMeasures);

                        List<long> Ids = UnitOfMeasures.Select(x => x.Id).ToList();
                        UnitOfMeasures = await UOW.UnitOfMeasureRepository.List(Ids);
                        Sync(UnitOfMeasures);
                        Logging.CreateAuditLog(UnitOfMeasures, new { }, nameof(UnitOfMeasureService));
                        return UnitOfMeasures;
                  }
                  catch (Exception ex)
                  {
                        Logging.CreateSystemLog(ex, nameof(UnitOfMeasureService));
                  }
                  return null;
            }

            private void Sync(List<UnitOfMeasure> UnitOfMeasures)
            {
                  RabbitManager.PublishList(UnitOfMeasures, RoutingKeyEnum.UnitOfMeasureSync.Code);
            }
      }
}
