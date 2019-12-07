using DataVisualizer.Desktop.Enums;
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
        #region Bindings
        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                OnPropertyChanged("TableName");
            }
        }
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
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
            get { return _previewData; }
            set
            {
                _previewData = value;
                OnPropertyChanged("PreviewData");
            }
        }

        private bool _isSingleRange;
        public bool IsSingleRange
        {
            get { return _isSingleRange; }
            set
            {
                _isSingleRange = value;
                OnPropertyChanged("IsSingleRange");
            }
        }

        private bool _containsHeader;
        public bool ContainsHeader
        {
            get { return _containsHeader; }
            set
            {
                _containsHeader = value;
                OnPropertyChanged("ContainsHeader");
            }
        }

        private int[] _selectedRanges;
        public int[] SelectedRanges
        {
            get { return _selectedRanges; }
            set
            {
                _selectedRanges = value;
                OnPropertyChanged("SelectedRanges");
            }
        }
        public IEnumerable<RangeType> RangeTypes
        {
            get
            {
                return Enum.GetValues(typeof(RangeType)).Cast<RangeType>();
            }
        }

        private RangeType _rangeType = RangeType.Argument;
        public RangeType RangeType
        {
            get { return _rangeType; }
            set
            {
                _rangeType = value;
                OnPropertyChanged("RangeType");
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

            //Add bindings

            //TODO :: Add customization
            _previewData = context.GetFirstLines(100);
        }
    }
}
