using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    public partial class AnotherDummyModel : IEquatable<AnotherDummyModel>
    {
        private int _id;
        
        public int Id { get; init; }
        public DateTime? CreationDate { get; init; }
    }
}
