using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using System.Windows.Media;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Views;
using DataVisualizer.Persistence;
using DataVisualizer.Persistence.Contracts;
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

        private ICommand _addSeriesCommand;
        public ICommand AddSeriesCommand
        {
            get { return _addSeriesCommand; }
            private set { _addSeriesCommand = value; }
        }

        private ICommand _removeSeriesCommand;
        public ICommand RemoveSeriesCommand
        {
            get { return _removeSeriesCommand; }
            private set { _removeSeriesCommand = value; }
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

        #endregion

        private IFileDialogService _dialogService;

        private IContext _context;


        public MainViewModel()
        {
            //Bind commands
            OpenFileCommand = new RelayCommand(new Action<object>(OpenFile));
            AddSeriesCommand = new RelayCommand(new Action<object>(AddSeries));
            RemoveSeriesCommand = new RelayCommand(new Action<object>(RemoveSeries));

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
                    _context.ReadData(file);
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
            ProceedToSelectionWindow();
            //AddSeries(xValues, yValues);
        }

        public void ProceedToSelectionWindow()
        {
            var model = new SelectDataViewModel(_context);
            var view = new SelectDataWindow(model);
            view.Show();
        }
        public int AddSeries(double[] xValues, double[] yValues)
        {
            var newSeriesData = new XyDataSeries<double, double>() { SeriesName = "ChangeMe" };
            newSeriesData.Append(xValues, yValues);
            var lineSeries = new LineRenderableSeriesViewModel() { DataSeries = newSeriesData };
            RenderableSeries.Add(lineSeries);
            var style = lineSeries.StyleKey;
            return RenderableSeries.IndexOf(lineSeries); 
        }

        public void RemoveSeries(object series)
        {
            RenderableSeries.Remove((IRenderableSeriesViewModel)series);
        }
    }
}