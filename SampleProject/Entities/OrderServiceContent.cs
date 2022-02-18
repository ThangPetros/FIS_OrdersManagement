using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using SampleProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using TrueSight.Common;

namespace SampleProject.Entities
{
	public class OrderServiceContent : DataEntity, IEquatable<OrderServiceContent>
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

		public virtual OrderService OrderService { get; set; }
            public virtual UnitOfMeasure PrimaryUnitOfMeasure { get; set; }
            public virtual Service Service { get; set; }
            public virtual UnitOfMeasure UnitOfMeasure { get; set; }
            public bool Equals(OrderServiceContent other)
		{
			return other != null && ServiceId == other.ServiceId && OrderServiceId == other.OrderServiceId && PrimaryUnitOfMeasureId == other.PrimaryUnitOfMeasureId && UnitOfMeasureId == other.UnitOfMeasureId;
		}
		public override int GetHashCode()
		{
			return ServiceId.GetHashCode() ^ OrderServiceId.GetHashCode() ^ PrimaryUnitOfMeasureId.GetHashCode() ^ UnitOfMeasureId.GetHashCode();
		}
	}
	public class OrderServiceContentFilter : FilterEntity
	{
		public IdFilter Id { get; set; }
		public IdFilter ServiceId { get; set; }
		public IdFilter OrderServiceId { get; set; }
		public IdFilter UnitOfMeasureId { get; set; }
		public IdFilter PrimaryUnitOfMeasureId { get; set; }
		public LongFilter Quantity { get; set; }
		public LongFilter RequestQuantity { get; set; }
		public DecimalFilter Price { get; set; }
		public DecimalFilter Amount { get; set; }
		public DateFilter UpdateTime { get; set; }
		public List<OrderServiceContentFilter> OrFilter { get; set; }
		public OrderServiceContentOrder OrderBy { get; set; }
		public OrderServiceContentSelect Selects { get; set; }
	}
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderServiceContentOrder
	{
		Id = 0,
		Service = 1,
		OrderService = 2,
		PrimaryUnitOfMeasure = 3,
		UnitOfMeasure = 4,
		Quantity = 5,
		Price = 6
	}
	[Flags]
	public enum OrderServiceContentSelect : long
	{
		ALL = E.ALL,
		Id = E._0,
		Service = E._1,
		OrderService = E._2,
		PrimaryUnitOfMeasure = E._3,
		UnitOfMeasure = E._4,
		Quantity = E._5,
		Price = E._6
	}
}
