using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Persistence.Contracts;
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
    public class SelectDataViewModel : BindableObject
    {
        public int[] XSelection { get; set; }
        public int[] YSelection { get; set; }

        public delegate void OnOkButtonClicked();
        public event OnOkButtonClicked OkButtonClicked;

        public delegate void OnCancelButtonClicked();
        public event OnCancelButtonClicked CancelButtonClicked;

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

        private bool _YAxisSelected;
        public bool YAxisSelected
        {
            get => _YAxisSelected;
            set
            {
                _YAxisSelected = value;
                if (value)
                {
                    OnAxisChanged("y");
                }
                OnPropertyChanged("YAxisSelected");
            }
        }

        private bool _XAxisSelected;
        public bool XAxisSelected
        {
            get => _XAxisSelected;
            set
            {
                _XAxisSelected = value;
                if (value)
                {
                    OnAxisChanged("x");
                }
                OnPropertyChanged("XAxisSelected");
            }
        }

        #endregion

        IContext _context;
        public SelectDataViewModel()
        {

        }
        public SelectDataViewModel(IContext context)
        {
            _context = context;

            //TODO :: Add customization
            _previewData = context.GetFirstLines(100);
            XAxisSelected = true;
            OkCommand = new RelayCommand(OnOkClicked);
            CancelCommand = new RelayCommand(OnCancelClicked);
        }

        public void OnAxisChanged(string newAxis)
        {
            if (newAxis == "x")
            {
                IsMultipleRange = false;
                YSelection = SelectedRanges;
                SelectedRanges = XSelection;
            }
            else
            {
                IsMultipleRange = true;
                XSelection = SelectedRanges;
                SelectedRanges = YSelection;
            }
        }

        public void OnOkClicked(object obj)
        {
            if (XAxisSelected)
                XSelection = SelectedRanges;
            if (YAxisSelected)
                YSelection = SelectedRanges;
            OkButtonClicked?.Invoke();
        }
        public void OnCancelClicked(object obj)
        {
            CancelButtonClicked?.Invoke();
        }
    }
}
