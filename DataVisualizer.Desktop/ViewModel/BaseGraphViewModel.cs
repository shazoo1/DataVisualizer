using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using Dragablz;
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
    public abstract class BaseGraphViewModel : BaseViewModel
    {
        protected IContext _context;
        protected IDialogService _dialogService;
        protected IValidationService _validationService;
        internal SurfaceType SurfaceType;

        #region Bindables
        private bool _hasPlots;
        public bool HasPlots
        {
            get => _hasPlots;
            set => _hasPlots = value;
        }
        private string _chartTitle;
        public string ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                _chartTitle = value;
                RaisePropertyChanged("ChartTitle");
            }
        }

        private int _tabIndex;
        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
                RaisePropertyChanged("SelectedIndex");
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
        private ICommand _addSeriesCommand;
        public ICommand AddSeriesCommand
        {
            get => _addSeriesCommand;
            protected set => _addSeriesCommand = value;
        }

        private ICommand _removeSeriesCommand;
        public virtual ICommand RemoveSeriesCommand
        {
            get { return _removeSeriesCommand; }
            private set { _removeSeriesCommand = value; }
        }
        #endregion

        public BaseGraphViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
            _dialogService = dialogService;

            RemoveSeriesCommand = new RelayCommand(new Action<object>(RemoveSeries));
            AddSeriesCommand = new RelayCommand(new Action<object>(AddSeries));
            RenderableSeries = new ObservableCollection<IRenderableSeries>();
        }

        public void RemoveSeries(object series)
        {
            RenderableSeries.Remove((IRenderableSeries)series);
            HasPlots = _series.Count > 0;
        }

        //Every PlotViewModel should override this method
        protected virtual void AddSeries(object obj)
        {
            HasPlots = _series.Count > 0;
        }

        protected Color GetRandomColor()
        {
            Random rnd = new Random();
            Color randomColor = Color.FromScRgb(1, (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble(), (float)App.RANDOM.NextDouble());
            return randomColor;
        }
    }
}
