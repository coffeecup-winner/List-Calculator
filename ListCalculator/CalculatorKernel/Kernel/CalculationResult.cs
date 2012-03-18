using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorKernel.Kernel {
    public interface ICalculationResult {
        string PlainText { get; }
    }
    public interface ICalculationResult<T> : ICalculationResult {
        T Value { get; }
    }

    public static class CalculationResult {
        public static CalculationResult<T> Create<T>(T value) {
            return new CalculationResult<T>(value);
        }
    }

    public class CalculationResult<T> : ICalculationResult<T> {
        readonly T value;
        string plainText = null;

        public CalculationResult(T value) {
            this.value = value;
        }
        public T Value { get { return value; } }
        public string PlainText { get { return plainText ?? (plainText = Value.ToString()); } }
    }

    public class CalculationCompletedEventArgs : EventArgs {
        readonly ICalculationResult result;

        public CalculationCompletedEventArgs(ICalculationResult result) {
            this.result = result;
        }
        public ICalculationResult Result { get { return result; } }
    }
}
