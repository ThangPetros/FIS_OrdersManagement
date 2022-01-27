using NGS.Templater;
using System;

namespace SampleProject.Helper
{
      public class StaticParams
      {
            public static int ChangeYear = 0;
            public static DateTime DateTimeNow => DateTime.UtcNow.AddYears(ChangeYear);
            public static DateTime DateTimeMin => DateTime.MinValue;
            //public static string ExcelFileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public static string ModuleName = "SampleProject";
            public static bool EnableExternalService = true;
            public static IDocumentFactory DocumentFactory => Configuration.Builder.Build("ToanTV19@fpt.com.vn", "n8seTdTnm6lKE/bJAtLDfz2+YCES84tPeDpORepZ95ZtffIzEPc83DSzTJ74aUDt7qwochs3JjlyLJ3RAg809IGyiSTttv1iScB1KSdLHNdscIbkang7DV1f3uBA1RKncDBa1Y2UG/EMIhyEcm48COCYWQc1YNAjNBG9L5LsZ5JjEa8B55US5iRdEDPLuCB+Kdchw6+jM+k4vrQTTNbwYMSCSMpzk1APSmZCpsfjliQxsODHReOUwvBUPK8KOD9jqewrfFJ0Nh0ZNYMFHFZf1efi0oN2lG1l/lcEzTdIEi0BcLuUhXHStMQaqJmEf0voFjJfG0I/c+N30E50j0RncIxOKg8rEvrdASzGUkBdEeU4bNlq1FhGOAiUHPVi/B4oqKHEfFOwTsHfnUmSGRpufdffp9zBvg8DYPYaj00xaysZ7y2x7EQoIPX0KRXqY0Pt+AEEOp5KfsQmhsUKV2Ajd2+hb3PqBvD3W8I/miehwRXpNjzIuLCCTZanBtz+8r+me58loFYUY0fnKz5vSwZ1kNsCFFYR5f7ILBp+RdwgduhLQqlHsoTIn24zS3DCKLytxydz/J5O8TgZzwl6cgixZEtB6MVMbsokrhAH6XSs36sCuoxVpEwqIIz4al8AreU65IwZIlz4FZ19g2/Oarej+/qAdFTk3ih29Mwrnbj+dTw=");
      }
}
