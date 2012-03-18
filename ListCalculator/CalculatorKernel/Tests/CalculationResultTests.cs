using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CalculatorKernel.Kernel;
using Python = IronPython.Runtime;

namespace CalculatorKernel.Tests {
    [TestFixture]
    public class CalculationResultTests {
        Kernel.Kernel kernel;

        protected Kernel.Kernel Kernel { get { return kernel; } }
        [TestFixtureSetUp]
        public void FixtureSetUp() {
            kernel = new Kernel.Kernel();
        }
        [Test]
        public void ListFormatTest() {
            var result = kernel.Calculate<Python.List>("[1, 2]");
            Assert.That(result.PlainText, Is.EqualTo("[1, 2]"));
        }
        [Test, Ignore("To be implemented")]
        public void DictFormatTest() {
            var result = kernel.Calculate<Python.PythonDictionary>("{123:'x','x':123}");
            Assert.That(result.PlainText, Is.EqualTo("{123: 'x', 'x': 123}"));
        }
    }
}
