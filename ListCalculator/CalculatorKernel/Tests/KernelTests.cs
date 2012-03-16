using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;

namespace CalculatorKernel.Tests {
    [TestFixture]
    public class KernelTests {
        Kernel.Kernel kernel;

        Kernel.Kernel Kernel { get { return kernel; } }
        [TestFixtureSetUp]
        public void FixtureSetUp() {
            kernel = new Kernel.Kernel();
        }
        [Test]
        public void SimpleTest() {
            TestCalculationPlainText("2", "2");
        }
        [Test]
        public void FunctionTest() {
            TestCalculationPlainText(@"
def foo():
    return 2
foo()
            ", "2");
        }
        [Test]
        public void FactorialTest() {
            TestCalculationPlainText(@"
def factorial(n):
    if(n <= 0):
        return 1
    return n * factorial(n - 1)
factorial(5)
            ", "120");
        }
        void TestCalculationPlainText(string expression, string expectedResult) {
            ManualResetEvent evt = new ManualResetEvent(false);
            string actualResult = string.Empty;
            EventHandler<Kernel.CalculationCompletedEventArgs> action = (s, e) => {
                actualResult = e.Result.PlainText;
                evt.Set();
            };
            kernel.CalculationCompleted += action;
            kernel.StartCalculating(expression);
            evt.WaitOne();
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}
