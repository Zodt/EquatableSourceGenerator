using System;

namespace EquatableSourceGenerator.Test.Models
{
    public partial class DummyModel : IEquatable<DummyModel>
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
        public string? DummyName { get; init; }
        public DateTime? CreationDate { get; set; }
    }
}