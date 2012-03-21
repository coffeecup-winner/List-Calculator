using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Python = IronPython.Runtime;
using System.Diagnostics.Contracts;

namespace CalculatorKernel.Kernel {
    public static class CalculationResult {
        //HACK: this method get called if value == null
        public static CalculationResultNull Create(object value, object tag) {
            Contract.Assert(value == null);
            return new CalculationResultNull(tag);
        }
        public static ICalculationResult<T> Create<T>(T value, object tag) {
            return new CalculationResult<T>(value, tag);
        }
    }

    public sealed class CalculationResultNull : ICalculationResult {
        readonly object tag;

        public CalculationResultNull(object tag) {
            this.tag = tag;
        }
        public object Tag { get { return tag; } }
        public string PlainText { get { return "Expression was evaluated without the output value."; } }
        public override string ToString() {
            return PlainText;
        }
    }
}
