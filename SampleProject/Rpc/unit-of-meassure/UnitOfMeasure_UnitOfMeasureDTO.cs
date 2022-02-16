using SampleProject.Entities;
using TrueSight.Common;

namespace SampleProject.Rpc.unit_of_meassure
{
      public class UnitOfMeasure_UnitOfMeasureDTO : DataDTO
      {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public long StatusId { get; set; }
            public UnitOfMeasure_StatusDTO Status { get; set; }
            public UnitOfMeasure_UnitOfMeasureDTO() { }
            public UnitOfMeasure_UnitOfMeasureDTO(UnitOfMeasure UnitOfMeasure)
            {
                  this.Id = UnitOfMeasure.Id;
                  this.Code = UnitOfMeasure.Code;
                  this.Name = UnitOfMeasure.Name;
                  this.StatusId = UnitOfMeasure.StatusId;
                  this.Status = UnitOfMeasure.Status == null ? null : new UnitOfMeasure_StatusDTO(UnitOfMeasure.Status);
                  this.Errors = UnitOfMeasure.Errors;
            }
      }

      public class UnitOfMeasure_UnitOfMeasureFilterDTO : FilterDTO
      {
            public IdFilter Id { get; set; }
            public StringFilter Code { get; set; }
            public StringFilter Name { get; set; }
            public IdFilter StatusId { get; set; }
            public UnitOfMeasureOrder OrderBy { get; set; }
            public string Search { get; set; }
      }
}
