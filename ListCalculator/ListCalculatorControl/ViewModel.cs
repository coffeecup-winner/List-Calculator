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
using System.Threading;
using System.ComponentModel;
using System.Windows.Input;

namespace ListCalculatorControl {
    public class ListCalculatorViewModel : DependencyObject {
        Kernel kernel = new Kernel();
        int idGenerator = 0;
        readonly ObservableCollection<Block> blocks = new ObservableCollection<Block>();
        ActiveBlock lastActiveBlock = null;

        public ListCalculatorViewModel() {
            //test stuff
            Kernel.CalculationCompleted += (s, e) => GetBlockByID<ActiveBlock>((int)e.Result.Tag).Output = e.Result;
            if(DesignerProperties.GetIsInDesignMode(this)) {
                Blocks.Add(new ActiveBlock(this, GetNextID()) { Input = "2 + 4", Output = new CalculationResult<int>(6, 0) });
                Blocks.Add(new ActiveBlock(this, GetNextID()) { Input = "'x' + 'y'", Output = new CalculationResult<string>("xy", 1) });
            }
            AddActiveBlock();
        }
        protected Kernel Kernel { get { return kernel; } }
        public ObservableCollection<Block> Blocks { get { return blocks; } }
        int GetNextID() { return idGenerator++; }
        TBlock GetBlockByID<TBlock>(int id) where TBlock : Block {
            return Blocks.OfType<TBlock>().Single(b => b.ID == id);
        }
        protected ActiveBlock AddActiveBlock() {
            ActiveBlock block = new ActiveBlock(this, GetNextID());
            Blocks.Add(block);
            lastActiveBlock = block;
            return block;
        }
        public void Calculate(ActiveBlock block) {
            if(lastActiveBlock == block)
                AddActiveBlock();
            Kernel.StartCalculating(block.Input, block.ID);
        }
    }

    public class Block : FrameworkElement {
        readonly ListCalculatorViewModel viewModel;
        readonly int id;

        public Block(ListCalculatorViewModel viewModel, int id) {
            this.viewModel = viewModel;
            this.id = id;
        }
        public int ID { get { return id; } }
        protected ListCalculatorViewModel ViewModel { get { return viewModel; } }
    }

    public class ActiveBlock : Block {
        public static readonly DependencyProperty InputProperty;
        public static readonly DependencyProperty OutputProperty;
        private static readonly DependencyPropertyKey UpdateCommandPropertyKey;
        public static readonly DependencyProperty UpdateCommandProperty;
        static ActiveBlock() {
            Type ownerType = typeof(ActiveBlock);
            InputProperty = DependencyProperty.Register("Input", typeof(string), ownerType);
            OutputProperty = DependencyProperty.Register("Output", typeof(ICalculationResult), ownerType);
            UpdateCommandPropertyKey = DependencyProperty.RegisterReadOnly("UpdateCommand", typeof(ICommand), ownerType, new PropertyMetadata());
            UpdateCommandProperty = UpdateCommandPropertyKey.DependencyProperty;
        }

        public ActiveBlock(ListCalculatorViewModel viewModel, int id)
            : base(viewModel, id) {
            UpdateCommand = new DelegateCommand(d => this.Calculate());
        }
        public string Input {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public ICalculationResult Output {
            get {
                return (ICalculationResult)Dispatcher.Invoke(DispatcherPriority.Background,
                    new DispatcherOperationCallback(target => GetValue(OutputProperty)));
            }
            set {
                Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new SendOrPostCallback(target => SetValue(OutputProperty, target)), value);
            }
        }
        public ICommand UpdateCommand {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            private set { this.SetValue(UpdateCommandPropertyKey, value); }
        }
        public void Calculate() {
            ViewModel.Calculate(this);
        }
    }
}
