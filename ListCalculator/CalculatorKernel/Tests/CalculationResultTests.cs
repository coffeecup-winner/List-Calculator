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
            var result = Kernel.Calculate<Python.List>("[1, 2]");
            Assert.That(result.PlainText, Is.EqualTo("[1, 2]"));
        }
        [Test]
        public void DictFormatTest() {
            var result = Kernel.Calculate<Python.PythonDictionary>("{123:'x','x':123}");
            Assert.That(result.PlainText, Is.EqualTo("{'x': 123, 123: 'x'}"));
        }
        [Test]
        public void StringFormatTest() {
            var result = Kernel.Calculate<string>("'xyz'");
            Assert.That(result.PlainText, Is.EqualTo("'xyz'"));
        }
        [Test]
        public void NestedTypesTest() {
            var result = Kernel.Calculate<Python.List>("[1,{2:3,'4':[5,6,{7:'8'}]}]");
            Assert.That(result.PlainText, Is.EqualTo("[1, {2: 3, '4': [5, 6, {7: '8'}]}]"));
        }
    }
}
