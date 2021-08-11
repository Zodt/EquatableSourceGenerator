using System;

namespace EquatableSourceGenerator.Sample.Models
{
    public partial class AnotherDummyModel : IEquatable<AnotherDummyModel>
    {
        public int Id { get; init; }
        public DateTime? CreationDate { get; init; }
    }
}
