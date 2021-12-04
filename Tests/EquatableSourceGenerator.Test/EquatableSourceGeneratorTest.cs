using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace EquatableSourceGenerator.Test
{
    public class EquatableSourceGeneratorTest
    {
        private readonly ITestOutputHelper _output;

        public EquatableSourceGeneratorTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        #region DummyModel

        public const string DummyModelDeclared = @"
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

        public const string DummyModelGenerated = @"using System;

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

        #endregion

        #region AnotherDummyModel

        public const string AnotherDummyModelDeclared = @"
using System;

namespace EquatableSourceGenerator.Sample.Models
{
    public partial class AnotherDummyModel : IEquatable<AnotherDummyModel>
    {
        public int Id { get; init; }
        public DateTime? CreationDate { get; init; }
    }
}
";

        public const string AnotherDummyModelGenerated = @"using System;

namespace EquatableSourceGenerator.Sample.Models
{
    partial class AnotherDummyModel
    {
        public bool Equals(AnotherDummyModel? other)
        {
            return other is not null 
					&& Id == other.Id
					&& CreationDate == other.CreationDate;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is AnotherDummyModel self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
			hashCode.Add<int>(Id);
			hashCode.Add<System.DateTime?>(CreationDate);
			return hashCode.ToHashCode();
        }

        public static bool operator == (AnotherDummyModel? self, AnotherDummyModel? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (AnotherDummyModel? self, AnotherDummyModel? other)
        {
            return !(self == other);
        }
    }
}";

        #endregion

        #region DummyInterface

        public const string DummyInterfaceDeclared = @"using System;

namespace EquatableSourceGenerator.InterfaceImplementationSample.Models
{
    public partial interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }

    public partial class DummyInterface : IDummyInterface, IEquatable<DummyInterface?>
    {
        private int _count;

        public DummyInterface()
        {
            _count = default;
        }
        public Guid PrimaryKey { get; set; }
        public Guid ForeignKey { get; set; }
    }
}
";
        public const string DummyInterfaceGenerated = @"using System;

namespace EquatableSourceGenerator.InterfaceImplementationSample.Models
{
    partial class DummyInterface
    {
        public bool Equals(DummyInterface? other)
        {
            return other is not null 
					&&  == other.
					&& PrimaryKey == other.PrimaryKey
					&& ForeignKey == other.ForeignKey;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return obj.GetType() == this.GetType() 
                || obj is DummyInterface self && Equals(self);
        }
        public override int GetHashCode()
        {
            HashCode hashCode = new();
			hashCode.Add<int>();
			hashCode.Add<System.Guid>(PrimaryKey);
			hashCode.Add<System.Guid>(ForeignKey);
			return hashCode.ToHashCode();
        }

        public static bool operator == (DummyInterface? self, DummyInterface? other)
        {
            return other?.Equals(self) ?? self is null;
        }
        public static bool operator != (DummyInterface? self, DummyInterface? other)
        {
            return !(self == other);
        }
    }
}";

        #endregion

        #region ClassAggregation

        public const string SimpleModelDeclared = @"using System;

namespace EquatableSourceGenerator.ClassAggregationSample.Models
{
    internal sealed partial class SimpleModel : BaseSimpleModel, IEquatable<SimpleModel?>
    {
        protected override IDummyInterface? DummyInterfaceProtected { get; set; }
        public override IDummyInterface? DummyInterfacePublic { get; set; }
        public string? Name { get; set; }
    }

    internal partial class BaseSimpleModel : IEquatable<BaseSimpleModel?>
    {
        protected virtual IDummyInterface? DummyInterfaceProtected { get; set; }
        internal virtual IDummyInterface? DummyInterfaceInternal { get; set; }
        public virtual IDummyInterface? DummyInterfacePublic { get; set; }
        public virtual IDummyInterface? DummyInterfacePublic1 { get; set; }
    }

    public interface IDummyInterface : IEquatable<IDummyInterface?>
    {
        public Guid? PrimaryKey { get; set; }
        public Guid? ForeignKey { get; set; }
    }
}
";

        #endregion

        [Theory]
        [InlineData(DummyModelDeclared)]
        [InlineData(SimpleModelDeclared)]
        [InlineData(AnotherDummyModelDeclared)]
        [InlineData(DummyInterfaceDeclared)]
        public void Can_Generate_IEquatable_Implementation(string declared)
        {
            Compilation comp = CreateCompilation(declared, OutputKind.DynamicallyLinkedLibrary);
            Compilation newComp = RunGenerators(comp, out _, new EquatableGenerator());
            IEnumerable<SyntaxTree> generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees;
            
            
            Assert.NotEmpty(generatedTrees);
            foreach (var generatedTree in generatedTrees)
            {
                _output.WriteLine(generatedTree.ToString().Trim() + "\n\n");
            }
        }
        

        [Theory]
        [InlineData(DummyModelDeclared, DummyModelGenerated)]
        [InlineData(AnotherDummyModelDeclared, AnotherDummyModelGenerated)]
        [InlineData(DummyInterfaceDeclared, DummyInterfaceGenerated)]
        public void GenerateIEquatableMethods_IsSuccess(string declared, string generated)
        {
            SyntaxTree expectedTree = CSharpSyntaxTree.ParseText(generated);
            Compilation comp = CreateCompilation(declared, OutputKind.DynamicallyLinkedLibrary);
            Compilation newComp = RunGenerators(comp, out ImmutableArray<Diagnostic> generatorDiags, new EquatableGenerator());
            IEnumerable<SyntaxTree> generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees;

            Assert.Single(generatedTrees);
            Assert.Empty(generatorDiags);
            Assert.Equal(expectedTree.ToString().Trim(), generatedTrees.First().ToString().Trim());
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