using CalculatorKernel.Kernel;
using ListCalculatorControl.Templates;
using System;
using System.Globalization;
using System.Windows.Data;
using CalculatorKernel.Library;

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
            templateDictionary.AddTemplateFor<LineChartSeries>(TemplateRepository.LineChartTemplate);
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