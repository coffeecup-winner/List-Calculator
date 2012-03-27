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
        List<ICalculationResult> calculationHistory = new List<ICalculationResult>();
        int nextSequenceNumber = 0;

        public Kernel() {
            pythonWrapper.SetVariable("__kernel", this);
            pythonWrapper.Execute(@"
import clr
import System
clr.AddReferenceToFileAndPath('" + Assembly.GetExecutingAssembly().Location.Replace(@"\", @"\\") + @"')
import CalculatorKernel.Library
from CalculatorKernel.Library.Charting import *
__out = __kernel.GetHistoryAt");
        }
        int GetNextSequenceNumber() {
            return this.nextSequenceNumber++;
        }
        public void StartCalculating(string expression, object tag = null) {
            new Task(() => {
                try {
                    var result = this.pythonWrapper.Execute(expression);
                    RaiseCalculationCompleted(result, tag);
                } catch(Exception e) {
                    RaiseCalculationCompleted(new CalculationException(e), tag);
                }
            }).Start();
        }
        public dynamic GetHistoryAt(int index) {
            dynamic result = calculationHistory[index];
            return result.Value;
        }
        void RaiseCalculationCompleted(dynamic result, object tag) {
            dynamic calculationResult = CalculationResult.Create(result, GetNextSequenceNumber(), tag);
            OnBeforeCalculationCompleted(calculationResult);
            CalculationCompleted(this, new CalculationCompletedEventArgs(calculationResult));
        }
        void OnBeforeCalculationCompleted(dynamic result) {
            calculationHistory.Add(result);
            this.pythonWrapper.SetVariable("__last", result is CalculationResultNull ? null : result.Value);
        }
        public void Reset() {
            this.nextSequenceNumber = 0;
            this.calculationHistory.Clear();
        }
    }

    public class CalculationException : Exception {
        public CalculationException(Exception innerException) : base(string.Empty, innerException) { }
        public override string ToString() {
            return InnerException.Message;
        }
    }
}
