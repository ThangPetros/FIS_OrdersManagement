using System;
using TrueSight.Common;

namespace SampleProject.Entities
{
      public class SystemLog : DataEntity
      {
            public long Id { get; set; }
		public string AppUser { get; set; }
		public string Exception { get; set; }
            public string ModuleName { get; set; }
            public string ClassName { get; set; }
            public string MethodName { get; set; }
            public DateTime Time { get; set; }
      }
}
