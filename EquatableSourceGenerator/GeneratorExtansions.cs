using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EquatableSourceGenerator
{
    internal static class GeneratorExtensions
    {
        private const string EquatableFullName = "System.IEquatable`1";
        public static bool IsNeedRealizationIEquatable(this ITypeSymbol typeSymbol)
        {
            if (!typeSymbol.GetMembers().Any(x => x.MetadataName.Equals(nameof(object.Equals)) || x.MetadataName.Equals(nameof(GetHashCode))))
            {
                return false;
            }
            
            if ($"{typeSymbol.ContainingNamespace}.{typeSymbol.MetadataName}".Equals(EquatableFullName))
            {
                return true;
            }

            return typeSymbol.AllInterfaces
                .Any(x => $"{x.ContainingNamespace}.{x.MetadataName}".Equals(EquatableFullName));
        }


        public static bool IsNeedRealizationIEquatable(this ImmutableArray<INamedTypeSymbol> typeSymbol)
        {
            return typeSymbol.Any(x => x.IsNeedRealizationIEquatable());
        }
    }
}
