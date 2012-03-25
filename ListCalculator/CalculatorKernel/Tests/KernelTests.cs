using CalculatorKernel.Kernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            TestCalculation("2", "tag",
            expectedResult: (int)2,
            expectedString: "2");
        }
        [Test]
        public void FunctionTest() {
            TestCalculation(@"
def foo():
    return 2

foo()", "tag",
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

factorial(5)", "tag",
            expectedResult: (int)120,
            expectedString: "120");
        }
        [Test]
        public void IllegalExpressionTest() {
            TestCalculation("1 + / * 2", "tag",
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
            TestCalculation("a = 1", "tag",
            expectedResult: (CalculationResultNull)null,
            expectedString: new CalculationResultNull(null).PlainText,
            skipValueCheck: true);
        }
        [Test]
        public void ParallelCalculationTest() {
            int sum = 0;
            const int count = 50;
            ManualResetEvent[] waitHandles = new ManualResetEvent[count];
            object lck = new object();
            EventHandler<CalculationCompletedEventArgs> handler = (s, e) => {
                int x = ((ICalculationResult<int>)e.Result).Value;
                Assert.That(x, Is.EqualTo(e.Result.Tag));
                Interlocked.Add(ref sum, x);
                lock(lck) {
                    waitHandles[x].Set();
                }
            };
            try {
                Kernel.CalculationCompleted += handler;
                for(int i = 0; i < count; i++) {
                    waitHandles[i] = new ManualResetEvent(false);
                    Kernel.StartCalculating(@"
import time
time.sleep(0.2)
" + i.ToString(), i);
                }
                WaitHandle.WaitAll(waitHandles);
                Assert.That(sum, Is.EqualTo(1225));
            } finally {
                Kernel.CalculationCompleted -= handler;
            }
        }

        void TestCalculation<T>(string expression, object tag, T expectedResult, string expectedString, bool skipValueCheck = false) {
            ICalculationResult result = Kernel.Calculate(expression, tag);
            Assert.That(result, Is.Not.Null);
            ICalculationResult<T> castedResult = result as ICalculationResult<T>;
            if(castedResult != null)
                Assert.That(castedResult.Type, Is.EqualTo(typeof(T)));
            else
                Assert.That(result.Type, Is.EqualTo(null));
            if(!skipValueCheck) {
                Assert.That(castedResult, Is.Not.Null, "Returned value is not of type " + typeof(T));
                Assert.That(castedResult.Value, Is.EqualTo(expectedResult));
            }
            Assert.That(result.PlainText, Is.EqualTo(expectedString));
            Assert.That(result.Tag, Is.EqualTo(tag));
            Assert.That(result.ToString(), Is.EqualTo(result.PlainText));
        }
    }
}
