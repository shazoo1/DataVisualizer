using DataVisualizer.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataVisualizer.Desktop.Helpers
{
    internal class PlotDataTemplateSelector : DataTemplateSelector
    {
        private Dictionary<Type, DataTemplate> _graphTypeToTemplate;
        public DataTemplate XYGraphTemplate { get; private set; }
        public DataTemplate PieChartTemplate { get; private set; }

        public PlotDataTemplateSelector() : base()
        {
            LoadTemplates();
            InitializeDictionary();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null && item is BaseGraphViewModel)
            {
                BaseGraphViewModel plot = item as BaseGraphViewModel;
                return _graphTypeToTemplate[plot.GetType()];
            }
            return null;
        }

        private void InitializeDictionary()
        {
            _graphTypeToTemplate = new Dictionary<Type, DataTemplate>();
            _graphTypeToTemplate.Add(typeof(XYPlotViewModel), XYGraphTemplate);
            _graphTypeToTemplate.Add(typeof(PieChartViewModel), PieChartTemplate);
        }

        private void LoadTemplates()
        {
            //Load plot templates
            var rd = new ResourceDictionary();
            rd.Source = new Uri("/Templates/XYGraphTemplate.xaml", UriKind.Relative);
            XYGraphTemplate = (DataTemplate)rd[(object)"XYGraphTemplate"];

            rd.Source = new Uri("/Templates/PieChartTemplate.xaml", UriKind.Relative);
            PieChartTemplate = (DataTemplate)rd[(object)"PieChartTemplate"];
        }
    }
}
