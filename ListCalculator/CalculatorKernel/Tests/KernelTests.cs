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

        protected Kernel.Kernel Kernel { get { return kernel; } }
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
        [Test]
        public void ExpressionWithoutResultTest() {
            TestCalculation("a = 1",
            expectedResult: (CalculationResultNull)null,
            expectedString: CalculationResultNull.Instance.PlainText,
            skipValueCheck: true);
        }

        void TestCalculation<T>(string expression, T expectedResult, string expectedString, bool skipValueCheck = false) {
            ICalculationResult result = Kernel.Calculate(expression);
            Assert.That(result, Is.Not.Null);
            if(!skipValueCheck) {
                ICalculationResult<T> castedResult = result as ICalculationResult<T>;
                Assert.That(castedResult, Is.Not.Null, "Returned value is not of type " + typeof(T));
                Assert.That(castedResult.Value, Is.EqualTo(expectedResult));
            }
            Assert.That(result.PlainText, Is.EqualTo(expectedString));
            Assert.That(result.ToString(), Is.EqualTo(result.PlainText));
        }
    }
}
