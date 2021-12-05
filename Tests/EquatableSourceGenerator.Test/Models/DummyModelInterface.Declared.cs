using System;

// ReSharper disable once CheckNamespace
namespace EquatableSourceGenerator.Test.Models.DummyInterface
{
    public partial interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }

    public partial class DummyModelInterface : IDummyInterface, IEquatable<DummyModelInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }
}
