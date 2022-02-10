//using SampleProject.Entities;
//using SampleProject.Common;
//using System;
//using TrueSight.Common;

//namespace SampleProject.Rpc.Customer
//{
//    public class Customer_CustomerDTO : DataDTO
//    {
//        public long Id { get; set; }
//        public string Code { get; set; }
//        public string Name { get; set; }
//        public string Phone { get; set; }
//        public string Address { get; set; }
//        public long? StatusId { get; set; }
//        public Customer_StatusDTO Status { get; set; }
//        public Customer_CustomerDTO() { }
//        public Customer_CustomerDTO(Customer Customer)
//        {
//            this.Id = Customer.Id;
//            this.Code = Customer.Code;
//            this.Name = Customer.Name;
//            this.Phone = Customer.Phone;
//            this.Address = Customer.Address;
//            this.Status = Customer.Status == null ? null : new Customer_CustomerDTO(Customer.Status);
//        }
//    }
//    public class Customer_CustomerFilterDTO : FilterDTO
//    {
//        public IdFilter Id { get; set; }
//        public StringFilter Code { get; set; }
//        public StringFilter Name { get; set; }
//        public StringFilter Phone { get; set; }
//        public StringFilter Address { get; set; }
//        public IdFilter StatusId { get; set; }
//        public CustomerOrder OrderBy { get; set; }
//        public CustomerSelect Selects { get; set; }
//    }
//}
