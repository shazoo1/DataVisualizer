﻿using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.ViewModel;
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
        (int x, int[] y, ChartType type)? SelectXYPlotData(SelectXYPlotDataViewModel model);
        (string[] categories, double[] values)? SelectPieChartData(SelectPieChartDataViewModel model);
        void ShowWarning(string text);
    }
}