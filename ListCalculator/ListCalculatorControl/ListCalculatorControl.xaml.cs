using System.Windows.Controls;
using System.Windows.Input;

namespace ListCalculatorControl {
    /// <summary>
    /// Interaction logic for ListCalculatorControl.xaml
    /// </summary>
    public partial class ListCalculatorControl : ListBox {
        public ListCalculatorViewModel ViewModel {
            get { return DataContext as ListCalculatorViewModel; }
            set { DataContext = value; }
        }

        public ListCalculatorControl() {
            InitializeComponent();
            Items.Clear();
            ViewModel = new ListCalculatorViewModel();
        }

        void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key != Key.Enter) return;
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            ((ActiveBlock)(((TextBox)sender).DataContext)).Calculate();
        }
    }
}