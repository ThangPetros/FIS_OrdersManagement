using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class UnitOfMeasureDAO
    {
        public UnitOfMeasureDAO()
        {
            OrderServiceContents = new HashSet<OrderServiceContentDAO>();
            Services = new HashSet<ServiceDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<OrderServiceContentDAO> OrderServiceContents { get; set; }
        public virtual ICollection<ServiceDAO> Services { get; set; }
    }
}
