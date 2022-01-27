using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TrueSight.Common;

namespace SampleProject.Entities
{
	public class OrderService : DataEntity, IEquatable<OrderService>
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public DateTime OrderDate { get; set; }
		public long CustomerId { get; set; }
		public decimal Total { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool? Used { get; set; }

		public bool Equals(OrderService other)
		{
			return other != null && Id == other.Id;
		}
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
	public class OrderServiceFilter : FilterEntity
	{
		public IdFilter Id { get; set; }
		public StringFilter Code { get; set; }
		public DateFilter OrderDate { get; set; }
		public IdFilter CustomerId { get; set; }
		public DecimalFilter Total { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<OrderServiceFilter> OrFilter { get; set; }
		public OrderServiceOrder OrderBy { get; set; }
		public OrderServiceSelect Selects { get; set; }
	}
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderServiceOrder
	{
		Id = 0,
		Code = 1,
		OrderDate = 2,
		Total = 3,
		UpdateAt = 4
	}
	[Flags]
	public enum OrderServiceSelect : long
	{
		ALL = E.ALL,
		Id = E._0,
		Code = E._1,
		OrderDate = E._2,
		Total = E._3,
	}
}
