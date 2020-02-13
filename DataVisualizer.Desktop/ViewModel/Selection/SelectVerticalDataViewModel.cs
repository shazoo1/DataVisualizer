using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Common.Enums;
using DataVisualizer.Desktop.Classes;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.ViewModel.Selection
{
    public class SelectVerticalDataViewModel : SelectDataViewModel
    {
        //Both selections are column numbers
        public int[] XSelection { get; set; }
        public int[] YSelection { get; set; }

        #region Bindings
        private ObservableCollection<SeriesType> _availableSeriesTypes;
        public ObservableCollection<SeriesType> AvailableSeriesTypes
        {
            get => _availableSeriesTypes;
            private set { _availableSeriesTypes = value; }
        }

        private SeriesType _selectedSeriesType;
        public SeriesType SelectedSeriesType
        {
            get => _selectedSeriesType;
            set { _selectedSeriesType = value; }
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


        public SelectVerticalDataViewModel() : base()
        {

        }
        public SelectVerticalDataViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
            : base(context, dialogService, validationService)
        {
            XAxisSelected = true;

            AvailableSeriesTypes = new ObservableCollection<SeriesType>();
            AvailableSeriesTypes.Add(new SeriesType { ChartType = PlainSeriesTypes.Bar, SeriesTypeLabel = "Spaltendiagramm" });

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
            Validate();
            base.OnOkClicked(obj);
        }

        /// <summary>
        /// Validation of selection. ValidationService validates only values, so selection validation
        /// should be done in this method.
        /// </summary>
        protected override void Validate()
        {
            // Very common validation can be done before specific ones
            if (XSelection == null || XSelection.Length == 0)
            {
                HasSelectionError = true;
                ErrorText += "Auswählen Sie bitte X Werte.\n";
            }
            if (YSelection == null || YSelection.Length == 0)
            {
                HasSelectionError = true;
                ErrorText += "Auswählen Sie bitte Y Werte.\n";
            }
            if (!_validationService.ValidateNumerical(XSelection) ||
                            !_validationService.ValidateNumerical(YSelection))
            {
                HasSelectionError = true;
                ErrorText += string.Format("Die Werte von {0} müssen zahlenmäßig sein.\n", SelectedSeriesType.SeriesTypeLabel);
                return;
            }
            if (SelectedSeriesType == null)
            {
                HasSelectionError = true;
                ErrorText += "Wählen Sie bitte den Diagrammentyp.";
            }
            else
            {
                switch (SelectedSeriesType.ChartType)
                {
                    case PlainSeriesTypes.Line:
                        {
                            if (!_validationService.ValidateUnique(XSelection[0]) ||
                                !_validationService.ValidateOrdered(XSelection[0]))
                            {
                                HasSelectionError = true;
                                ErrorText += "X Werte müssen einzigartig und geordnet sein.\n";
                            }
                            break;
                        }
                    case PlainSeriesTypes.Column:
                        {
                            if (!_validationService.ValidateUnique(XSelection[0]))
                            {
                                HasSelectionError = true;
                                ErrorText += "X Werte müssen müssen einzigartig sein.\n";
                            }
                            break;
                        }
                    case PlainSeriesTypes.Scatter:
                        {
                            break;
                        }
                }
            }
        }
    }
}

