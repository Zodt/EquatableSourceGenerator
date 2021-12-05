using System;

// ReSharper disable CheckNamespace
namespace EquatableSourceGenerator.Test.Models.DummyModel
{
    public partial class DummyModel : IEquatable<DummyModel?>
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
        public string? DummyName { get; init; }
        public DateTime? CreationDate { get; set; }
    }
}