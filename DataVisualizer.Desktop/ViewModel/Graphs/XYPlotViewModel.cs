using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DataVisualizer.Desktop.ViewModel
{
    internal class XYPlotViewModel : BasePlotViewModel
    {
        #region Bindables

        private string _xAxisTitle = "XAxis";
        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set
            {
                _xAxisTitle = value;
                RaisePropertyChanged("XAxisTitle");
            }
        }

        private string _yAxisTitle = "YAxis";
        public string YAxisTitle
        {
            get { return _yAxisTitle; }
            set
            {
                _yAxisTitle = value;
                RaisePropertyChanged("YAxisTitle");
            }
        }

        private ICommand _addPlotCommand;
        public ICommand AddPlotCommand
        {
            get { return _addPlotCommand; }
            private set { _addPlotCommand = value; }
        }
        #endregion

        public XYPlotViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
            : base(context, dialogService, validationService)
        {
            VMType = Enums.VMType.XYPlotViewModel;
        }

        protected override void AddSeries(object obj)
        {
            var selectionModel = new SelectXYPlotDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectXYPlotData(selectionModel);
            if (selection != null)
            {
                //There must be only one range for x
                var xValues = _context.GetNumericalColumnByIndex(selection.Value.x);

                foreach (int yColumnIndex in selection.Value.y)
                {
                    var yValues = _context.GetNumericalColumnByIndex(yColumnIndex);
                    RenderableSeries.Add(BuildXYChart(xValues, yValues, selection.Value.type));
                }
            }
            base.AddSeries(obj);
        }

        public BaseRenderableSeries BuildXYChart(double[] xValues, double[] yValues, ChartType type)
        {
            var newSeriesData = new XyDataSeries<double, double>();
            BaseRenderableSeries series = null;
            switch (type)
            {
                case ChartType.Line:
                    {
                        series = new FastLineRenderableSeries();
                        break;
                    }
                case ChartType.Column:
                    {
                        newSeriesData.AcceptsUnsortedData = true;
                        series = new FastColumnRenderableSeries();
                        break;
                    }
                case ChartType.Scatter:
                    {
                        newSeriesData.AcceptsUnsortedData = true;
                        series = new XyScatterRenderableSeries();
                        series.PointMarker = new Abt.Controls.SciChart.Visuals.PointMarkers.EllipsePointMarker();
                        series.StrokeThickness = 5;
                        break;
                    }
            }
            newSeriesData.Append(xValues, yValues);

            series.DataSeries = newSeriesData;
            series.Tag = "Chart";
            series.IsVisible = true;
            series.SeriesColor = GetRandomColor();
            return series;
        }
    }
}
