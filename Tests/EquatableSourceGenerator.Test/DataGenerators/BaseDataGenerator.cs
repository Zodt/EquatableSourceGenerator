using System.Collections;
using System.Collections.Generic;

namespace EquatableSourceGenerator.Test.DataGenerators
{
    public abstract class BaseDataGenerator : IEnumerable<object?[]>
    {
        private readonly string? _declared;
        private readonly string? _generated;

        protected BaseDataGenerator(string? declared, string? generated)
        {
            _declared = declared;
            _generated = generated;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<object?[]> GetEnumerator() { yield return new object?[] { _declared, _generated, GetType().FullName }; }
    }
}
