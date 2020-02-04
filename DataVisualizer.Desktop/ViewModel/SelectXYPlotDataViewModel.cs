using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using Abt.Controls.SciChart.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace DataVisualizer.Desktop.ViewModel
{
    public class SelectXYPlotDataViewModel : SelectDataViewModel
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

        private ChartType _chartType;
        public ChartType ChartType
        {
            get => _chartType;
            set 
            { 
                _chartType = value;
                RaisePropertyChanged("ChartType");
            }
        }

        private bool _YAxisSelected;
        public bool YAxisSelected
        {
            get => _YAxisSelected;
            set
            {
                if (value != _YAxisSelected)
                {
                    _YAxisSelected = value;
                    if (value)
                    {
                        OnAxisChanged("y");
                    }
                    RaisePropertyChanged("YAxisSelected");
                }
            }
        }

        private bool _XAxisSelected;
        public bool XAxisSelected
        {
            get => _XAxisSelected;
            set
            {
                if (value != _XAxisSelected)
                {
                    _XAxisSelected = value;
                    if (value)
                    {
                        OnAxisChanged("x");
                    }
                    RaisePropertyChanged("XAxisSelected");
                }
            }
        }
        private ObservableCollection<IRenderableSeries> _renderableSeries = new ObservableCollection<IRenderableSeries>();
        public ObservableCollection<IRenderableSeries> RenderableSeries
        {
            get => _renderableSeries;
            set
            {
                _renderableSeries = value;
                RaisePropertyChanged("RenderableSeries");
            }
        }
        #endregion


        public SelectXYPlotDataViewModel() : base()
        {

        }
        public SelectXYPlotDataViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
            : base(context, dialogService, validationService)
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
        protected override void OnOkClicked(object obj)
        {
            if (XAxisSelected)
                XSelection = SelectedRanges;
            else if (YAxisSelected)
                YSelection = SelectedRanges;
            if (XSelection == null || XSelection.Length == 0)
            {
                Error = true;
                ErrorText += "Auswählen Sie bitte X Werte.\n";
            }
            if (YSelection == null || YSelection.Length == 0)
            {
                Error = true;
                ErrorText += "Auswählen Sie bitte Y Werte.\n";
            }
            //No need to validate if no data selected
            if (!Error)
                Validate();
            //Error message is shown in the base class. Error value will be reset.
            base.OnOkClicked(obj);
        }

        private void Validate()
        {
            // Very common validation can be done before specific ones
            if (!_validationService.ValidateNumerical(XSelection) ||
                            !_validationService.ValidateNumerical(YSelection))
            {
                Error = true;
                ErrorText += string.Format("{0} Diagramm Werte müssen zahlenmäßig sein.\n", ChartType.ToString());
                return;
            }
            switch (ChartType)
            {
                case ChartType.Line:
                    {
                        if (!_validationService.ValidateUnique(XSelection[0]) ||
                            !_validationService.ValidateOrdered(XSelection[0]))
                        {
                            Error = true;
                            ErrorText += "X Werte müssen einzigartig und geordnet sein.\n";
                        }
                        break;
                    }
                case ChartType.Column:
                    {
                        if (!_validationService.ValidateUnique(XSelection[0]))
                        {
                            Error = true;
                            ErrorText += "X Werte müssen müssen einzigartig sein.\n";
                        }
                        break;
                    }
                case ChartType.Scatter:
                    {
                        break;
                    }
            }
        }
    }
}
