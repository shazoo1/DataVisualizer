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
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace DataVisualizer.Desktop.ViewModel
{
    public class MainViewModel : BindableObject
    {

        #region Bindings
        private ObservableCollection<IRenderableSeriesViewModel> _series;
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("RenderableSeries");
            }
        }

        private string _chartTitle = "Hello SciChart World!";
        public string ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                _chartTitle = value;
                OnPropertyChanged("ChartTitle");
            }
        }

        private string _xAxisTitle = "XAxis";
        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set
            {
                _xAxisTitle = value;
                OnPropertyChanged("XAxisTitle");
            }
        }

        private string _yAxisTitle = "YAxis";
        public string YAxisTitle
        {
            get { return _yAxisTitle; }
            set
            {
                _yAxisTitle = value;
                OnPropertyChanged("YAxisTitle");
            }
        }

        private bool _enableZoom = true;
        public bool EnableZoom
        {
            get { return _enableZoom; }
            set
            {
                if (_enableZoom != value)
                {
                    _enableZoom = value;
                    OnPropertyChanged("EnableZoom");
                    if (_enableZoom) EnablePan = false;
                }
            }
        }

        private bool _enablePan;
        public bool EnablePan
        {
            get { return _enablePan; }
            set
            {
                if (_enablePan != value)
                {
                    _enablePan = value;
                    OnPropertyChanged("EnablePan");
                    if (_enablePan) EnableZoom = false;
                }
            }
        }

        private string _fileName = "";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private bool _isFileOpen = false;
        public bool IsFileOpen
        {
            get { return _isFileOpen; }
            set
            {
                _isFileOpen = value;
                OnPropertyChanged("IsFileOpen");
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

        private ICommand _removeSeriesCommand;
        public ICommand RemoveSeriesCommand
        {
            get { return _removeSeriesCommand; }
            private set { _removeSeriesCommand = value; }
        }
        #endregion

        private IValidationService _validationService;
        private IDialogService _dialogService;
        private IContext _context;


        public MainViewModel()
        {
            //Bind commands
            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            AddPlotCommand = new RelayCommand(new Action<object>(AddPlot));
            RemoveSeriesCommand = new RelayCommand(new Action<object>(RemoveSeries));

            _series = new ObservableCollection<IRenderableSeriesViewModel>();
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
                var file = _dialogService.OpenFile();
                if (!string.IsNullOrEmpty(file))
                {
                    _context.ReadData(file);
                    FileName = file;
                    IsFileOpen = true;
                }
            }
            catch (Exception) { IsFileOpen = false; }
        }

        public void AddPlot(object obj)
        {
            var model = new SelectXYPlotDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectXYPlotData(model);
            if (selection != null)
            {
                //There must be only one range for x
                var xValues = _context.GetNumericalColumnByIndex(selection.Value.x);
                foreach (int yColumnIndex in selection.Value.y)
                {
                    var yValues = _context.GetNumericalColumnByIndex(yColumnIndex);
                    BuildXYChart(xValues, yValues, selection.Value.type);
                }
            }
        }

        public int BuildXYChart(double[] xValues, double[] yValues, ChartType type)
        {
            var newSeriesData = new XyDataSeries<double, double>();
            BaseRenderableSeriesViewModel series = null;
            switch (type)
            {
                case ChartType.Line:
                    {
                        series = new LineRenderableSeriesViewModel();
                        break;
                    }
                case ChartType.Column:
                    {
                        newSeriesData.AcceptsUnsortedData = true;
                        series = new ColumnRenderableSeriesViewModel();
                        break;
                    }
                case ChartType.Scatter:
                    {
                        newSeriesData.AcceptsUnsortedData = true;
                        series = new ExtremeScatterRenderableSeriesViewModel();
                        series.PointMarker = new SciChart.Charting.Visuals.PointMarkers.EllipsePointMarker();
                        series.StrokeThickness = 5;
                        break;
                    }
            }
            newSeriesData.Append(xValues, yValues);

            series.DataSeries = newSeriesData;
            series.Tag = "Chart";
            series.IsVisible = true;
            series.Stroke = GetRandomColor();

            if (series != null)
            {
                RenderableSeries.Add(series);
                return RenderableSeries.IndexOf(series);
            }
            return -1;
        }

        public void RemoveSeries(object series)
        {
            RenderableSeries.Remove((IRenderableSeriesViewModel)series);
        }

        private Color GetRandomColor()
        {
            Random rnd = new Random();
            Color randomColor = Color.FromScRgb(1, (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble());
            return randomColor;
        }
    }
}