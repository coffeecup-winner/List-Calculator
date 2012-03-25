using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace CalculatorKernel.Kernel {
    public interface IKernel {
        void StartCalculating(string expression, object tag);
        event EventHandler<CalculationCompletedEventArgs> CalculationCompleted;
    }

    public class Kernel : IKernel {
        IronPythonWrapper pythonWrapper = new IronPythonWrapper();
        public event EventHandler<CalculationCompletedEventArgs> CalculationCompleted = delegate { };

        public Kernel() {
            pythonWrapper.Execute(@"
import clr
import System
clr.AddReferenceToFileAndPath('" + Assembly.GetExecutingAssembly().Location.Replace(@"\", @"\\") + @"')
import CalculatorKernel.Library
from CalculatorKernel.Library.Charting import *");
        }

        public void StartCalculating(string expression, object tag = null) {
            new Task(() => {
                try {
                    var result = this.pythonWrapper.Execute(expression);
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(result, tag)));
                } catch(Exception e) {
                    CalculationException exception = new CalculationException(e);
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(exception, tag)));
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
