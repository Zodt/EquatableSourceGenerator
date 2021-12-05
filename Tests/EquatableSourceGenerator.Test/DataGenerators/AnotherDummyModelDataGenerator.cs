using EquatableSourceGenerator.Test.Helpers;
using EquatableSourceGenerator.Test.Models.AnotherDummyModel;

namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class AnotherDummyModelDataGenerator : ReadDataGenerator
    {
        public AnotherDummyModelDataGenerator() 
            : base(typeof(AnotherDummyModel)) { }
    }
}