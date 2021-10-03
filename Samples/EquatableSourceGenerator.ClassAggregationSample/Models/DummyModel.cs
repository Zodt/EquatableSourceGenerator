using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    public partial class DummyModel : IEquatable<DummyModel?>
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string? DummyName { get; init; }
        public AnotherDummyModel? Model { get; init; }

        public IDummyInterface? DummyInterface { get; set; }
        
    }
}
