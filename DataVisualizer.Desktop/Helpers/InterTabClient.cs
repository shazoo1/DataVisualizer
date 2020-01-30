using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dragablz.Core;
using DataVisualizer.Desktop.Views;
using DataVisualizer.Desktop.ViewModel;

namespace DataVisualizer.Desktop.Helpers
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var view = new TabHostWindow();
            var model = new TabHostViewModel(interTabClient, partition);
            view.DataContext = model;
            return new NewTabHost<Window>(view, view.TabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
