using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Python = IronPython.Runtime;

namespace CalculatorKernel.Kernel {
    public static class TypeFormatters {
        public static string Format<T>(T value) {
            return Format(value, new StringBuilder());
        }
        static string Format<T>(T value, StringBuilder stringBuilder) {
            Python.List list = value as Python.List;
            if(list != null)
                return "[" + list.StringJoin(v => Format(v)) + "]";
            Python.PythonDictionary dict = value as Python.PythonDictionary;
            if(dict != null)
                return "{" + dict.StringJoin(p => Format(p.Key) + ": " + Format(p.Value)) + "}";
            string str = value as string;
            if(str != null)
                return "'" + str + "'";
            return value.ToString();
        }
        static string StringJoin<T>(this IEnumerable<T> enumerable, Func<T, string> formatter) {
            StringBuilder stringBuilder = new StringBuilder();
            IEnumerator<T> enumerator = enumerable.GetEnumerator();
            if(enumerator.MoveNext())
                stringBuilder.Append(formatter(enumerator.Current));
            while(enumerator.MoveNext())
                stringBuilder.Append(", ").Append(formatter(enumerator.Current));
            return stringBuilder.ToString();
        }
    }
}
