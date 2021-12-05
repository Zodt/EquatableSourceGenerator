namespace EquatableSourceGenerator
{
    internal readonly ref struct EquatableTemplates
    {
        internal const string EquatableNamespacePartialClassTemplate = @"{0}
namespace {1}
{{
    partial class {2}
    {{
        {3}    
    }}
}}";
        internal const string HashCodeTemplate = @"
        public override int GetHashCode()
        {
            {0}
        }";
        
        internal const string StaticEqualsOperatorsTemplate = @"
        public static bool operator == ({0} self, {0} other)
        {{
            return other?.Equals(self) ?? self is null;
        }}
        public static bool operator != ({0} self, {0} other)
        {{
            return !(self == other);
        }}";

        internal const string ObjectEqualsTypeCallEqualsTemplate = @"
                       && Equals(({0})obj)";

        internal const string EqualsTypeTemplate = @"
        public bool Equals({0} other)
        {
            return {1};
        }";

        internal const string ObjectEqualsTemplate = @"
        public override bool Equals(object? obj)
        {{
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            return this.GetType() == obj.GetType() {0};
        }}";


        internal const string PropertyTypeEqualsTemplate = @"{0}&& {1} == other.{1}";
        internal const string EquatablePropertyTypeEqualsTemplate = @"{0}&& {1}.Equals(other.{1})";
        internal const string NullableAnnotationPropertyEqualsTemplate = @"{0}&& Nullable.Equals({1}, other.{1})";


    }
}
