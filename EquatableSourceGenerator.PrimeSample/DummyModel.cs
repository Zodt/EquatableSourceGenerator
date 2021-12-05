using System;

namespace EquatableSourceGenerator.PrimeSample
{
    public partial class DummyModel : IEquatable<DummyModel?>
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string? DummyName { get; init; }
        
    }
}
