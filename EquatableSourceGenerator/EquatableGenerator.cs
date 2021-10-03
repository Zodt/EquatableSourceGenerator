using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        private const string EquatableFullName = "System.IEquatable`1";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver receiver) return;

            //Debugger.Launch();
            var compilation = context.Compilation;
            foreach (var (typeDeclarationSyntax, syntaxType) in receiver.CandidateTypes)
            {
                var model = compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
                var declaredSymbol = model.GetDeclaredSymbol(typeDeclarationSyntax);
                if (declaredSymbol is not ITypeSymbol typeSymbol) continue;
                if (!IsNeedRealizationIEquatable(typeSymbol, syntaxType)) continue;
                var source = new EquatableCodeCreator(typeSymbol).GenerateEquals();

                context.AddSource($"{typeSymbol.Name}.Equatable.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }


        private static bool IsNeedRealizationIEquatable(ITypeSymbol typeSymbol, DeclarationSyntaxType declarationSyntaxType)
        {
            if (declarationSyntaxType == DeclarationSyntaxType.Interface)
            {
                return $"{typeSymbol.ContainingNamespace}.{typeSymbol.MetadataName}".Equals(EquatableFullName);
            }

            var isTypeImplementIEquatable = true;
            var isTypeAlreadyImplementedIEquatable = true;

            if (declarationSyntaxType == DeclarationSyntaxType.Class)
            {
                isTypeImplementIEquatable = typeSymbol.Interfaces.Any(z => $"{z.ContainingNamespace}.{z.MetadataName}".Equals(EquatableFullName));
                isTypeAlreadyImplementedIEquatable = typeSymbol.GetMembers()
                    .Any(x => x.MetadataName.Equals(nameof(object.Equals)) || x.MetadataName.Equals(nameof(GetHashCode)));
            }

            return isTypeImplementIEquatable && !isTypeAlreadyImplementedIEquatable;
        }
    }

    internal class SyntaxReceiver : ISyntaxReceiver
    {
        internal IList<(TypeDeclarationSyntax TypeDeclarationSyntax, DeclarationSyntaxType SyntaxType)> CandidateTypes { get; } =
            new List<(TypeDeclarationSyntax, DeclarationSyntaxType)>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            var declarationSyntax = syntaxNode as NamespaceDeclarationSyntax;
            if (declarationSyntax is not { } namespaceDeclarationSyntax)
            {
                return;
            }

            for (var i = 0; i < namespaceDeclarationSyntax.Members.Count; i++)
            {
                switch (namespaceDeclarationSyntax.Members[i])
                {
                    case ClassDeclarationSyntax classDeclarationSyntax:
                        CandidateTypes.Add((classDeclarationSyntax, DeclarationSyntaxType.Class));
                        break;

                    case StructDeclarationSyntax structDeclarationSyntax:
                        CandidateTypes.Add((structDeclarationSyntax, DeclarationSyntaxType.Struct));
                        break;

                    case InterfaceDeclarationSyntax interfaceDeclarationSyntax:
                        CandidateTypes.Add((interfaceDeclarationSyntax, DeclarationSyntaxType.Interface));
                        break;

                    default:
                        continue;
                }
            }
        }
    }

    internal enum DeclarationSyntaxType
    {
        Class,
        Struct,
        Interface
    }

}
