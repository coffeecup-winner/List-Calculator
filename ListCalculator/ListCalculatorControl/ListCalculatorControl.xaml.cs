using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListCalculatorControl {
    /// <summary>
    /// Interaction logic for ListCalculatorControl.xaml
    /// </summary>
    public partial class ListCalculatorControl : ListBox {
        public ListCalculatorControl() {
            InitializeComponent();
            Items.Clear();
            DataContext = new ListCalculatorViewModel();
        }

        void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key != Key.Enter) return;
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            ((ActiveBlock)(((TextBox)sender).DataContext)).Calculate();
        }
    }
}