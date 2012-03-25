using System;
using System.Collections.Generic;
using System.Windows;

namespace ListCalculatorControl {
    public class TypedDataTemplateDictionary : Dictionary<Type, DataTemplate> {
        public DataTemplate DefaultOutputAreaTemplate { get; set; }
    }
}