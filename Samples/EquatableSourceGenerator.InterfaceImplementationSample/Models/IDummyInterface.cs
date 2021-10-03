using System;

namespace EquatableSourceGenerator.InterfaceImplementationSample.Models
{
    public partial interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }
}
