using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculatorKernel.Kernel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Threading;

namespace ListCalculatorControl {
    public class ListCalculatorViewModel : ObservableCollection<Block> {
        Kernel kernel = new Kernel();

        public ListCalculatorViewModel() {
            //test stuff
            Add(new ActiveBlock() { Input = "2 + 4", Output = new CalculationResult<int>(6) });
            Add(new ActiveBlock() { Input = "'x' + 'y'", Output = new CalculationResult<string>("xy") });
        }
        protected Kernel Kernel { get { return kernel; } }
    }

    public class Block : FrameworkElement {

    }

    public class ActiveBlock : Block {
        static DependencyProperty InputProperty = DependencyProperty.Register("Input", typeof(string), typeof(ActiveBlock));
        static DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(ICalculationResult), typeof(ActiveBlock));
        public string Input {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public ICalculationResult Output {
            get { return (ICalculationResult)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }
    }
}
