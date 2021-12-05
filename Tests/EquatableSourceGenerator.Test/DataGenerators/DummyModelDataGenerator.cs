namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class DummyModelDataGenerator : BaseDataGenerator
    {
        public DummyModelDataGenerator() 
            : base(DummyModelDeclared, DummyModelGenerated) { }
        
        private const string DummyModelDeclared = @"
using System;

namespace EquatableSourceGenerator.Sample.Models
{
    public partial class DummyModel : IEquatable<DummyModel?>
    {
        public int Id { get; init; }
        public bool IsActive { get; init; }
        public string? DummyName { get; init; }
        public AnotherDummyModel? Model { get; init; }
    }
}";

        private const string DummyModelGenerated = @"using System;

namespace EquatableSourceGenerator.Sample.Models
{
    partial class DummyModel
    {
        public bool Equals(DummyModel? other)
        {
            return other is not null 
					&& Id == other.Id
					&& IsActive == other.IsActive
					&& DummyName == other.DummyName
					&& Model == other.Model;
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
			hashCode.Add<int>(Id);
			hashCode.Add<bool>(IsActive);
			hashCode.Add<string?>(DummyName);
			hashCode.Add<AnotherDummyModel?>(Model);
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
}";
    }
}