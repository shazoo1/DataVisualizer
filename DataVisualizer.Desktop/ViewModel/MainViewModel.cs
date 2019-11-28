using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using System.Windows.Media;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Persistence;
using DavaVisualizer.Services.Classes;
using DavaVisualizer.Services.Contracts;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
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
        #endregion

        #region Temporary bindings
        //They will be probably deleted after adding new features
        private int _xValuesIndex;
        public int XValuesIndex
        {
            get { return _xValuesIndex; }
            set
            {
                _xValuesIndex = value;
                OnPropertyChanged("XValuesIndex");
            }
        }

        private int _yValuesIndex;
        public int YValuesIndex
        {
            get { return _yValuesIndex; }
            set
            {
                _yValuesIndex = value;
                OnPropertyChanged("YValuesIndex");
            }
        }

        private ICommand _addSeriesCommand;
        public ICommand AddSeriesCommand
        {
            get { return _addSeriesCommand; }
            private set { _addSeriesCommand = value; }
        }
        #endregion

        private IFileDialogService _dialogService;

        //TODO :: Interface
        private CSVContext _context;

        private double[] _xValues;


        public MainViewModel()
        {
            //Bind commands
            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            AddSeriesCommand = new RelayCommand(new Action<object>(AddSeries));

            _series = new ObservableCollection<IRenderableSeriesViewModel>();
            //TODO :: dinjection
            _dialogService = new FileDialogService();
            
            //Hardcode here
            _context = new CSVContext();

            
            //plotData.Append(xAxis, yAxis);
        }

        public void OpenFile(object obj)
        {
            try
            {
                var file = _dialogService.OpenFile();
                if (!string.IsNullOrEmpty(file))
                {
                    _context.ReadFile(file);
                    FileName = file;
                    IsFileOpen = true;
                }
            }
            catch (Exception) { IsFileOpen = false; }
        }

        public void AddSeries(object obj)
        {
            var xValues = _context.GetNumericalColumnByIndex(XValuesIndex);
            var yValues = _context.GetNumericalColumnByIndex(YValuesIndex);
            AddSeries(xValues, yValues);
        }

        public void AddSeries(double[] xValues, double[] yValues)
        {
            var newSeriesData = new XyDataSeries<double, double>() { SeriesName = "ChangeMe" };
            newSeriesData.Append(xValues, yValues);
            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                DataSeries = newSeriesData
            });
        }
    }
}