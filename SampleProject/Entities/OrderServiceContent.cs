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
            public decimal Prive { get; set; }
            public decimal Amount { get; set; }

            public virtual OrderServiceDAO OrderService { get; set; }
            public virtual UnitOfMeasureDAO PrimaryUnitOfMeasure { get; set; }
            public virtual ServiceDAO Service { get; set; }
            public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
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
		public IdFilter ServiceId { get; set; }
		public IdFilter OrderServiceId { get; set; }
		public IdFilter UnitOfMeasureId { get; set; }
		public IdFilter PrimaryUnitOfMeasureId { get; set; }
		public LongFilter Quantity { get; set; }
		public LongFilter RequestQuantity { get; set; }
		public DecimalFilter Prive { get; set; }
		public DecimalFilter Amount { get; set; }
		public List<OrderServiceContentFilter> OrFilter { get; set; }
		public OrderServiceContentOrder OrderBy { get; set; }
		public OrderServiceContentSelect Selects { get; set; }
	}
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderServiceContentOrder
	{
		Service = 0,
		OrderService = 1,
		PrimaryUnitOfMeasure = 2,
		UnitOfMeasure = 3,
		Quantity = 4,
		Prive = 5
	}
	[Flags]
	public enum OrderServiceContentSelect : long
	{
		ALL = E.ALL,
		Service = E._0,
		OrderService = E._1,
		PrimaryUnitOfMeasure = E._2,
		UnitOfMeasure = E._3,
		Quantity = E._4,
		Prive = E._5
	}
}
