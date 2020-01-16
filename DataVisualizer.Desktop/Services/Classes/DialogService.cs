using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.ViewModel;
using DataVisualizer.Desktop.Views;
using DataVisualizer.Persistence.Contracts;
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

        public bool PreviewFile(DataPreviewViewModel model, ref IContext context)
        {
            var dataPreviewWindow = new DataPreviewWindow();
            dataPreviewWindow.DataContext = model;
            model.OkButtonClicked += () => {
                dataPreviewWindow.DialogResult = true;

            };
            if (dataPreviewWindow.ShowDialog() == true)
            {
                context = model.Context;
                return true;
            }
            else return false;
        }

        public (int x, int[] y, ChartType type)? SelectXYPlotData(SelectXYPlotDataViewModel model)
        {
            var selectionWindow = new SelectXYPlotDataWindow();
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
                var values = (model.XSelection.FirstOrDefault(), model.YSelection, model.ChartType);
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

        public void ShowError(Exception e)
        {
            var messageText = string.Format("Exception: {0}\nStackTrace:{1}\nInner:{2}", e.Message, e.StackTrace, e.InnerException.Message);
            MessageBox.Show(messageText);
        }
    }
}
