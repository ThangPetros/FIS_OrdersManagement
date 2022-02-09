using Newtonsoft.Json;
using SampleProject.Common;
using SampleProject.Enums;
using SampleProject.Entities;
using SampleProject.Handlers.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TrueSight.Common;

namespace SampleProject.Helper
{
      public interface ILogging : IServiceScoped
      {
            void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "");
            void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "");
      }
      public class Logging : ILogging
      {
            private ICurrentContext CurrentContext;
            private IRabbitManager RabbitManager;
            public Logging(
                ICurrentContext CurrentContext,
                IRabbitManager RabbitManager
                )
            {
                  this.CurrentContext = CurrentContext;
                  this.RabbitManager = RabbitManager;
            }
            public void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
            {
                  AuditLog AuditLog = new AuditLog
                  {
                        AppUser = CurrentContext.UserName,
                        ClassName = className,
                        MethodName = methodName,
                        ModuleName = StaticParams.ModuleName,
                        OldData = JsonConvert.SerializeObject(oldData),
                        NewData = JsonConvert.SerializeObject(newData),
                        Time = StaticParams.DateTimeNow,
                  };
                  RabbitManager.PublishSingle(AuditLog, RoutingKeyEnum.AuditLogSend.Code);
            }
            public void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "")
            {
                  SystemLog SystemLog = new SystemLog
                  {
                        AppUser = CurrentContext.UserName,
                        ClassName = className,
                        MethodName = methodName,
                        ModuleName = StaticParams.ModuleName,
                        Exception = ex.ToString(),
                        Time = StaticParams.DateTimeNow,
                  };
                  RabbitManager.PublishSingle(SystemLog, RoutingKeyEnum.SystemLogSend.Code);
                  throw new MessageException(ex);
            }
      }
}
