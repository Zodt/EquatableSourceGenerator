using EquatableSourceGenerator.Test.Helpers;
using EquatableSourceGenerator.Test.Models.DummyInterface;

namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class DummyInterfaceGeneratorData : ReadDataGenerator
    {
        public DummyInterfaceGeneratorData() 
            : base(typeof(DummyModelInterface)) { }
    }
}
