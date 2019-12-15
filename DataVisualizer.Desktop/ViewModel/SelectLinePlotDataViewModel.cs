using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
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
    public class SelectLinePlotDataViewModel : SelectDataViewModel
    {
        public int[] XSelection { get; set; }
        public int[] YSelection { get; set; }

        #region Bindings
        private Array _chartTypes;
        public Array ChartTypes
        {
            get => _chartTypes;
            private set { _chartTypes = value; }
        }

        // TODO :: Enum instead
        private ChartType _chartType;
        public ChartType ChartType
        {
            get => _chartType;
            set 
            { 
                _chartType = value;
                OnPropertyChanged("ChartType");
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

        
        public SelectLinePlotDataViewModel() : base()
        {

        }
        public SelectLinePlotDataViewModel(IContext context, IDialogService dialogService) : base(context, dialogService)
        {
            XAxisSelected = true;
            ChartType = ChartType.Line;
            ChartTypes = Enum.GetValues(typeof(ChartType));
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
                //MaxSelectedColumns = 0;
            }
        }
        public override void OnOkClicked(object obj)
        {
            if (XAxisSelected)
                XSelection = SelectedRanges;
            else if (YAxisSelected)
                YSelection = SelectedRanges;
            if (XSelection == null || XSelection.Length == 0)
            {
                _error = true;
                _errorText += "Select X axis values.\n";
            }
            if (YSelection == null || YSelection.Length == 0)
            {
                _error = true;
                _errorText += "Select Y axis values.\n";
            }
            base.OnOkClicked(obj);
        }
    }
}
