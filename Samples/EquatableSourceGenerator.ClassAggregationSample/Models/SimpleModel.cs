using System;

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

    public class MyClass1
    {
        
    }
    
}
