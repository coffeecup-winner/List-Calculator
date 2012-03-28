using System.Windows.Controls;
using System.Windows.Input;
using CalculatorKernel.Kernel;
using System.Collections.Generic;
using System.Windows;

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
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void ContentControl_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            TypeToDataTemplateConverter converter = (TypeToDataTemplateConverter)Resources["typeToDataTemplateConverter"];
            ContentControl control = (ContentControl)sender;
            ActiveBlock block = (ActiveBlock)control.DataContext;
            List<DataTemplate> templates = converter.GetTemplatesFor(block.Output.Type);
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem { Header = "Select Template" };
            menu.Items.Add(item);
            control.ContextMenu = menu;
            foreach(DataTemplate template in templates) {
                MenuItem subItem = new MenuItem {
                    Header = template.ToString(),
                    Tag = template
                };
                subItem.Click += (s, ee) => control.ContentTemplate = (DataTemplate)((MenuItem)s).Tag;
                item.Items.Add(subItem);
            }
            menu.IsOpen = true;
        }
    }
}