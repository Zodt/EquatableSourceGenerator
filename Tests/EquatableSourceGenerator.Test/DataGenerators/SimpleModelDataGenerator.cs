namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class SimpleModelDataGenerator : BaseDataGenerator
    {
        public SimpleModelDataGenerator() 
            : base(SimpleModelDeclared, SimpleModelGenerated) { }
        
        public const string SimpleModelDeclared = @"using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    internal sealed partial class SimpleModel : BaseSimpleModel, IEquatable<SimpleModel?>
    {
        protected override IDummyInterface? DummyInterfaceProtected { get; set; }
        public override IDummyInterface? DummyInterfacePublic { get; set; }
        public string? Name { get; set; }
    }

    internal partial class BaseSimpleModel : IEquatable<BaseSimpleModel?>
    {
        protected virtual IDummyInterface? DummyInterfaceProtected { get; set; }
        internal virtual IDummyInterface? DummyInterfaceInternal { get; set; }
        public virtual IDummyInterface? DummyInterfacePublic { get; set; }
        public virtual IDummyInterface? DummyInterfacePublic1 { get; set; }
    }

    public interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid? PrimaryKey { get; set; }
        public Guid? ForeignKey { get; set; }
    }
}";
        public const string SimpleModelGenerated = @"using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    partial class SimpleModel
    {
        public bool Equals(SimpleModel? other)
        {
            return other is not null 
					&& DummyInterfaceProtected == other.DummyInterfaceProtected
					&& DummyInterfacePublic == other.DummyInterfacePublic
					&& Name == other.Name;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is SimpleModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfaceProtected);
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfacePublic);
			hashCode.Add<string?>(Name);
			return hashCode.ToHashCode();
        }

        public static bool operator == (SimpleModel? self, SimpleModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (SimpleModel? self, SimpleModel? other)
        {
            return !(self == other);
        }
    }
}
using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    partial class BaseSimpleModel
    {
        public bool Equals(BaseSimpleModel? other)
        {
            return other is not null 
					&& DummyInterfaceProtected == other.DummyInterfaceProtected
					&& DummyInterfaceInternal == other.DummyInterfaceInternal
					&& DummyInterfacePublic == other.DummyInterfacePublic
					&& DummyInterfacePublic1 == other.DummyInterfacePublic1;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is BaseSimpleModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfaceProtected);
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfaceInternal);
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfacePublic);
			hashCode.Add<EquatableSourceGenerator.ClassAggregationSample.Models.IDummyInterface?>(DummyInterfacePublic1);
			return hashCode.ToHashCode();
        }

        public static bool operator == (BaseSimpleModel? self, BaseSimpleModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (BaseSimpleModel? self, BaseSimpleModel? other)
        {
            return !(self == other);
        }
    }
}
";
    }
}
