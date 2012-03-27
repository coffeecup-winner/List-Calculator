using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using CalculatorKernel.Kernel;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;

namespace CalculatorKernel.Tests {

    [TestFixture]
    public class IronPythonIntegrationTests {
        IronPythonWrapper wrapper;

        protected IronPythonWrapper Wrapper { get { return wrapper; } }
        [TestFixtureSetUp]
        public void FixtureSetUp() {
            wrapper = new IronPythonWrapper();
        }
        [SetUp]
        public void SetUp() {
            wrapper.Reset();
        }
        [Test]
        public void SimpleTest() {
            dynamic x = Wrapper.Execute("1");
            Assert.That(x, Is.EqualTo(1));
        }
        [Test]
        public void FunctionTest() {
            Wrapper.Execute(@"
def foo():
    return 2
");
            Assert.That(Wrapper.Execute("foo()"), Is.EqualTo(2));
        }
        [Test]
        public void PreserveScopeTest() {
            Wrapper.Execute("x = 2");
            Assert.That(Wrapper.Execute("x"), Is.EqualTo(2));
            Wrapper.Execute("x = 3");
            Assert.That(Wrapper.Execute("x"), Is.EqualTo(3));
        }
        [Test]
        public void ResetScopeTest() {
            Wrapper.Execute("x = 2");
            Assert.That(Wrapper.Execute("x"), Is.EqualTo(2));
            Wrapper.Reset();
            Assert.That(() => Wrapper.Execute("x"), Throws.TypeOf<UnboundNameException>());
        }
        [Test]
        public void TypedExecuteTest() {
            Assert.That(Wrapper.Execute<int>("2"), Is.EqualTo(2));
            Assert.That(() => Wrapper.Execute<string>("2"), Throws.TypeOf<TypeErrorException>());
        }
        [Test]
        public void SetVariableTest() {
            Assert.That(() => Wrapper.Execute("x"), Throws.TypeOf<UnboundNameException>());
            Wrapper.SetVariable("x", 2);
            Assert.That(Wrapper.Execute<int>("x"), Is.EqualTo(2));
            Wrapper.Reset();
            Assert.That(() => Wrapper.Execute("x"), Throws.TypeOf<UnboundNameException>());
        }
    }
}