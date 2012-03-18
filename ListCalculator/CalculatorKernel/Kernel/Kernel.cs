using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorKernel.Kernel {
    public interface IKernel {
        void StartCalculating(string expression);
        event EventHandler<CalculationCompletedEventArgs> CalculationCompleted;
    }

    public class Kernel : IKernel {
        IronPythonWrapper pythonWrapper = new IronPythonWrapper();
        public event EventHandler<CalculationCompletedEventArgs> CalculationCompleted = delegate { };

        public void StartCalculating(string expression) {
            new Task(() => {
                try {
                    var result = this.pythonWrapper.Execute(expression);
                    if(result == null)
                        result = NullResult.Instance;
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(result)));
                } catch(Exception e) {
                    CalculationException exception = new CalculationException(e);
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(exception)));
                }
            }).Start();
        }
    }

    public class CalculationException : Exception {
        public CalculationException(Exception innerException) : base(string.Empty, innerException) { }
        public override string ToString() {
            return InnerException.Message;
        }
    }
}
