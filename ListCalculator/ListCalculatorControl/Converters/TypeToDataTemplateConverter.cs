using System;
using System.Windows.Data;
using CalculatorKernel.Kernel;
using ListCalculatorControl.Templates;

namespace ListCalculatorControl {
    class TypeToDataTemplateConverter : IValueConverter {
        readonly TypedDataTemplateDictionary typedOutputAreaTemplateDictionary;

        public TypeToDataTemplateConverter() {
            typedOutputAreaTemplateDictionary = new TypedDataTemplateDictionary();
            FillOutputAreaTemplateDictionary();
        }

        void FillOutputAreaTemplateDictionary() {
            typedOutputAreaTemplateDictionary.DefaultOutputAreaTemplate = TemplateRepository.PlaintTextTemplate;
            typedOutputAreaTemplateDictionary[typeof(string)] = TemplateRepository.PlaintTextTemplate;
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null)
                return null;

            ICalculationResult result = (ICalculationResult)value;

            if(result.Type != null && typedOutputAreaTemplateDictionary.ContainsKey(result.Type))
                return typedOutputAreaTemplateDictionary[result.Type];
            else
                return typedOutputAreaTemplateDictionary.DefaultOutputAreaTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}