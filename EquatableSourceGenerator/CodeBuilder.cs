using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static EquatableSourceGenerator.Helpers.CodeBuilderHelpers;

namespace EquatableSourceGenerator
{
    public ref struct CodeBuilder
    {
        private ImmutableArray<char> _builder;

        public char this[int index] => _builder[index];
        public int Length => _builder.Length;
        public CodeBuilder(ImmutableArray<char> chars) => _builder = chars;

        public CodeBuilder(char[] chars) : this(chars.ToImmutableArray()) { }
        public CodeBuilder(string chars) : this(chars.ToArray()) { }
        public CodeBuilder(IEnumerable<char> chars) : this(chars.ToArray()) { }
        public CodeBuilder(ReadOnlySpan<char> chars) : this(chars.ToArray()) { }

        public CodeBuilder Append(string value)
        {
            var immutableArray = value.ToCharArray();
            _builder = _builder.AddRange(immutableArray);
            return this;
        }

        public CodeBuilder AppendFormat(string template, params string[] values)
        {
            _builder = _builder.AddRange(AppendFormatHelper(template, values.AsSpan()));
            return this;
        }
        
        public CodeBuilder AppendFormat(string template, CodeBuilder codeBuilder)
        {
            _builder = _builder.AddRange(AppendFormatHelper(template, codeBuilder.ToString()));
            return this;
        }

        public CodeBuilder AppendLine()
        {
            _builder = _builder.Add('\r');
            _builder = _builder.Add('\n');

            return this;
        }
        
        public CodeBuilder AppendLine(string value)
        {
            Append(value);
            return AppendLine();
        }
        
        public CodeBuilder Remove(int start, int length)
        {
            _builder = _builder.RemoveRange(start, length);
            return this;
        }


        public static CodeBuilder CreateInstance()
        {
            return new CodeBuilder
            {
                _builder = ImmutableArray<char>.Empty
            };
        }
        
        public override string ToString() => _builder.ToStringResult();
        public static implicit operator string(CodeBuilder x) => x.ToString();
    }
}
