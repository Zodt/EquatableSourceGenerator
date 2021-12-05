using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EquatableSourceGenerator.Helpers
{
    public static class CodeBuilderHelpers
    {
        private static readonly Regex Regex = new(@"\{(?<Id>\d+)\}", RegexOptions.Compiled, TimeSpan.FromMilliseconds(1000));

        #region AsImmutableArray

        internal static ImmutableArray<char> AppendFormatHelper(string template, params string[] args) 
        {
            return AppendFormatHelper(template, args.AsSpan());
        }
        internal static ImmutableArray<char> AppendFormatHelper(string template, ReadOnlySpan<string> args) 
        {
            if (template is null) throw new ArgumentNullException(nameof(template));

            var matchCollection = Regex.Matches(template);
            for (var i = 0; i < matchCollection.Count; i++)
            {
                if (int.TryParse(matchCollection[i].Groups["Id"].Value, NumberStyles.Integer, null!, out var id))
                {
                    //Debugger.Launch();
                    template = Regex.Replace(template, $"\\{{{id}\\}}", args[id]);
                }
            }

            return template.ToImmutableArray();
        }
        internal static ImmutableArray<char> AppendFormatHelper(string template, string arg) 
        {
            if (template is null) throw new ArgumentNullException(nameof(template));

            var matchCollection = Regex.Matches(template);
            if (matchCollection.Count > 1)
                throw new Exception("Template was contains more than one data entry");

            return Regex.Replace(template, arg).ToImmutableArray();
        }

        #endregion

        #region AsString

        internal static string AppendFormatHelperAsString(string template, ReadOnlySpan<string> args) 
        {
            if (template is null) throw new ArgumentNullException(nameof(template));

            var matchCollection = Regex.Matches(template);
            for (var i = 0; i < matchCollection.Count; i++)
            {
                if (int.TryParse(matchCollection[i].Groups["Id"].Value, NumberStyles.Integer, null!, out var id))
                {
                    template = Regex.Replace(template, $"\\{{{id}\\}}", args[id]);
                }
            }

            return template;
        }
        internal static string AppendFormatHelperAsString(string template, params string[] args)
        {
            return AppendFormatHelperAsString(template, args.AsSpan());
        }
        internal static string AppendFormatHelperAsString(string template, string arg) 
        {
            if (template is null) throw new ArgumentNullException(nameof(template));

            var matchCollection = Regex.Matches(template);
            if (matchCollection.Count > 1)
                throw new Exception("Template was contains more than one data entry");

            Regex.Replace(template, arg);
            return template;
        }

        #endregion

        internal static string ToStringResult(this ImmutableArray<char> array)
        {
            string result = string.Empty;
            for (var i = 0; i < array.Length; i++)
            {
                result += array[i];
            }
            return result;
        } 
    }
}
