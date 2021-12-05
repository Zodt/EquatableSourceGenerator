namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class DummyInterfaceGeneratorData : BaseDataGenerator
    {
        public DummyInterfaceGeneratorData() 
            : base(DummyInterfaceDeclared, DummyInterfaceGenerated) { }
        
        public const string DummyInterfaceDeclared = @"using System;

namespace EquatableSourceGenerator.InterfaceImplementationSample.Models
{
    public partial interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }

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
";
        public const string DummyInterfaceGenerated = @"using System;

namespace EquatableSourceGenerator.InterfaceImplementationSample.Models
{
    partial class DummyInterface
    {
        public bool Equals(DummyInterface? other)
        {
            return other is not null 
					&&  == other.
					&& PrimaryKey == other.PrimaryKey
					&& ForeignKey == other.ForeignKey;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is DummyInterface self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
			hashCode.Add<int>();
			hashCode.Add<System.Guid>(PrimaryKey);
			hashCode.Add<System.Guid>(ForeignKey);
			return hashCode.ToHashCode();
        }

        public static bool operator == (DummyInterface? self, DummyInterface? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (DummyInterface? self, DummyInterface? other)
        {
            return !(self == other);
        }
    }
}";
    }
}
