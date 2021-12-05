using System;

namespace EquatableSourceGenerator.InterfacesImplementationSample
{
    public partial class DummyInterface : IDummyInterface, IEquatable<DummyInterface?>
    {
        private int _count;
        
        public DummyInterface()
        {
            _count = default;
        }
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }
}
