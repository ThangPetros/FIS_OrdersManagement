using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TrueSight.Common;

namespace SampleProject.Entities
{
	public class Service : DataEntity, IEquatable<Service>
	{
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
		public Status Status { get; set; }

		public bool Equals(Service other)
		{
			return other != null && Id == other.Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
	public class ServiceFilter : FilterEntity
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public StringFilter Name { get; set; }
		public IdFilter UnitOfMeasureId { get; set; }
		public DecimalFilter Prive { get; set; }
		public IdFilter StatusId { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<ServiceFilter> OrFilter { get; set; }
		public ServiceOrder OrderBy { get; set; }
		public ServiceSelect Selects { get; set; }
	}
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ServiceOrder
	{
		Id = 0,
		Code = 1,
		Name = 2,
		UnitOfMeasure = 3,
		Prive = 4,
		Status = 5,
		UpdateAt = 6
	}
	[Flags]
	public enum ServiceSelect : long
	{
		ALL = E.ALL,
		Id = E._0,
		Code = E._1,
		Name = E._2,
		UnitOfMeasure = E._3,
		Prive = E._4,
		Status = E._5
	}
}
