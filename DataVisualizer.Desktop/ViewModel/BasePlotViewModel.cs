using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        public BasePlotViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
            _dialogService = dialogService;

            RenderableSeries = new ObservableCollection<IRenderableSeries>();
        }
    }
}
