using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Desktop.ViewModel.Selection;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataVisualizer.Desktop.ViewModel.Graphs
{
    class VerticalPlotViewModel : BaseGraphViewModel
    {
        #region Bindings

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

        private ObservableCollection<string> _xLabels;
        public ObservableCollection<string> XLabels
        {
            get => _xLabels;
            set { _xLabels = value; }
        }

        private IAxis _xAxis;
        public IAxis XAxis
        {
            get => _xAxis;
            set
            {
                _xAxis = value;
            }
        }

        private IAxis _yAxis;
        public IAxis YAxis
        {
            get => _yAxis;
            set
            {
                _yAxis = value;
            }
        }

        private ICommand _addPlotCommand;
        public ICommand AddPlotCommand
        {
            get { return _addPlotCommand; }
            private set { _addPlotCommand = value; }
        }
        #endregion

        public VerticalPlotViewModel(IContext context, IDialogService dialogService,
            IValidationService validationService) : base(context, dialogService, validationService)
        {
            SurfaceType = Enums.SurfaceType.VerticalSurface;
            ChartTitle = "Senkrechter Grafik";
            XLabels = new ObservableCollection<string>();

            AddAxes();
        }

        protected override void AddSeries(object obj)
        {
            var selectionModel = new SelectVerticalDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectVerticalPlotData(selectionModel);
            if (selection != null)
            {
                //There must be only one range for x
                var xValues = _context.GetTextualColumnByIndex(selection.Value.x);

                foreach (int yColumnIndex in selection.Value.y)
                {
                    var yValues = _context.GetNumericalColumnByIndex(yColumnIndex);
                    RenderableSeries.Add(BuildChart(xValues, yValues, selection.Value.type));
                }
            }
            base.AddSeries(obj);
        }

        /// <summary>
        /// Build a vertical chart. Vertical chart will have categories in vertical axis.
        /// To have numbers in vertical axis, use common XY Chart surface. Important: vertical axis is
        /// an X axis.
        /// </summary>
        /// <param name="xValues">Array of categories. However, numbers can also be handled like categories.</param>
        /// <param name="yValues">Array of numbers.</param>
        /// <param name="type">Graph type</param>
        /// <returns></returns>
        public BaseRenderableSeries BuildChart(string[] xValues, double[] yValues, PlainSeriesTypes type)
        {
            var newSeriesData = new XyDataSeries<int, double>();
            var labelPositions = GetLabelPositions(xValues);
            BaseRenderableSeries series = null;
            switch (type)
            {
                case PlainSeriesTypes.Bar:
                    {
                        newSeriesData.AcceptsUnsortedData = true;
                        series = new FastColumnRenderableSeries();
                        break;
                    }
            }
            newSeriesData.Append(labelPositions, yValues);

            series.DataSeries = newSeriesData;
            series.Name = "Chart";
            series.IsVisible = true;
            series.SeriesColor = GetRandomColor();
            return series;
        }

        private void AddAxes()
        {
            //TODO :: Add axis labels
            XAxis = new CategoryDateTimeAxis() {
                AxisAlignment = AxisAlignment.Left
                
            };
            YAxis = new NumericAxis() {
                AxisAlignment = AxisAlignment.Bottom
            };
        }

        /// <summary>
        /// Get label positions on the graph. Create if there are none.
        /// </summary>
        /// <param name="labels">String labels</param>
        /// <returns></returns>
        private int[] GetLabelPositions(string[] labels)
        {
            List<int> indices = new List<int>();
            //If there are no labels, then just add all of the given labels and return indices.
            if (XLabels == null || XLabels.Count == 0)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    XLabels.Add(labels[i]);
                    indices.Add(i);
                }
            }
            //Else, get indices of the existing ones and add new labels
            else
            {
                foreach(string label in labels)
                {
                    int existingIndex = XLabels.IndexOf(label);
                    if (existingIndex != -1)
                        indices.Add(existingIndex);
                    else
                    {
                        XLabels.Add(label);
                        indices.Add(XLabels.IndexOf(label));
                    }
                }
            }
            return indices.ToArray();
        }
    }
}
