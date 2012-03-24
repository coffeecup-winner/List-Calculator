using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorKernel.Kernel {
    public interface ICalculationResult {
        string PlainText { get; }
        object Tag { get; }
        Type Type { get; }
    }
    public interface ICalculationResult<T> : ICalculationResult {
        T Value { get; }
    }

    public class CalculationResult<T> : ICalculationResult<T> {
        readonly T value;
        readonly object tag;
        string plainText = null;

        public CalculationResult(T value, object tag = null) {
            this.value = value;
            this.tag = tag;
        }
        public T Value { get { return value; } }
        public Type Type { get { return typeof(T); } }
        public string PlainText { get { return plainText ?? (plainText = GetPlainText()); } }
        public object Tag { get { return tag; } }
        protected virtual string GetPlainText() {
            return TypeFormatters.Format(Value);
        }
        public override string ToString() {
            return PlainText;
        }
    }

    public class CalculationCompletedEventArgs : EventArgs {
        readonly ICalculationResult result;

        public CalculationCompletedEventArgs(ICalculationResult result) {
            this.result = result;
        }
        public ICalculationResult Result { get { return result; } }
    }
}
