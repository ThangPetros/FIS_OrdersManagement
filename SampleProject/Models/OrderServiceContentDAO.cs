﻿using System;
using System.Collections.Generic;

namespace SampleProject.Models
{
    public partial class OrderServiceContentDAO
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public long OrderServiceId { get; set; }
        public long PrimaryUnitOfMeasureId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long Quantity { get; set; }
        public long RequestQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual OrderServiceDAO OrderService { get; set; }
        public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
        public virtual ServiceDAO Service { get; set; }
        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
    }
}
