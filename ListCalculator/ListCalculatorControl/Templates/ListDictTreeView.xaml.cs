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
using PythonList = IronPython.Runtime.List;
using IronPython.Runtime;
using System.Collections;

namespace ListCalculatorControl.Templates {
    /// <summary>
    /// Interaction logic for ListDictTreeView.xaml
    /// </summary>
    public partial class ListDictTreeView : TreeView {
        public static readonly DependencyProperty HierarchicalDataSourceProperty;
        static ListDictTreeView() {
            Type ownerType = typeof(ListDictTreeView);
            HierarchicalDataSourceProperty = DependencyProperty.Register("HierarchicalDataSource", typeof(ICollection), ownerType,
                new PropertyMetadata() { PropertyChangedCallback = OnDataSourceChanged });
        }
        static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ListDictTreeView treeView = (ListDictTreeView)d;
            treeView.OnDataSourceChanged();
        }

        public ListDictTreeView() {
            InitializeComponent();
        }
        public ICollection HierarchicalDataSource {
            get { return (ICollection)GetValue(HierarchicalDataSourceProperty); }
            set { SetValue(HierarchicalDataSourceProperty, value); }
        }
        public void OnDataSourceChanged() {
            Items.Clear();
            AddItems(this, HierarchicalDataSource);
        }
        void AddItems(ItemsControl itemsControl, object value) {
            PythonList list = value as PythonList;
            TreeViewItem item = new TreeViewItem();
            itemsControl.Items.Add(item);
            if(list != null) {
                item.Header = "list:" + list.Count;
                foreach(var obj in list)
                    AddItems(item, obj);
                return;
            }
            PythonDictionary dict = value as PythonDictionary;
            if(dict != null) {
                item.Header = "dict:" + dict.Count;
                foreach(var pair in dict) {
                    TreeViewItem keyItem = new TreeViewItem { Header = pair.Key };
                    item.Items.Add(keyItem);
                    AddItems(keyItem, pair.Value);
                }
                return;
            }
            item.Header = value;
        }
    }
}
