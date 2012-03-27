using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Python = IronPython.Runtime;
using System.Diagnostics.Contracts;

namespace CalculatorKernel.Kernel {
    public static class CalculationResult {
        //HACK: this method get called if value == null
        public static CalculationResultNull Create(object value, int sequenceNumber, object tag) {
            Contract.Assert(value == null);
            return new CalculationResultNull(sequenceNumber, tag);
        }
        public static ICalculationResult<T> Create<T>(T value, int sequenceNumber, object tag) {
            return new CalculationResult<T>(value, sequenceNumber, tag);
        }
    }

    public sealed class CalculationResultNull : CalculationResultBase {
        public CalculationResultNull(int sequenceNumber, object tag)
            : base(sequenceNumber, tag) {
        }
        public override Type Type { get { return null; } }
        public override string PlainText { get { return "Expression was evaluated without the output value."; } }
        public override string ToString() {
            return PlainText;
        }
    }
}
