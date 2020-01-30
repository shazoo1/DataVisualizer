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
using Dragablz;
using System.Diagnostics;
using Dragablz.Dockablz;
using System.Collections.Generic;
using System.Linq;

namespace DataVisualizer.Desktop.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        #region Bindings
        private IInterTabClient _interTabClient;
        public IInterTabClient InterTabClient { get => _interTabClient; }

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

        public ICommand _addPieChartCommand;
        public ICommand AddPieChartCommand
        {
            get => _addPieChartCommand;
            private set { _addPieChartCommand = value; }
        }

        //This has been added in tutorials
        private readonly object _partition;
        public object Partition
        {
            get { return _partition; }
        }

        private readonly ObservableCollection<HeaderedItemViewModel> _floatingItems;
        public ObservableCollection<HeaderedItemViewModel> FloatingItems
        {
            get { return _floatingItems; }
        }

        private readonly ObservableCollection<HeaderedItemViewModel> _toolItems
            = new ObservableCollection<HeaderedItemViewModel>();
        public ObservableCollection<HeaderedItemViewModel> ToolItems
        {
            get { return _toolItems; }
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
            AddPieChartCommand = new RelayCommand(new Action<object>(AddPieChart));
            Tabs = new ObservableCollection<BasePlotViewModel>();

            _series = new ObservableCollection<IRenderableSeries>();
            _context = new CSVContext();
            
            //TODO :: dinjection
            _dialogService = new DialogService();
            _validationService = new ValidationService(_context);
            _interTabClient = new InterTabClient();

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

        //TODO :: Move to PieChartViewModel
        private void AddPieChart(object obj)
        {
            var selectionModel = new SelectPieChartDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectPieChartData(selectionModel).Value;
            var chartData = new ObservableCollection<KeyValuePair<string, double>>();
            var model = new PieChartViewModel(_context, _dialogService, _validationService);
            if (_validationService.ValidateCategorical(selection.categories))
            {
                var categories = _context.GetTextualColumnByIndex(selection.categories);
                if (selection.values != -1 && _validationService.ValidateNumerical(selection.values))
                {
                    var values = _context.GetNumericalColumnByIndex(selection.values);
                    model.Segments = (ObservableCollection<KeyValuePair<string, double>>)
                        BuildPieChartSegments(categories, values);
                }
                else
                {
                    model.Segments = (ObservableCollection<KeyValuePair<string, double>>)
                       BuildPieChartSegments(categories);
                }
            }
            GetOrAddNewTab<PieChartViewModel>(model);
        }

        public ObservableCollection<KeyValuePair<string, double>> BuildPieChartSegments(string[] categories)
        {
            var aggregation = categories.GroupBy(x => x)
                .Select(val => new Tuple<string, double>(val.Key, val.Count()));
            var preparedCategories = aggregation.Select(x => x.Item1).ToArray();
            var preparedValues = aggregation.Select(x => x.Item2).ToArray();

            return BuildPieChartSegments(preparedCategories, preparedValues);
        }

        public ObservableCollection<KeyValuePair<string, double>> BuildPieChartSegments(string[] categories, double[] values)
        {
            var result = new ObservableCollection<KeyValuePair<string, double>>();
            for (int i = 0; i < categories.Length; i++)
            {
                result.Add(new KeyValuePair<string, double>(categories[i], values[i]));
            }
            return result;
        }

        //TODO :: Move to XYPlotViewModel
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


        #region Tab Operations
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

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }

        /// <summary>
        /// Callback to handle tab closing.
        /// </summary>        
        private static void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model:
            var viewModel = args.DragablzItem.DataContext as HeaderedItemViewModel;
            Debug.Assert(viewModel != null);

            //here's how you can cancel stuff:
            //args.Cancel(); 
        }

        public ClosingFloatingItemCallback ClosingFloatingItemHandler
        {
            get { return ClosingFloatingItemHandlerImpl; }
        }

        /// <summary>
        /// Callback to handle floating toolbar/MDI window closing.
        /// </summary>        
        private static void ClosingFloatingItemHandlerImpl(ItemActionCallbackArgs<Layout> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model: 
            var disposable = args.DragablzItem.DataContext as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            //here's how you can cancel stuff:
            //args.Cancel(); 
        }
        #endregion
    }
}