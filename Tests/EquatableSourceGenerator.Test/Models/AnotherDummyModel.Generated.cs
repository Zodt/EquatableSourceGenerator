using System;

namespace EquatableSourceGenerator.Test.Models.AnotherDummyModel
{
    partial class AnotherDummyModel
    {
        public bool Equals(AnotherDummyModel? other)
        {
            return other is not null 
               && Id == other.Id
               && CreationDate == other.CreationDate;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is AnotherDummyModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
         hashCode.Add<int>(Id);
         hashCode.Add<System.DateTime?>(CreationDate);
         return hashCode.ToHashCode();
        }

        public static bool operator == (AnotherDummyModel? self, AnotherDummyModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (AnotherDummyModel? self, AnotherDummyModel? other)
        {
            return !(self == other);
        }
    }
}