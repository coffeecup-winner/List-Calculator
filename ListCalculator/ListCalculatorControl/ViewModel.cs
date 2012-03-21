﻿using System;
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

namespace ListCalculatorControl {
    public class ListCalculatorViewModel : ObservableCollection<Block> {
        Kernel kernel = new Kernel();
        int idGenerator = 0;

        public ListCalculatorViewModel() {
            //test stuff
            Kernel.CalculationCompleted += (s, e) => GetBlockByID<ActiveBlock>((int)e.Result.Tag).Output = e.Result;
            Add(new ActiveBlock(this, GetNextID()) { Input = "2 + 4", Output = new CalculationResult<int>(6) });
            Add(new ActiveBlock(this, GetNextID()) { Input = "'x' + 'y'", Output = new CalculationResult<string>("xy") });
        }
        protected Kernel Kernel { get { return kernel; } }
        int GetNextID() { return idGenerator++; }
        TBlock GetBlockByID<TBlock>(int id) where TBlock : Block {
            return this.OfType<TBlock>().Single(b => b.ID == id);
        }
        public void Calculate(ActiveBlock block) {
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
        public static readonly DependencyProperty InputProperty = DependencyProperty.Register("Input", typeof(string), typeof(ActiveBlock));
        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(ICalculationResult), typeof(ActiveBlock));

        public ActiveBlock(ListCalculatorViewModel viewModel, int id) : base(viewModel, id) { }
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
        public void Calculate() {
            ViewModel.Calculate(this);
        }
    }
}
