using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class StatusDAO
    {
        public StatusDAO()
        {
            Customers = new HashSet<CustomerDAO>();
            Services = new HashSet<ServiceDAO>();
            UnitOfMeasures = new HashSet<UnitOfMeasureDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CustomerDAO> Customers { get; set; }
        public virtual ICollection<ServiceDAO> Services { get; set; }
        public virtual ICollection<UnitOfMeasureDAO> UnitOfMeasures { get; set; }
    }
}
