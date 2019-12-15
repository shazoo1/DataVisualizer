using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataVisualizer.Desktop.ViewModel
{
    public abstract class SelectDataViewModel : BindableObject
    {
        protected IContext _context;
        protected IDialogService _dialogService;
        protected IValidationService _validationService;

        protected bool Error;
        protected string ErrorText;

        public delegate void OnCancelButtonClicked();
        public event OnCancelButtonClicked CancelButtonClicked;

        public delegate void OnOkButtonClicked();
        public event OnOkButtonClicked OkButtonClicked;

        #region Bindings

        private string _tableName;
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                OnPropertyChanged("TableName");
            }
        }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set { _cancelCommand = value; }
        }
        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        private DataTable _previewData;
        public DataTable PreviewData
        {
            get => _previewData;
            set
            {
                _previewData = value;
                OnPropertyChanged("PreviewData");
            }
        }

        private bool _isMultipleRange;
        public bool IsMultipleRange
        {
            get => _isMultipleRange;
            set
            {
                _isMultipleRange = value;
                OnPropertyChanged("IsMultipleRange");
            }
        }

        private bool _containsHeader;
        public bool ContainsHeader
        {
            get => _containsHeader;
            set
            {
                _containsHeader = value;
                OnPropertyChanged("ContainsHeader");
            }
        }

        private int[] _selectedRanges;
        public int[] SelectedRanges
        {
            get => _selectedRanges;
            set
            {
                _selectedRanges = value;
                OnPropertyChanged("SelectedRanges");
            }
        }

        #endregion

        public SelectDataViewModel()
        {

        }

        public SelectDataViewModel (IContext context, IDialogService dialogService, IValidationService validationService)
        {
            _context = context;
            _dialogService = dialogService;
            _validationService = validationService;
            Error = false;

            PreviewData = context.GetFirstLines(100);

            OkCommand = new RelayCommand(OnOkClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
        }
        protected virtual void OnOkClicked(object obj)
        {
            if (Error)
            {
                _dialogService.ShowWarning(ErrorText);

                // Reset error immediately after warning is shown
                Error = false;
                ErrorText = "";
                return;
            }
            OkButtonClicked?.Invoke();
        }
        public virtual void OnCancelClicked(object obj)
        {
            CancelButtonClicked?.Invoke();
        }
    }
}
