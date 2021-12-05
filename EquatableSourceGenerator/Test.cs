using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using EquatableSourceGenerator.Helpers;
using Microsoft.CodeAnalysis;

namespace EquatableSourceGenerator
{
    internal readonly ref struct EquatableCodeCreatorTest
    {
        private readonly ITypeSymbol _typeSymbol;
        private readonly Compilation _compilation;
        private readonly ImmutableArray<ITypeSymbol>? _namedTypeSymbols;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal EquatableCodeCreatorTest(Compilation compilation, ITypeSymbol typeSymbol, ImmutableArray<ITypeSymbol>? namedTypeSymbols)
        {
            _typeSymbol = typeSymbol;
            _compilation = compilation;
            _namedTypeSymbols = namedTypeSymbols;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal string Generate()
        {
            var generateTypeContent = GenerateTypeContent();
            return string.Format(EquatableTemplates.EquatableNamespacePartialClassTemplate,
                "using System;\r\n",
                _typeSymbol.ContainingNamespace,
                _typeSymbol.Name,
                generateTypeContent);
        }

        private string GenerateTypeContent()
        {
            var builder = CodeBuilder.CreateInstance();
            
            var tabulation = new string(char.Parse("\t"), 5);
            CodeBuilder hashCodeBuilder = new("HashCode hashCode = new();\n");
            if (_namedTypeSymbols is { })
            {
                foreach (var typeSymbol in _namedTypeSymbols)
                {
                    string typeEquals = string.Empty;
                    var equalsBuilder = CodeBuilder.CreateInstance();
                    bool isBaseType = !SymbolEqualityComparer.Default.Equals(_typeSymbol.BaseType, typeSymbol);

                    if (isBaseType)
                    {
                        equalsBuilder.AppendLine("other is not null");
                    }
                    if (_typeSymbol.BaseType is { } baseType && baseType.MetadataName != "Object")
                    {
                        equalsBuilder.AppendFormat(
                            baseType.AllInterfaces.IsNeedRealizationIEquatable()
                                ? EquatableTemplates.PropertyTypeEqualsTemplate
                                : EquatableTemplates.EquatablePropertyTypeEqualsTemplate, tabulation, baseType.MetadataName);
                        hashCodeBuilder.AppendLine($"{tabulation[..^2]}hashCode.Add(base.GetHashCode());");
                    }

                    foreach (var symbol in typeSymbol.GetMembers())
                    {
                        if (symbol is not IPropertySymbol propertySymbol) continue;

                        var symbolKind = propertySymbol.Kind == SymbolKind.Property;
                        var symbolAccessibility = propertySymbol.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal;
                        var isVirtual = propertySymbol.IsVirtual && typeSymbol.IsSealed || !propertySymbol.IsVirtual;// ???

                        if (!(symbolKind && symbolAccessibility && isVirtual)) continue;

                        if (isBaseType)
                        {
                            string template = propertySymbol.Type.AllInterfaces.IsNeedRealizationIEquatable() switch
                            {
                                true => propertySymbol.NullableAnnotation switch
                                {
                                    NullableAnnotation.Annotated => EquatableTemplates.NullableAnnotationPropertyEqualsTemplate,
                                    _ => EquatableTemplates.EquatablePropertyTypeEqualsTemplate
                                },
                                false => EquatableTemplates.PropertyTypeEqualsTemplate
                            };
                            equalsBuilder.AppendFormat(template, tabulation, propertySymbol.MetadataName);
                        }

                        hashCodeBuilder.AppendLine($"{tabulation[..^2]}hashCode.Add<{propertySymbol.Type}>({propertySymbol.MetadataName});");
                    }

                    if (isBaseType)
                    {
                        typeEquals = CodeBuilderHelpers
                            .AppendFormatHelperAsString(EquatableTemplates.EqualsTypeTemplate,
                                GetEqualsType(typeSymbol), equalsBuilder);
                    }
                    builder.AppendLine(typeEquals);
                }

                #region EndHashCode
                //Debugger.Launch();

                hashCodeBuilder.Append($"{tabulation[..^2]}return hashCode.ToHashCode();");
                builder.AppendFormat(EquatableTemplates.HashCodeTemplate, hashCodeBuilder);

                #endregion

                #region ObjectEquals

                string objectEquals = string.Empty;
                if (_namedTypeSymbols is not null && _namedTypeSymbols.Value.Any())
                {
                    var objectEqualsCodeBuilder = CodeBuilder.CreateInstance();
                    foreach (var objectEqualsSymbol in _namedTypeSymbols.Value)
                    {
                        objectEqualsCodeBuilder.AppendFormat(EquatableTemplates.ObjectEqualsTypeCallEqualsTemplate,
                            GetEqualsType(objectEqualsSymbol));
                    }
                    objectEquals = string.Format(EquatableTemplates.ObjectEqualsTemplate, objectEqualsCodeBuilder);
                }
                builder.AppendLine(objectEquals);

                #endregion

                return builder.ToString();
            }

            #region StaticEquals

            string staticEquals = string.Empty;
            if (_typeSymbol is { TypeKind: TypeKind.Class })
            {
                staticEquals = string.Format(EquatableTemplates.StaticEqualsOperatorsTemplate, GetEqualsType(_typeSymbol));
            }
            builder.AppendLine(staticEquals);

            #endregion

            if (_namedTypeSymbols is null)
            {
                string typeEquals = string.Empty;
                var equalsBuilder = CodeBuilder.CreateInstance();
                bool isBaseType = !SymbolEqualityComparer.Default.Equals(_typeSymbol.BaseType, _typeSymbol);

                if (isBaseType)
                {
                    equalsBuilder.AppendLine("other is not null");
                }
                if (_typeSymbol.BaseType is { } baseType && baseType.MetadataName != "Object")
                {
                    equalsBuilder.AppendFormat(
                        baseType.AllInterfaces.IsNeedRealizationIEquatable()
                            ? EquatableTemplates.PropertyTypeEqualsTemplate
                            : EquatableTemplates.EquatablePropertyTypeEqualsTemplate, tabulation, baseType.MetadataName);
                    hashCodeBuilder.AppendLine($"{tabulation[..^2]}hashCode.Add(base.GetHashCode());");
                }

                foreach (var symbol in _typeSymbol.GetMembers())
                {
                    if (symbol is not IPropertySymbol propertySymbol) continue;

                    var symbolKind = propertySymbol.Kind == SymbolKind.Property;
                    var symbolAccessibility = propertySymbol.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal;
                    var isVirtual = propertySymbol.IsVirtual && _typeSymbol.IsSealed || !propertySymbol.IsVirtual;// ???

                    if (!(symbolKind && symbolAccessibility && isVirtual)) continue;

                    if (isBaseType)
                    {
                        string template = propertySymbol.Type.AllInterfaces.IsNeedRealizationIEquatable() switch
                        {
                            true => propertySymbol.NullableAnnotation switch
                            {
                                NullableAnnotation.Annotated => EquatableTemplates.NullableAnnotationPropertyEqualsTemplate,
                                _ => EquatableTemplates.EquatablePropertyTypeEqualsTemplate
                            },
                            false => EquatableTemplates.PropertyTypeEqualsTemplate
                        };
                        equalsBuilder.AppendFormat(template, tabulation, propertySymbol.MetadataName);
                    }

                    hashCodeBuilder.AppendLine($"{tabulation[..^2]}hashCode.Add<{propertySymbol.Type}>({propertySymbol.MetadataName});");
                }

                if (isBaseType)
                {
                    equalsBuilder.Remove(equalsBuilder.Length - 2, 2);
                    typeEquals = CodeBuilderHelpers
                        .AppendFormatHelperAsString(EquatableTemplates.EqualsTypeTemplate,
                            GetEqualsType(_typeSymbol), equalsBuilder);
                }

                builder.AppendLine(typeEquals);
                hashCodeBuilder.Append($"{tabulation[..^2]}return hashCode.ToHashCode();");
                builder.AppendFormat(EquatableTemplates.HashCodeTemplate, hashCodeBuilder);
                builder.Append(GetObjectEquals());
            }

            return builder.ToString();
        }
        private void PropertyHandler(ITypeSymbol typeSymbol, string tabulation, CodeBuilder builder, CodeBuilder hashCodeBuilder)
        {


        }
        private string GetObjectEquals()
        {
            if (_namedTypeSymbols is null || !_namedTypeSymbols.Value.Any())
                return string.Empty;

            var equalsType = _namedTypeSymbols.Value
                .Select(x =>
                    CodeBuilderHelpers
                        .AppendFormatHelperAsString(EquatableTemplates.ObjectEqualsTypeCallEqualsTemplate,
                            GetEqualsType(x)))
                .Aggregate(string.Concat);

            return CodeBuilderHelpers
                .AppendFormatHelperAsString(EquatableTemplates.ObjectEqualsTemplate, equalsType);
        }
        private static string GetEqualsType(ITypeSymbol? typeSymbol)
        {
            return typeSymbol switch
            {
                { NullableAnnotation: NullableAnnotation.Annotated } => typeSymbol.OriginalDefinition
                    .WithNullableAnnotation(NullableAnnotation.Annotated).ToString(),
                { } => $"{typeSymbol.OriginalDefinition}?",
                _ => throw new ArgumentException()
            };
        }

        private string GetEqualsTypeTemplate(ITypeSymbol? typeSymbol)
        {
            if (SymbolEqualityComparer.Default.Equals(_typeSymbol.BaseType, typeSymbol))
            {
                return string.Empty;
            }
            var equalsType = GetEqualsType(typeSymbol);

            return CodeBuilderHelpers
                .AppendFormatHelperAsString(EquatableTemplates.EqualsTypeTemplate,
                    equalsType, GeneratePropertiesForEquals());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GeneratePropertiesForEquals()
        {
            var tabulation = new string(char.Parse("\t"), 5);
            CodeBuilder equalsBuilder = new("other is not null");
            CodeBuilder hashCodeBuilder = new("HashCode hashCode = new();");

            equalsBuilder = GetStartEqualsLine(equalsBuilder, hashCodeBuilder, tabulation);
            equalsBuilder = GetProperties(equalsBuilder, hashCodeBuilder, _typeSymbol, tabulation);

            return equalsBuilder.ToString();
        }
        private CodeBuilder GetStartEqualsLine(CodeBuilder equalsBuilder, CodeBuilder hashCodeBuilder, string tabulation)
        {
            if (_typeSymbol.BaseType is not { } baseType) return equalsBuilder;

            if (!baseType.AllInterfaces.IsNeedRealizationIEquatable())
            {
                hashCodeBuilder.AppendLine($"{tabulation}hashCode.Add(base.GetHashCode());");
                return GetProperties(equalsBuilder.AppendLine(), hashCodeBuilder, _typeSymbol.BaseType, tabulation);
            }

            equalsBuilder.AppendLine($"{tabulation}&& base.Equals(other as {baseType})");
            hashCodeBuilder.AppendLine($"{tabulation}hashCode.Add(base.GetHashCode());");
            return equalsBuilder;

        }

        private static CodeBuilder GetProperties(CodeBuilder equalsBuilder, CodeBuilder hashCodeBuilder, ITypeSymbol typeSymbol, string tabulation)
        {
            foreach (var symbol in typeSymbol.GetMembers())
            {
                if (symbol is not IPropertySymbol propertySymbol) continue;

                var symbolKind = propertySymbol.Kind == SymbolKind.Property;
                var symbolAccessibility = propertySymbol.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal;
                var isVirtual = propertySymbol.IsVirtual && typeSymbol.IsSealed || !propertySymbol.IsVirtual;// ???

                if (!(symbolKind && symbolAccessibility && isVirtual)) continue;

                string template = propertySymbol.Type.AllInterfaces.IsNeedRealizationIEquatable() switch
                {
                    true => propertySymbol.NullableAnnotation switch
                    {
                        NullableAnnotation.Annotated => EquatableTemplates.NullableAnnotationPropertyEqualsTemplate,
                        _ => EquatableTemplates.EquatablePropertyTypeEqualsTemplate
                    },
                    false => EquatableTemplates.PropertyTypeEqualsTemplate
                };

                equalsBuilder.AppendFormat(template, tabulation, propertySymbol.MetadataName);
                hashCodeBuilder.AppendLine($"{tabulation}hashCode.Add<{propertySymbol.Type}>({propertySymbol.MetadataName});");
            }
            return equalsBuilder.Remove(equalsBuilder.Length - 2, 2);
        }
    }


}
