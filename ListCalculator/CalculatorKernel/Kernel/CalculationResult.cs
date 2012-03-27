using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorKernel.Kernel {
    public interface ICalculationResult {
        string PlainText { get; }
        object Tag { get; }
        Type Type { get; }
        int SequenceNumber { get; }
    }
    public interface ICalculationResult<T> : ICalculationResult {
        T Value { get; }
    }

    public abstract class CalculationResultBase : ICalculationResult {
        readonly object tag;
        readonly int sequenceNumber;

        public CalculationResultBase(int sequenceNumber, object tag = null) {
            this.sequenceNumber = sequenceNumber;
            this.tag = tag;
        }
        public object Tag { get { return tag; } }
        public int SequenceNumber { get { return sequenceNumber; } }
        public abstract string PlainText { get; }
        public abstract Type Type { get; }
    }

    public class CalculationResult<T> : CalculationResultBase, ICalculationResult<T> {
        readonly T value;
        string plainText = null;

        public CalculationResult(T value, int sequenceNumber, object tag = null)
            : base(sequenceNumber, tag) {
            this.value = value;
        }
        public T Value { get { return value; } }
        public override Type Type { get { return typeof(T); } }
        public override string PlainText { get { return plainText ?? (plainText = GetPlainText()); } }
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
