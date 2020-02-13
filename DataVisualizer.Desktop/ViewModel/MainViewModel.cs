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
using System.Windows;
using DataVisualizer.Desktop.ViewModel.Graphs;

namespace DataVisualizer.Desktop.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        #region Bindings
        private IInterTabClient _interTabClient;
        public IInterTabClient InterTabClient { get => _interTabClient; }

        private ObservableCollection<BaseGraphViewModel> _tabs;
        public ObservableCollection<BaseGraphViewModel> Tabs
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

        public ICommand OpenFileCommand { get; private set; }
        public ICommand AddNewTabCommand { get; private set; }
        public ICommand QuitCommand { get; private set; }

        //This has been added in tutorials. My hands are bound
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
            AddNewTabCommand = new RelayCommand(new Action<object>(AddNewTab));
            QuitCommand = new RelayCommand(new Action<object>(Quit));
            
            Tabs = new ObservableCollection<BaseGraphViewModel>();

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
                    //Don't show data preview if selected file path is null
                    //It occurs when user cancels file opening
                    if (FileName == null) return;
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
                _dialogService.ShowError(e);
                IsDataSourceConnected = false; 
            }
        }

        private void Quit(object obj)
        {
            Application.Current.Shutdown();
        }

        #region Tab Operations

        public void AddNewTab(object obj)
        {
            BaseGraphViewModel model = null;
            SurfaceType? sType = _dialogService.SelectSurfaceType();
            if (sType != null)
            {
                switch (sType.Value)
                {
                    case SurfaceType.PieChartSurface:
                        {
                            model = new PieChartViewModel(_context, _dialogService, _validationService);
                            break;
                        }
                    case SurfaceType.XYPlotSurface:
                        {
                            model = new XYPlotViewModel(_context, _dialogService, _validationService);
                            break;
                        }
                    case SurfaceType.VerticalSurface:
                        {
                            model = new VerticalPlotViewModel(_context, _dialogService, _validationService);
                            break;
                        }
                }
            }
            if (model != null)
            {
                Tabs.Add(model);
                model.TabIndex = Tabs.IndexOf(model);
                //return model.TabIndex;
            }
            //return -1;
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