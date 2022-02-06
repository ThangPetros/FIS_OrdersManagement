using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class CustomerDAO
    {
        public CustomerDAO()
        {
            OrderServices = new HashSet<OrderServiceDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<OrderServiceDAO> OrderServices { get; set; }
    }
}
