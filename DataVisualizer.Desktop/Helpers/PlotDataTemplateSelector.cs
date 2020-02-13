using DataVisualizer.Desktop.Templates;
using DataVisualizer.Desktop.ViewModel;
using DataVisualizer.Desktop.ViewModel.Graphs;
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
        public DataTemplate VerticalPlotTemplate { get; private set; }

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
                try
                {
                    return _graphTypeToTemplate[plot.GetType()];
                }
                catch (KeyNotFoundException) { }
            }
            return null;
        }

        private void InitializeDictionary()
        {
            _graphTypeToTemplate = new Dictionary<Type, DataTemplate>();
            _graphTypeToTemplate.Add(typeof(XYPlotViewModel), XYGraphTemplate);
            _graphTypeToTemplate.Add(typeof(PieChartViewModel), PieChartTemplate);
            _graphTypeToTemplate.Add(typeof(VerticalPlotViewModel), VerticalPlotTemplate);
        }

        private void LoadTemplates()
        {
            //Load plot templates
            var rd = new ResourceDictionary();
            rd.Source = new Uri("/Templates/XYGraphTemplate.xaml", UriKind.Relative);
            XYGraphTemplate = (DataTemplate)rd["XYGraphTemplate"];

            rd.Source = new Uri("/Templates/PieChartTemplate.xaml", UriKind.Relative);
            PieChartTemplate = (DataTemplate)rd["PieChartTemplate"];

            rd.Source = new Uri("/Templates/VerticalPlotTemplate.xaml", UriKind.Relative);
            VerticalPlotTemplate = (DataTemplate)rd["VerticalPlotTemplate"];
        }
    }
}
