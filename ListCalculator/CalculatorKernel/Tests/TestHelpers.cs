using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CalculatorKernel.Kernel;
using NUnit.Framework;

namespace CalculatorKernel.Tests {
    static class KernelExtensions {
        public static ICalculationResult Calculate(this Kernel.Kernel kernel, string expression) {
            ICalculationResult result = null;
            ManualResetEvent evt = new ManualResetEvent(false);
            kernel.CalculationCompleted += (s, e) => { result = e.Result; evt.Set(); };
            kernel.StartCalculating(expression);
            evt.WaitOne();
            return result;
        }
        public static ICalculationResult<T> Calculate<T>(this Kernel.Kernel kernel, string expression) {
            ICalculationResult result = kernel.Calculate(expression);
            ICalculationResult<T> castedResult = result as ICalculationResult<T>;
            return castedResult;
        }
    }
}
