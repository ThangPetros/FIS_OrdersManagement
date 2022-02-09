using System.Collections.Generic;
using TrueSight.Common;

namespace SampleProject.Enums
{
	public class StatusEnum
	{
            public static GenericEnum ACTIVE = new GenericEnum { Id = 1, Code = "Active", Name = "Đang hoạt động" };
            public static GenericEnum INACTIVE = new GenericEnum { Id = 0, Code = "Inactive", Name = "Không hoạt động" };

            public static List<GenericEnum> StatusEnumList = new List<GenericEnum>
        {
            INACTIVE, ACTIVE
        };
      }
}
