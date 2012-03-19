using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Python = IronPython.Runtime;
using System.Diagnostics.Contracts;

namespace CalculatorKernel.Kernel {
    public static class CalculationResult {
        //HACK: this method get called if value == null
        public static CalculationResultNull Create(object value) {
            Contract.Assert(value == null);
            return CalculationResultNull.Instance;
        }
        public static ICalculationResult<T> Create<T>(T value) {
            return new CalculationResult<T>(value);
        }
    }

    public class CalculationResultNull : ICalculationResult {
        static readonly CalculationResultNull instance = new CalculationResultNull();

        private CalculationResultNull() { }
        public static CalculationResultNull Instance { get { return instance; } }
        public string PlainText { get { return "Expression was evaluated without the output value."; } }
        public override string ToString() {
            return PlainText;
        }
    }
}
