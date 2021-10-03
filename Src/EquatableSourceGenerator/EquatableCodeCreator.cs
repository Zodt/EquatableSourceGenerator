using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace EquatableSourceGenerator
{
    public class EquatableCodeCreator
    {
        private readonly ITypeSymbol _typeSymbol;
        private static readonly Regex FieldRegex = new(@"(?>.*\<)(?<FieldName>.*)\>");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EquatableCodeCreator(ITypeSymbol typeSymbol)
        {
            _typeSymbol = typeSymbol;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string GenerateEquals()
        {
            return $@"using System;

namespace {_typeSymbol.ContainingNamespace}
{{
    partial class {_typeSymbol.Name}
    {{
        public bool Equals({_typeSymbol.Name}? other)
        {{
            return {GeneratePropertiesForEquals()};
        }}
        public override bool Equals(object? obj)
        {{
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is {_typeSymbol.Name} self && Equals(self);
        }}
        public override int GetHashCode()
        {{
            {GetPropertiesHashCode()}
        }}

        public static bool operator == ({_typeSymbol.Name}? self, {_typeSymbol.Name}? other)
        {{
            return other?.Equals(self) ?? self is null;
        }}
        public static bool operator != ({_typeSymbol.Name}? self, {_typeSymbol.Name}? other)
        {{
            return !(self == other);
        }}
    }}
}}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetPropertiesHashCode()
        {
            StringBuilder builder = new();
            builder.AppendLine("HashCode hashCode = new();");
            var tabulation = new string(char.Parse("\t"), 3);

            foreach (var fieldSymbol in _typeSymbol.GetMembers().OfType<IFieldSymbol>())
            {
                var fieldSymbolName = FieldRegex.Match(fieldSymbol.Name).Groups["FieldName"];
                builder.AppendLine($"{tabulation}hashCode.Add<{fieldSymbol.Type}>({fieldSymbolName});");
            }

            return builder.Append($"{tabulation}return hashCode.ToHashCode();").ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GeneratePropertiesForEquals()
        {
            StringBuilder builder = new();
            builder.AppendLine("other is not null ");

            foreach (var fieldSymbol in _typeSymbol.GetMembers().OfType<IFieldSymbol>())
            {
                var fieldSymbolName = FieldRegex.Match(fieldSymbol.Name).Groups["FieldName"];
                builder.AppendLine($"{new string(char.Parse("\t"), 5)}&& {fieldSymbolName} == other.{fieldSymbolName}");
            }

            return builder.Remove(builder.Length - 2, 2).ToString();
        }


    }
}
