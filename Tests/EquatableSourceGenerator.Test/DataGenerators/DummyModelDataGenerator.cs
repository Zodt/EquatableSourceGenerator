using EquatableSourceGenerator.Test.Helpers;

namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class DummyModelDataGenerator : ReadDataGenerator
    {
        public DummyModelDataGenerator() 
            : base(typeof(Models.DummyModel.DummyModel)) { }
    }

}