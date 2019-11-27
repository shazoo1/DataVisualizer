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
        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeries;
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries
        {
            get { return _renderableSeries; }
            set
            {
                _renderableSeries = value;
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

        private ICommand m_OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get { return m_OpenFileCommand; }
            private set { m_OpenFileCommand = value; }
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

            _renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            //TODO :: dinjection
            _dialogService = new FileDialogService();
            
            //Hardcode here
            _context = new CSVContext("C:\\Users\\Arli\\Desktop\\SampleCSV.csv", ',', '"', true);

            var plotData = new XyDataSeries<double, double>() { SeriesName = "TestCSV" };
            FileName = _context.GetFileName();
            
            _xValues = _context.GetNumericalColumnByIndex(0);
            double[] yAxis = _context.GetNumericalColumnByIndex(3);
            AddSeries(yAxis);

            //plotData.Append(xAxis, yAxis);

            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = plotData,
            });
        }

        public void OpenFile(object obj)
        {
            try
            {
                var file = _dialogService.OpenFile();
                _context.SetConnectionString(file);
                _context.ReadLines();
                FileName = file;
            }
            catch (Exception) { }
        }

        public void AddSeries(double[] yValues)
        {
            var newSeriesData = new XyDataSeries<double, double>() { SeriesName = "ChangeMe" };
            newSeriesData.Append(_xValues, yValues);
            RenderableSeries.Add(new LineRenderableSeriesViewModel()
            {
                DataSeries = newSeriesData
            });
        }
    }
}