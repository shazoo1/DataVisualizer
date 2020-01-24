using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using System.Windows.Media;
using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Services.Classes;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Desktop.Views;
using DataVisualizer.Persistence;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Classes;
using DavaVisualizer.Desktop.Services.Contracts;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Model;
using DataVisualizer.Desktop.Enums;

namespace DataVisualizer.Desktop.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        #region Bindings

        private ObservableCollection<BasePlotViewModel> _tabs;
        public ObservableCollection<BasePlotViewModel> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                RaisePropertyChanged("Tabs");
            }
        }
        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                RaisePropertyChanged("SelectedTabIndex");
            }
        }
        private ObservableCollection<IRenderableSeries> _series;
        public ObservableCollection<IRenderableSeries> RenderableSeries
        {
            get => _series;
            set
            {
                _series = value;
                RaisePropertyChanged("RenderableSeries");
            }
        }

        private string _chartTitle = "Hello SciChart World!";
        public string ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                _chartTitle = value;
                RaisePropertyChanged("ChartTitle");
            }
        }

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

        private string _fileName = "";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        private bool _isDataSourceConnected = false;
        public bool IsDataSourceConnected
        {
            get { return _isDataSourceConnected; }
            set
            {
                _isDataSourceConnected = value;
                RaisePropertyChanged("IsDataSourceConnected");
            }
        }

        private bool _hasPlots = false;
        public bool HasPlots
        {
            get => _hasPlots;
            set
            {
                _hasPlots = value;
                RaisePropertyChanged("HasPlots");
            }
        }

        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _openFileCommand; }
            private set { _openFileCommand = value; }
        }

        private ICommand _addPlotCommand;
        public ICommand AddPlotCommand
        {
            get { return _addPlotCommand; }
            private set { _addPlotCommand = value; }
        }

        #endregion

        private IValidationService _validationService;
        private IDialogService _dialogService;
        private IContext _context;


        public MainViewModel()
        {
            //Bind commands
            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            AddPlotCommand = new RelayCommand(new Action<object>(AddXYPlot));
            Tabs = new ObservableCollection<BasePlotViewModel>();

            _series = new ObservableCollection<IRenderableSeries>();
            _context = new CSVContext();
            
            //TODO :: dinjection
            _dialogService = new DialogService();
            _validationService = new ValidationService(_context);

            //Hardcode here
            
        }

        public void OpenFile(object obj)
        {
            try
            {
                DataPreviewViewModel model;
                if (!IsDataSourceConnected)
                {
                    FileName = _dialogService.OpenFile();
                    model = new DataPreviewViewModel(_dialogService, _context, FileName);
                }
                else
                {
                    model = new DataPreviewViewModel(_dialogService, _context);
                }
                if (_dialogService.PreviewFile(model, ref _context) == true)
                {
                    IsDataSourceConnected = true;
                }
            }
            catch (Exception e)
            {
                _dialogService.ShowWarning(e.Message + "\n" + e.StackTrace);
                IsDataSourceConnected = false; 
            }
        }

        public void AddXYPlot(object obj)
        {
            var selectionModel = new SelectXYPlotDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectXYPlotData(selectionModel);
            if (selection != null)
            {
                BasePlotViewModel model = new XYPlotViewModel(_context, _dialogService, _validationService);
                SelectedTabIndex = GetOrAddNewTab<XYPlotViewModel>(model);


                //There must be only one range for x
                var xValues = _context.GetNumericalColumnByIndex(selection.Value.x);
                
                foreach (int yColumnIndex in selection.Value.y)
                {
                    var yValues = _context.GetNumericalColumnByIndex(yColumnIndex);
                    model.RenderableSeries.Add(BuildXYChart(xValues, yValues, selection.Value.type));
                }
                HasPlots = _series.Count > 0;
            }
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
        

        private Color GetRandomColor()
        {
            Random rnd = new Random();
            Color randomColor = Color.FromScRgb(1, (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble());
            return randomColor;
        }

        //Create new tab, containing the needed Surface
        public int GetOrAddNewTab<T>(BasePlotViewModel model) where T : BasePlotViewModel
        {
            if (!Tabs.Contains(model))
            {
                Tabs.Add((T)model);
                model.TabIndex = Tabs.IndexOf(model);
            }
            return model.TabIndex;
        }
    }
}