using CalculatorKernel.Kernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            TestCalculation("2",
            expectedResult: (int)2,
            expectedString: "2");
        }
        [Test]
        public void FunctionTest() {
            TestCalculation(@"
def foo():
    return 2

foo()",
            expectedResult: (int)2,
            expectedString: "2");
        }
        [Test]
        public void FactorialTest() {
            TestCalculation(@"
def factorial(n):
    if(n <= 0):
        return 1
    return n * factorial(n - 1)

factorial(5)",
            expectedResult: (int)120,
            expectedString: "120");
        }
        [Test]
        public void IllegalExpressionTest() {
            TestCalculation("1 + / * 2",
            expectedResult: (CalculationException)null,
            expectedString: "unexpected token '/'",
            skipValueCheck: true);
        }
        [Test]
        public void OperabilityAfterExceptionTest() {
            IllegalExpressionTest();
            FactorialTest();
        }
        void TestCalculation<T>(string expression, T expectedResult, string expectedString, bool skipValueCheck = false) {
            ManualResetEvent evt = new ManualResetEvent(false);
            T actualResult = default(T);
            string actualString = null;
            EventHandler<CalculationCompletedEventArgs> action = (s, e) => {
                ICalculationResult<T> result = e.Result as ICalculationResult<T>;
                if(result != null) {
                    actualResult = result.Value;
                    actualString = result.PlainText;
                }
                evt.Set();
            };
            kernel.CalculationCompleted += action;
            kernel.StartCalculating(expression);
            evt.WaitOne();
            Assert.That(actualString, Is.Not.Empty, "Returned value is not of type " + typeof(T));
            if(!skipValueCheck)
                Assert.That(actualResult, Is.EqualTo(expectedResult));
            Assert.That(actualString, Is.EqualTo(expectedString));
        }
    }
}
