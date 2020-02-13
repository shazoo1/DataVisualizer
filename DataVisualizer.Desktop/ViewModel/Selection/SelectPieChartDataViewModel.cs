using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using Abt.Controls.SciChart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataVisualizer.Desktop.Helpers;

namespace DataVisualizer.Desktop.ViewModel
{
    public class SelectPieChartDataViewModel : SelectDataViewModel
    {
        #region Bindings
        public int[] CategoryColumns { get; set; }
        public int[] ValueColumns { get; set; }
        private bool _valueSelected;
        public bool ValueSelected
        {
            get => _valueSelected;
            set
            {
                //Do nothing if the value doesn't change
                if (ValueSelected != value)
                {
                    _valueSelected = value;
                    RaisePropertyChanged("ValueSelected");
                    SwitchSelection("value");
                }
            }
        }

        private bool _categorySelected;
        public bool CategorySelected
        {
            get => _categorySelected;
            set
            {
                //Do nothing if the value doesn't change
                if (CategorySelected != value)
                {
                    _categorySelected = value;
                    RaisePropertyChanged("CategorySelected");
                    SwitchSelection("category");
                }
            }
        }
        #endregion

        public SelectPieChartDataViewModel() : base()
        {

        }

        public SelectPieChartDataViewModel(IContext context, IDialogService dialogService, IValidationService validationService) 
            : base(context, dialogService, validationService)
        {
            IsMultipleRange = false;
            CategorySelected = true;
        }

        protected override void OnOkClicked(object obj)
        {
            if (SelectedRanges.Count() == 0)
            {
                HasSelectionError = true;
                ErrorText += "Keine Daten ausgewählt.";
            }
            if (CategorySelected)
            {
                CategoryColumns = SelectedRanges;
                ValueColumns = new int[] { -1 };
            }
            if (ValueSelected)
            {
                ValueColumns = SelectedRanges;
            }

            // No need to validate, if the error has already occured
            if (!HasSelectionError)
                Validate();

            base.OnOkClicked(obj);
        }

        protected override void Validate()
        {
            if (SelectedRanges.Length == 1)
            {
                if (!_validationService.ValidateCategorical(SelectedRanges[0]))
                {
                    HasSelectionError = true;
                    ErrorText += "Die angegebene Daten haben zu viele Kategorien für das Kreisdiagramm.";
                }
            }
        }

        private void SwitchSelection(string selectionName)
        {
            switch (selectionName) {
                case "category":
                    {
                        ValueColumns = SelectedRanges;
                        SelectedRanges = CategoryColumns;
                        break;
                    }
                case "value":
                    {
                        CategoryColumns = SelectedRanges;
                        SelectedRanges = ValueColumns;
                        break;
                    }
            }
        }
    }
}
