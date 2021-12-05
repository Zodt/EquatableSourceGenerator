using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        public static SyntaxTree ParseText(string generated)
        {
            return CSharpSyntaxTree.ParseText(generated, new CSharpParseOptions(LanguageVersion.Preview));
        }

        public static string CastGeneratedSyntaxTreesToString(ImmutableArray<SyntaxTree> generatedTrees)
        {
            var aggregate = string(string x, SyntaxTree y) => $"{x.Trim()}{Environment.NewLine}{y.ToString().Trim()}";
            var generatedCode = generatedTrees.Aggregate(string.Empty, aggregate);
            return RemoveComments(generatedCode);
        }
        public static string RemoveComments(string generatedCode)
        {
            var correctedCode = generatedCode.Replace("\t", "   ").Trim();
            var removeComments = Regex.Replace(correctedCode, @"[\/\\*]{2}.*[\/\\*]{2}", string.Empty);
            return removeComments;
        }

        private static Compilation CreateCompilation(string source, OutputKind outputKind = OutputKind.ConsoleApplication)
            => CSharpCompilation.Create("compilation",
                new[] { ParseText(source) },
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
