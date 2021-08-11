using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EquatableSourceGenerator
{
    [Generator]
    public class EquatableGenerator : ISourceGenerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var immutableHashSet = syntaxTree.GetRoot()
                    .DescendantNodesAndSelf()
                    .OfType<ClassDeclarationSyntax>()
                    .Select(x => semanticModel.GetDeclaredSymbol(x))
                    .OfType<ITypeSymbol>()
                    .Where(x => x.Interfaces.Any(z => $"{z.ContainingNamespace}.{z.MetadataName}".Equals("System.IEquatable`1")))
                    .ToImmutableHashSet();

                foreach (var typeSymbol in immutableHashSet)
                {
                    var source = new EquatableCodeCreator(typeSymbol).GenerateEquals();
                    context.AddSource($"{typeSymbol.Name}.Equatable.cs",
                        SourceText.From(source, Encoding.UTF8));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize(GeneratorInitializationContext context) { }
    }
}
