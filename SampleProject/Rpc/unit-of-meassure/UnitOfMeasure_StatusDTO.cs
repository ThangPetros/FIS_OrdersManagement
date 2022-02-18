using SampleProject.Entities;
using TrueSight.Common;

namespace SampleProject.Rpc.unit_of_meassure
{
      public class UnitOfMeasure_StatusDTO : DataDTO
      {
            public long Id { get; set; }

            public string Code { get; set; }

            public string Name { get; set; }


            public UnitOfMeasure_StatusDTO() { }
            public UnitOfMeasure_StatusDTO(Status Status)
            {

                  this.Id = Status.Id;

                  this.Code = Status.Code;

                  this.Name = Status.Name;

            }
      }

      public class UnitOfMeasure_StatusFilterDTO : FilterDTO
      {

            public IdFilter Id { get; set; }

            public StringFilter Code { get; set; }

            public StringFilter Name { get; set; }

            public StatusOrder OrderBy { get; set; }
      }
}
