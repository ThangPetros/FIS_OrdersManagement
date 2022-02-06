using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TrueSight.Common;

namespace SampleProject.Entities
{
	public class Customer : DataEntity, IEquatable<Customer>
	{
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

            public Status Status { get; set; }

            public bool Equals(Customer other)
		{
                  return other != null && Id == other.Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
      
      public class CustomerFilter: FilterEntity
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
            public StringFilter Name { get; set; }
            public StringFilter Phone { get; set; }
            public StringFilter Address { get; set; }
            public IdFilter StatusId { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<CustomerFilter> OrFilter { get; set; }
		public CustomerOrder OrderBy { get; set; }
		public CustomerSelect Selects { get; set; }
	}
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CustomerOrder
	{
		Id = 0,
		Code = 1,
		Name = 2,
		Address = 3,
		Status = 4,
		UpdateAt = 5
	}
	[Flags]
	public enum CustomerSelect : long
	{
		ALL = E.ALL,
		Id = E._0,
		Code = E._1,
		Name = E._2,
		Address = E._3,
		Status = E._4,
	}
}
