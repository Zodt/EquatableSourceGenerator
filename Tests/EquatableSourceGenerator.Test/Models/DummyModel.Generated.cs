using System;

namespace EquatableSourceGenerator.Test.Models.DummyModel
{
    partial class DummyModel
    {
        public bool Equals(DummyModel? other)
        {
            return other is not null 
               && Id == other.Id/* ToDo: не работают отступы как надо*/
               && IsActive == other.IsActive
               && DummyName == other.DummyName
               && CreationDate == other.CreationDate;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is DummyModel self && Equals(self);/* ToDo: не работают отступы как надо*/
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
         hashCode.Add<System.Guid>(Id);/* ToDo: не работают отступы как надо*/
         hashCode.Add<bool>(IsActive);
         hashCode.Add<string?>(DummyName);
         hashCode.Add<System.DateTime?>(CreationDate);
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
