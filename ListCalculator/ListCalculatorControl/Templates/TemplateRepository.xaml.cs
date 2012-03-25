using System.Windows;

namespace ListCalculatorControl.Templates {
    partial class TemplateRepository : ResourceDictionary {
        static TemplateRepository instance;

        static TemplateRepository Instance { get { return instance ?? (instance = new TemplateRepository()); } }

        public static DataTemplate PlaintTextTemplate { get { return Instance["plainTextTemplate"] as DataTemplate; } }

        public TemplateRepository() {
            InitializeComponent();
        }
    }
}