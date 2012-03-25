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
        #region Chart templates
        public static DataTemplate AreaChartTemplate { get { return Instance["areaChartTemplate"] as DataTemplate; } }
        public static DataTemplate BarChartTemplate { get { return Instance["barChartTemplate"] as DataTemplate; } }
        public static DataTemplate BubbleChartTemplate { get { return Instance["bubbleChartTemplate"] as DataTemplate; } }
        public static DataTemplate ColumnChartTemplate { get { return Instance["columnChartTemplate"] as DataTemplate; } }
        public static DataTemplate LineChartTemplate { get { return Instance["lineChartTemplate"] as DataTemplate; } }
        public static DataTemplate PieChartTemplate { get { return Instance["pieChartTemplate"] as DataTemplate; } }
        public static DataTemplate ScatterChartTemplate { get { return Instance["scatterChartTemplate"] as DataTemplate; } }
        #endregion
    }
}