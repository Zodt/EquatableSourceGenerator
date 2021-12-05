using System;

// ReSharper disable once CheckNamespace
namespace EquatableSourceGenerator.Test.Models.AnotherDummyModel
{
    public partial class AnotherDummyModel : IEquatable<AnotherDummyModel>
    {
        public int Id { get; init; }
        public DateTime? CreationDate { get; init; }
    }
}