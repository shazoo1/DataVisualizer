using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;
using SciChart.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.ViewModel
{
    public class SelectPieChartDataViewModel : SelectDataViewModel
    {
        #region Bindings
        private bool _categoryValueSelected;
        public bool CategoryValueSelected
        {
            get => _categoryValueSelected;
            set
            {
                _categoryValueSelected = value;
                OnPropertyChanged("CategoryValueSelected");
            }
        }

        private bool _onlyCategorySelected;
        public bool OnlyCategorySelected
        {
            get => _onlyCategorySelected;
            set
            {
                _onlyCategorySelected = value;
                IsMultipleRange = true;
                OnPropertyChanged("OnlyCategorySelected");
            }
        }
        #endregion

        public SelectPieChartDataViewModel() : base()
        {

        }

        public SelectPieChartDataViewModel(IContext context, IDialogService dialogService, IValidationService validationService) 
            : base(context, dialogService, validationService)
        {

        }
    }
}
