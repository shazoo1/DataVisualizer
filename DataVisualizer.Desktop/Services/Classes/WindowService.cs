using DataVisualizer.Desktop.ViewModel;
using DataVisualizer.Desktop.Views;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaVisualizer.Desktop.Services.Classes
{
    public class WindowService : IWindowService
    {
        public (int x, int[] y)? SelectColumns(SelectDataViewModel model)
        {
            var win = new SelectDataWindow();
            win.DataContext = model;
            model.CancelButtonClicked += win.Close;
            //win.Show();
            if (win.ShowDialog() == true)
            {
                var values = (model.XSelection.FirstOrDefault(), model.YSelection);
                return values;
            }
            return null;
        }
    }
}
