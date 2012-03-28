using CalculatorKernel.Kernel;
using ListCalculatorControl.Templates;
using System;
using System.Globalization;
using System.Windows.Data;
using CalculatorKernel.Library;
using PythonList = IronPython.Runtime.List;
using IronPython.Runtime;
using System.Windows;
using System.Collections.Generic;

namespace ListCalculatorControl {
    class TypeToDataTemplateConverter : IValueConverter {
        readonly TypedDataTemplateDictionary templateDictionary;

        public TypeToDataTemplateConverter() {
            templateDictionary = new TypedDataTemplateDictionary();
            FillOutputAreaTemplateDictionary();
        }
        void FillOutputAreaTemplateDictionary() {
            templateDictionary.AddTemplateFor<object>(TemplateRepository.PlainTextTemplate); //fallback template
            templateDictionary.AddTemplateFor<Exception>(TemplateRepository.CalculationErrorTemplate);
            #region Chart templates
            templateDictionary.AddTemplateFor<AreaChartSeries>(TemplateRepository.AreaChartTemplate);
            templateDictionary.AddTemplateFor<BarChartSeries>(TemplateRepository.BarChartTemplate);
            templateDictionary.AddTemplateFor<BubbleChartSeries>(TemplateRepository.BubbleChartTemplate);
            templateDictionary.AddTemplateFor<ColumnChartSeries>(TemplateRepository.ColumnChartTemplate);
            templateDictionary.AddTemplateFor<LineChartSeries>(TemplateRepository.LineChartTemplate);
            templateDictionary.AddTemplateFor<PieChartSeries>(TemplateRepository.PieChartTemplate);
            templateDictionary.AddTemplateFor<ScatterChartSeries>(TemplateRepository.ScatterChartTemplate);
            #endregion
            templateDictionary.AddTemplateFor<PythonList>(TemplateRepository.ListDictTreeViewTemplate);
            templateDictionary.AddTemplateFor<PythonDictionary>(TemplateRepository.ListDictTreeViewTemplate);
        }
        public List<DataTemplate> GetTemplatesFor(Type type) {
            return templateDictionary.GetTemplatesFor(type);
        }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null)
                return null;

            ICalculationResult result = (ICalculationResult)value;
            return result.Type != null ? templateDictionary.GetBestTemplateFor(result.Type) : templateDictionary.GetBestTemplateFor<object>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }
        #endregion
    }
}