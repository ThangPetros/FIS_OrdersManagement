using SampleProject.Entities;
using TrueSight.Common;

namespace SampleProject.Rpc.service
{
      public class Service_StatusDTO : DataDTO
      {
            public long Id { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }


            public Service_StatusDTO() { }
            public Service_StatusDTO(Status Status)
            {

                  this.Id = Status.Id;

                  this.Code = Status.Code;

                  this.Name = Status.Name;

            }
      }

      public class Service_StatusFilterDTO : FilterDTO
      {

            public IdFilter Id { get; set; }

            public StringFilter Code { get; set; }

            public StringFilter Name { get; set; }

            public StatusOrder OrderBy { get; set; }
      }
}
