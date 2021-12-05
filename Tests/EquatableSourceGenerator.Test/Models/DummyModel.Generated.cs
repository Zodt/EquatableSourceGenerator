using System;

namespace EquatableSourceGenerator.Test.Models
{
    partial class DummyModel
    {
        public bool Equals(DummyModel? other)
        {
            return other is not null 
                   && Id == other.Id
                   && IsActive == other.IsActive
                   && Nullable.Equals(DummyName, other.DummyName)
                   && Nullable.Equals(CreationDate, other.CreationDate);
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                   || obj is DummyModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add<Guid>(Id);
            hashCode.Add<bool>(IsActive);
            hashCode.Add<string?>(DummyName);
            hashCode.Add<DateTime?>(CreationDate);
            return hashCode.ToHashCode();
        }

        public static bool operator == (DummyModel? self, DummyModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (DummyModel? self, DummyModel? other)
        {
            return !(self == other);
        }
    }
}
