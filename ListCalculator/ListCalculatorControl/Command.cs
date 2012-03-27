using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ListCalculatorControl {
    public class DelegateCommand : ICommand {
        #region fields
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;
        #endregion
        public event EventHandler CanExecuteChanged;
        #region ctor
        public DelegateCommand(Action<object> execute) {
            this.canExecute = d => true;
            this.execute = execute;
        }
        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute) {
            this.canExecute = canExecute;
            this.execute = execute;
        }
        #endregion
        public bool CanExecute(object parameter) {
            return this.canExecute(parameter);
        }
        public void Execute(object parameter) {
            this.execute(parameter);
        }
        public void RaiseCanExecuteChanged() {
            if(CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
