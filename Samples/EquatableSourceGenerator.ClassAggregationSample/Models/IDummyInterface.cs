using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    public interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid? PrimaryKey { get; set; }
        public Guid? ForeignKey { get; set; }
    }
}
