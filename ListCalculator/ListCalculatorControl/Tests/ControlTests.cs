using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace ListCalculatorControl.Tests {
    [TestFixture]
    public class CommandTests {
        private int testInt;
        private ICommand command;
        [SetUp]
        public void SetUp() {
            this.testInt = 0;
            this.command = null;
        }
        [Test]
        public void DelegateCommandExecuteTest() {
            ICommand command = new DelegateCommand(d => this.testInt = (int)d);
            Assert.That(command.CanExecute(null));
            command.Execute(1);
            Assert.That(testInt, Is.EqualTo(1));
        }
        [Test]
        public void DelegateCommandCanExecuteTest() {
            ICommand command = new DelegateCommand(d => (bool)d, null);
            Assert.That(command.CanExecute(false), Is.False);
            Assert.That(command.CanExecute(true), Is.True);
        }
        [Test]
        public void RaiseCanExecuteChangedTest() {
            command = new DelegateCommand(d => TestEventHandler((DelegateCommand)command));
            int i = 0;
            command.CanExecuteChanged += new EventHandler((d, e) => i = 10);
            command.Execute(null);
            Assert.That(i, Is.EqualTo(10));
        }
        private void TestEventHandler(DelegateCommand command) {
            command.RaiseCanExecuteChanged();
        }
    }
    [TestFixture]
    public class ListCalculatorControlTests {
        [Test, Category("TODO")]
        public void UpdateCommandTest() {
            Window wnd = new Window();
            ListCalculatorControl control = new ListCalculatorControl();
            wnd.Content = control;
            wnd.Show();
            TextBox expressionBox = LayoutHelper.FindChild<TextBox>(control, "ExpressionBox");
            expressionBox.Text = "2 + 2";
            expressionBox.Focus();
            control.UpdateLayout();
            expressionBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            //System.Windows.Forms.SendKeys.SendWait("+~");
            FrameworkElement output = LayoutHelper.FindChild<FrameworkElement>(control, "OutputControl");
            Assert.That(((ActiveBlock)output.DataContext).Output.PlainText, Is.EqualTo("4"));
        }
    }
}
