using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace EquatableSourceGenerator.Test
{
    public class EquatableSourceGeneratorTest
    {
        [Fact]
        public void Generate_IsSuccess()
        {
            string source = @"
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
}
";

            string expectedGenerated =
                @"using System;

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
            
            SyntaxTree expectedTree = CSharpSyntaxTree.ParseText(expectedGenerated);
            Compilation comp = CreateCompilation(source, OutputKind.DynamicallyLinkedLibrary);
            Compilation newComp = RunGenerators(comp, out ImmutableArray<Diagnostic> generatorDiags, new EquatableGenerator());
            IEnumerable<SyntaxTree> generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees;
            
            Assert.Single(generatedTrees);
            Assert.Empty(generatorDiags);
        }
        
        private static Compilation CreateCompilation(string source, OutputKind outputKind = OutputKind.ConsoleApplication)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview)) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(outputKind));

        private static GeneratorDriver CreateDriver(Compilation c, params ISourceGenerator[] generators)
            => CSharpGeneratorDriver.Create(generators, parseOptions: (CSharpParseOptions)c.SyntaxTrees.First().Options);

        private static Compilation RunGenerators(Compilation c, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
        {
            CreateDriver(c, generators).RunGeneratorsAndUpdateCompilation(c, out var d, out diagnostics);
            return d;
        }
    }
}