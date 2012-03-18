using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculatorKernel.Kernel;
using System.Threading;

namespace ListCalculatorConsole {
    class Program {
        static Kernel kernel;

        static void Main(string[] args) {
            kernel = new Kernel();
            AutoResetEvent evt = new AutoResetEvent(false);
            kernel.CalculationCompleted += (s, e) => {
                if(!(e.Result is CalculationResultNull))
                    Console.WriteLine(e.Result.PlainText);
                evt.Set();
            };
            do {
                Console.Write("> ");
                string expression = Console.ReadLine();
                if(expression == "exit") break;
                kernel.StartCalculating(expression);
                evt.WaitOne();
            } while(true);
        }
    }
}
