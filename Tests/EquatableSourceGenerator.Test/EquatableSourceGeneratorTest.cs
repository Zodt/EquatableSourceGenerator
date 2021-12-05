using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using EquatableSourceGenerator.Test.DataGenerators;
using EquatableSourceGenerator.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            
            SyntaxTree expectedTree = CSharpSyntaxTree.ParseText(generated);
            var generatedTrees = SyntaxTreeGenerator.GetGeneratedSyntaxTreesByDeclaredType(declared, out var generatorDiags);
            
            Assert.Empty(generatorDiags);
            Assert.Equal(expectedTree.ToString().Trim(), generatedTrees.CastGeneratedSyntaxTreesToString());
        }
    }
}