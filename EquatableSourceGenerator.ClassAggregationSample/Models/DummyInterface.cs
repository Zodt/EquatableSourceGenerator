using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    public partial class DummyInterface : IDummyInterface, IEquatable<DummyInterface?>
    {
        public Guid? PrimaryKey { get; set; }
        public Guid? ForeignKey { get; set; }
    }
}
