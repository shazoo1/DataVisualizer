using DataVisualizer.Desktop.ViewModel;
using DataVisualizer.Desktop.Views;
using DavaVisualizer.Desktop.Services.Contracts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DavaVisualizer.Desktop.Services.Classes
{
    public class DialogService : IDialogService
    {
        private OpenFileDialog _dialog;

        public DialogService()
        {
            _dialog = new OpenFileDialog();
        }

        public string OpenFile()
        {
            if (_dialog.ShowDialog() == true)
            {
                return _dialog.FileName;
            }
            else return null;
        }

        // TODO :: Reuse selectors
        public (int x, int[] y)? SelectLinePlotData(SelectLinePlotDataViewModel model)
        {
            var selectionWindow = new SelectLinePlotDataWindow();
            selectionWindow.DataContext = model;
            model.CancelButtonClicked += () => {
                selectionWindow.DialogResult = false;
                selectionWindow.Close();
            };
            model.OkButtonClicked += () =>
            {
                selectionWindow.DialogResult = true;
                selectionWindow.Close();
            };
            if (selectionWindow.ShowDialog() == true)
            {
                var values = (model.XSelection.FirstOrDefault(), model.YSelection);
                return values;
            }
            return null;
        }

        public (string[] categories, double[] values)? SelectPieChartData(SelectPieChartDataViewModel model)
        {
            var selectionWindow = new SelectPieChartDataWindow();
            selectionWindow.DataContext = model;
            model.CancelButtonClicked += () => {
                selectionWindow.DialogResult = false;
                selectionWindow.Close();
            };
            model.OkButtonClicked += () =>
            {
                selectionWindow.DialogResult = true;
                selectionWindow.Close();
            };
            if (selectionWindow.ShowDialog() == true)
            {
                
            }
            return null;
        }

        public void ShowWarning(string text)
        {
            MessageBox.Show(text);
        }


    }
}
