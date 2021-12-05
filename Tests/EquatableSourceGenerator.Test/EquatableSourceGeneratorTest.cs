using System.Linq;
using EquatableSourceGenerator.Test.DataGenerators;
using EquatableSourceGenerator.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace EquatableSourceGenerator.Test
{
    public sealed class EquatableSourceGeneratorTest
    {
        private readonly ITestOutputHelper _output;

        public EquatableSourceGeneratorTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [ClassData(typeof(DummyModelDataGenerator))]
        [ClassData(typeof(SimpleModelDataGenerator))]
        [ClassData(typeof(DummyInterfaceGeneratorData))]
        [ClassData(typeof(AnotherDummyModelDataGenerator))]
        public void GenerateIEquatableMethods_IsSuccess(string declared, string generated, string dataName)
        {
            _output.WriteLine(dataName);

            var generatedCode = SyntaxTreeGenerator.ParseText(generated).ToString().Trim();
            var expected = SyntaxTreeGenerator.RemoveComments(generatedCode);
            
            var generatedTrees = SyntaxTreeGenerator.GetGeneratedSyntaxTreesByDeclaredType(declared, out var generatorDiags);
            var actual = SyntaxTreeGenerator.CastGeneratedSyntaxTreesToString(generatedTrees);

            Assert.Empty(generatorDiags);
            Assert.Equal(expected, actual);
        }
    }
}