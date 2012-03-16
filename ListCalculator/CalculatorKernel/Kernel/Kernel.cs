using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace CalculatorKernel.Kernel {

    public interface IKernel {
        void StartCalculating(string expression);
        event EventHandler<CalculationCompletedEventArgs> CalculationCompleted;
    }

    public interface ICalculationResult {
        string PlainText { get; }
    }

    public class Kernel : IKernel {
        IronPythonWrapper pythonWrapper = new IronPythonWrapper();
        public event EventHandler<CalculationCompletedEventArgs> CalculationCompleted = delegate { };

        public void StartCalculating(string expression) {
            new Task(() => {
                var result = this.pythonWrapper.Execute(expression);
                CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(result)));
            }).Start();
        }
    }

    public static class CalculationResult {
        public static CalculationResult<T> Create<T>(T value) {
            return new CalculationResult<T>(value);
        }
    }
    public class CalculationResult<T> : ICalculationResult {
        readonly T value;
        string plainText = null;

        public CalculationResult(T value) {
            this.value = value;
        }
        public T Value { get { return value; } }
        public string PlainText { get { return plainText ?? (plainText = Value.ToString()); } }
    }

    public class CalculationCompletedEventArgs : System.EventArgs {
        readonly ICalculationResult result;

        public CalculationCompletedEventArgs(ICalculationResult result) {
            this.result = result;
        }
        public ICalculationResult Result { get { return result; } }
    }
}
