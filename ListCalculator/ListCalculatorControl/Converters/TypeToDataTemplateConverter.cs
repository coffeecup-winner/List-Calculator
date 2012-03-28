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
        TypedDataTemplateDictionary templateDictionary;

        public TypedDataTemplateDictionary TemplateDictionary { get { return templateDictionary; } set { templateDictionary =  value; } }
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null)
                return null;

            ICalculationResult result = (ICalculationResult)value;
            return result.Type != null ? TemplateDictionary.GetBestTemplateFor(result.Type) : TemplateDictionary.GetBestTemplateFor<object>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new InvalidOperationException();
        }
        #endregion
    }
}