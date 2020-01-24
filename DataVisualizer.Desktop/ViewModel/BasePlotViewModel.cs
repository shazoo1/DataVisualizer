using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
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

namespace DataVisualizer.Desktop.ViewModel
{
    public abstract class BasePlotViewModel : BaseViewModel
    {
        protected IContext _context;
        protected IDialogService _dialogService;
        protected IValidationService _validationService;
        internal VMType VMType;

        #region Bindables
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

        private ICommand _removeSeriesCommand;
        public virtual ICommand RemoveSeriesCommand
        {
            get { return _removeSeriesCommand; }
            private set { _removeSeriesCommand = value; }
        }
        #endregion

        public BasePlotViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
            _dialogService = dialogService;

            RemoveSeriesCommand = new RelayCommand(new Action<object>(RemoveSeries));
            RenderableSeries = new ObservableCollection<IRenderableSeries>();
        }

        public void RemoveSeries(object series)
        {
            RenderableSeries.Remove((IRenderableSeries)series);
            //HasPlots = _series.Count > 0;
        }
    }
}
