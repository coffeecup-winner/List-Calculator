using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace CalculatorKernel.Kernel {
    public interface IKernel {
        void StartCalculating(string expression, object tag);
        event EventHandler<CalculationCompletedEventArgs> CalculationCompleted;
        void Reset();
    }

    public class Kernel : IKernel {
        IronPythonWrapper pythonWrapper = new IronPythonWrapper();
        public event EventHandler<CalculationCompletedEventArgs> CalculationCompleted = delegate { };
        int nextSequenceNumber = 0;

        public Kernel() {
            pythonWrapper.Execute(@"
import clr
import System
clr.AddReferenceToFileAndPath('" + Assembly.GetExecutingAssembly().Location.Replace(@"\", @"\\") + @"')
import CalculatorKernel.Library
from CalculatorKernel.Library.Charting import *");
        }
        int GetNextSequenceNumber() {
            return this.nextSequenceNumber++;
        }
        public void StartCalculating(string expression, object tag = null) {
            new Task(() => {
                try {
                    var result = this.pythonWrapper.Execute(expression);
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(result, GetNextSequenceNumber(), tag)));
                } catch(Exception e) {
                    CalculationException exception = new CalculationException(e);
                    CalculationCompleted(this, new CalculationCompletedEventArgs(CalculationResult.Create(exception, GetNextSequenceNumber(), tag)));
                }
            }).Start();
        }
        public void Reset() {
            this.nextSequenceNumber = 0;
        }
    }

    public class CalculationException : Exception {
        public CalculationException(Exception innerException) : base(string.Empty, innerException) { }
        public override string ToString() {
            return InnerException.Message;
        }
    }
}
