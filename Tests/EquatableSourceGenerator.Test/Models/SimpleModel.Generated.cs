using System;/* ToDo: Убрать using */

namespace EquatableSourceGenerator.Test.Models.SimpleModel
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
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfaceProtected);
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfacePublic);
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

namespace EquatableSourceGenerator.Test.Models.SimpleModel
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
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfaceProtected);
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfaceInternal);
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfacePublic);
			hashCode.Add<EquatableSourceGenerator.Test.Models.SimpleModel.IDummyInterface?>(DummyInterfacePublic1);
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