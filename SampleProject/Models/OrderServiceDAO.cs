using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class OrderServiceDAO
    {
        public OrderServiceDAO()
        {
            OrderServiceContents = new HashSet<OrderServiceContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderDate { get; set; }
        public long CustomerId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? Used { get; set; }

        public virtual CustomerDAO Customer { get; set; }
        public virtual ICollection<OrderServiceContentDAO> OrderServiceContents { get; set; }
    }
}
