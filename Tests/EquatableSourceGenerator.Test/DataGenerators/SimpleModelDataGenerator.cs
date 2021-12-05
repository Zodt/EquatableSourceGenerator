using EquatableSourceGenerator.Test.Helpers;
using EquatableSourceGenerator.Test.Models.SimpleModel;

namespace EquatableSourceGenerator.Test.DataGenerators
{
    public sealed class SimpleModelDataGenerator : ReadDataGenerator
    {
        public SimpleModelDataGenerator() 
            : base(typeof(SimpleModel)) { }
    }
}
