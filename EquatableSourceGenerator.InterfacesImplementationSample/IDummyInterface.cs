using System;

namespace EquatableSourceGenerator.InterfacesImplementationSample
{
    public interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }
}
