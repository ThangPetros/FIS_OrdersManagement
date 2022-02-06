using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class ServiceDAO
    {
        public ServiceDAO()
        {
            OrderServiceContents = new HashSet<OrderServiceContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal Prive { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
        public virtual ICollection<OrderServiceContentDAO> OrderServiceContents { get; set; }
    }
}
