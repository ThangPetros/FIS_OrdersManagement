using SampleProject.Entities;
using TrueSight.Common;

namespace SampleProject.Rpc.service
{
	public class Service_UnitOfMeasureDTO: DataDTO
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public long StatusId { get; set; }
		public Service_StatusDTO Status { get; set; }
		public Service_UnitOfMeasureDTO() { }
		public Service_UnitOfMeasureDTO(UnitOfMeasure UnitOfMeasure) {
			this.Id = UnitOfMeasure.Id;
			this.Code = UnitOfMeasure.Code;
			this.Name = UnitOfMeasure.Name;
			this.StatusId = UnitOfMeasure.StatusId;
			this.Status = UnitOfMeasure.Status == null ? null : new Service_StatusDTO(UnitOfMeasure.Status);
			this.Errors = UnitOfMeasure.Errors;
		}
	}
	public class Service_UnitOfMeasureFilterDTO: FilterDTO
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public StringFilter Name { get; set; }
		public IdFilter StatusId { get; set; }
		public ServiceOrder OrderBy { get; set; }
	}
}
