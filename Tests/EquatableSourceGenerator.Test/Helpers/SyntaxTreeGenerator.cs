using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EquatableSourceGenerator.Test.Helpers
{
    public static class SyntaxTreeGenerator
    {
        public static ImmutableArray<SyntaxTree> GetGeneratedSyntaxTreesByDeclaredType(string declared, out ImmutableArray<Diagnostic> generatorDiags)
        {
            var comp = CreateCompilation(declared, OutputKind.DynamicallyLinkedLibrary);
            var newComp = RunGenerators(comp, out generatorDiags, new EquatableGenerator());
            return newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees.ToImmutableArray();
        }

        /*public static SyntaxTree ParseText()
        {
            return CSharpSyntaxTree.ParseText(generated, new CSharpParseOptions(LanguageVersion.Preview));
        }*/

        public static string? CastGeneratedSyntaxTreesToString(this ImmutableArray<SyntaxTree> generatedTrees)
        {
            return generatedTrees.Aggregate(string.Empty,
                (x, y) => $"{x.Trim()}{Environment.NewLine}{y.ToString().Trim()}").Trim();
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
