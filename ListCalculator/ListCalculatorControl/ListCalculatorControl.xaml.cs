using System.Windows.Controls;
using System.Windows.Input;
using CalculatorKernel.Kernel;
using System.Collections.Generic;
using System.Windows;
using System;
using CalculatorKernel.Library;
using IronPython.Runtime;
using PythonList = IronPython.Runtime.List;
using ListCalculatorControl.Templates;

namespace ListCalculatorControl {
    /// <summary>
    /// Interaction logic for ListCalculatorControl.xaml
    /// </summary>
    public partial class ListCalculatorControl : ListBox {
        readonly TypedDataTemplateDictionary outputTemplateDictionary;

        public ListCalculatorControl() {
            InitializeComponent();
            Items.Clear();
            ViewModel = new ListCalculatorViewModel();
            outputTemplateDictionary = new TypedDataTemplateDictionary();
            FillOutputAreaTemplateDictionary();
            ((TypeToDataTemplateConverter)Resources["typeToDataTemplateConverter"]).TemplateDictionary = OutputTemplateDictionary;
        }
        public ListCalculatorViewModel ViewModel {
            get { return DataContext as ListCalculatorViewModel; }
            set { DataContext = value; }
        }
        public TypedDataTemplateDictionary OutputTemplateDictionary {
            get { return outputTemplateDictionary; }
        }
        void FillOutputAreaTemplateDictionary() {
            OutputTemplateDictionary.AddTemplateFor<object>("Plain Text", TemplateRepository.PlainTextTemplate); //fallback template
            OutputTemplateDictionary.AddTemplateFor<Exception>("Calculation Error", TemplateRepository.CalculationErrorTemplate);
            #region Chart templates
            OutputTemplateDictionary.AddTemplateFor<AreaChartSeries>("Area Chart", TemplateRepository.AreaChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<BarChartSeries>("Bar Chart", TemplateRepository.BarChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<BubbleChartSeries>("Bubble Chart", TemplateRepository.BubbleChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<ColumnChartSeries>("Column Chart", TemplateRepository.ColumnChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<LineChartSeries>("Line Chart", TemplateRepository.LineChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<PieChartSeries>("Pie Chart", TemplateRepository.PieChartTemplate);
            OutputTemplateDictionary.AddTemplateFor<ScatterChartSeries>("Scatter Chart", TemplateRepository.ScatterChartTemplate);
            #endregion
            OutputTemplateDictionary.AddTemplateFor<PythonList>("List/Dict Tree View", TemplateRepository.ListDictTreeViewTemplate);
            OutputTemplateDictionary.AddTemplateFor<PythonDictionary>("List/Dict Tree View", TemplateRepository.ListDictTreeViewTemplate);
        }
        void TextBox_KeyDown(object sender, KeyEventArgs e) {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
        void ContentControl_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            ContentControl control = (ContentControl)sender;
            ActiveBlock block = (ActiveBlock)control.DataContext;
            List<DataTemplateInfo> templates = OutputTemplateDictionary.GetTemplatesFor(block.Output.Type);
            ContextMenu menu = CreateOutputControlMenu(control, templates);
            control.ContextMenu = menu;
            menu.IsOpen = true;
        }
        ContextMenu CreateOutputControlMenu(ContentControl control, List<DataTemplateInfo> templates) {
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem { Header = "Select Template" };
            menu.Items.Add(item);
            foreach(DataTemplateInfo info in templates) {
                bool isCurrent = info.DataTemplate == control.ContentTemplate;
                MenuItem subItem = new MenuItem {
                    Header = info.Name.ToString() + (isCurrent ? " (current)" : ""),
                    Tag = info.DataTemplate,
                    IsEnabled = !isCurrent
                };
                subItem.Click += (s, ee) => control.ContentTemplate = (DataTemplate)((MenuItem)s).Tag;
                item.Items.Add(subItem);
            }
            return menu;
        }
    }
}