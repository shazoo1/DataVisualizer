using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.ViewModel;
using DataVisualizer.Desktop.ViewModel.Selection;
using DataVisualizer.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaVisualizer.Desktop.Services.Contracts
{
    public interface IDialogService
    {
        string OpenFile();
        (int x, int[] y, PlainSeriesTypes type)? SelectXYPlotData(SelectXYPlotDataViewModel model);
        (int categories, int values)? SelectPieChartData(SelectPieChartDataViewModel model);
        (int x, int[] y, PlainSeriesTypes type)? SelectVerticalPlotData(SelectVerticalDataViewModel model);
        void ShowWarning(string text);
        void ShowError(Exception e);
        bool PreviewFile(DataPreviewViewModel model, ref IContext context);
        SurfaceType? SelectSurfaceType();
    }
}
