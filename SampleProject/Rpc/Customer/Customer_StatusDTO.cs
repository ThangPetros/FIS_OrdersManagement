using SampleProject.Entities;
using TrueSight.Common;

namespace SampleProject.Rpc.customer
{
    public class Customer_StatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Customer_StatusDTO() { }
        public Customer_StatusDTO(Status Status)
        {
            this.Id = Status.Id;
            this.Code = Status.Code;
            this.Name = Status.Name;
        }
        public class Customer_StatusFilterDTO : FilterDTO
        {
            public IdFilter Id { get; set; }
            public StringFilter Code { get; set; }
            public StringFilter Name { get; set; }
            public StatusOrder Orderby { get; set; }
        }
    }
}
