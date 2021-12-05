using System;

namespace EquatableSourceGenerator.Test.Models.DummyInterface
{
    partial class DummyModelInterface
    {
        public bool Equals(DummyModelInterface? other)
        {
            return other is not null 
               && PrimaryKey == other.PrimaryKey/* ToDo: не работают отступы как надо*/
               && ForeignKey == other.ForeignKey;
        }
        public bool Equals(EquatableSourceGenerator.Test.Models.DummyInterface.IDummyInterface? other)/* ToDo: Не генерируется */
        {
            return other is not null 
                && PrimaryKey == other.PrimaryKey/* ToDo: не работают отступы как надо*/
                && ForeignKey == other.ForeignKey;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is DummyModelInterface self && Equals(self);/* ToDo: не работают отступы как надо*/
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
         hashCode.Add<System.Guid>(PrimaryKey);/* ToDo: не работают отступы как надо*/
         hashCode.Add<System.Guid>(ForeignKey);
         return hashCode.ToHashCode();
        }

        public static bool operator == (DummyModelInterface? self, DummyModelInterface? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (DummyModelInterface? self, DummyModelInterface? other)
        {
            return !(self == other);
        }
    }
}
