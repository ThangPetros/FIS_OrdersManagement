using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace SampleProject.Entities
{
    public class UnitOfMeasure : DataEntity, IEquatable<UnitOfMeasure>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Status Status { get; set; }
        public bool Equals(UnitOfMeasure other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class UnitOfMeasureFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public List<UnitOfMeasureFilter> OrFilter { get; set; }
        public UnitOfMeasureOrder OrderBy { get; set; }
        public UnitOfMeasureSelect Selects { get; set; }
        public string Search { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UnitOfMeasureOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Status = 4,
    }

    [Flags]
    public enum UnitOfMeasureSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Status = E._4,
    }
}
