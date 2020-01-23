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
        public DataTemplate XYGraphTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is BasePlotViewModel)
            {
                BasePlotViewModel plot = item as BasePlotViewModel;
                var GraphTabTemplates = (ResourceDictionary)element.FindResource("GraphTabTemplates");

                if (plot.GetType() == typeof(XYPlotViewModel))
                {
                    XYGraphTemplate = (DataTemplate)GraphTabTemplates[(object)"XYGraphTemplate"];
                    return XYGraphTemplate;
                }
                else
                    return
                        element.FindResource("myTaskTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
