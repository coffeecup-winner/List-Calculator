using System.Windows;

namespace ListCalculatorControl.Templates {
    partial class TemplateRepository : ResourceDictionary {
        static TemplateRepository instance;

        static TemplateRepository Instance { get { return instance ?? (instance = new TemplateRepository()); } }
        public TemplateRepository() {
            InitializeComponent();
        }
        public static DataTemplate PlainTextTemplate { get { return Instance["plainTextTemplate"] as DataTemplate; } }
        public static DataTemplate CalculationErrorTemplate { get { return Instance["calculationErrorTemplate"] as DataTemplate; } }
        public static DataTemplate LineChartTemplate { get { return Instance["lineChartTemplate"] as DataTemplate; } }
    }
}