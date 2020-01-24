using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DataVisualizer.Common.Enums;
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
using System.Windows.Media;

namespace DataVisualizer.Desktop.ViewModel
{
    internal class XYPlotViewModel : BasePlotViewModel
    {
        #region Bindables

        private string _chartTitle = "XY Graph";
        public string ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                _chartTitle = value;
                RaisePropertyChanged("ChartTitle");
            }
        }

        private string _xAxisTitle = "XAxis";
        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set
            {
                _xAxisTitle = value;
                RaisePropertyChanged("XAxisTitle");
            }
        }

        private string _yAxisTitle = "YAxis";
        public string YAxisTitle
        {
            get { return _yAxisTitle; }
            set
            {
                _yAxisTitle = value;
                RaisePropertyChanged("YAxisTitle");
            }
        }

        private ICommand _addPlotCommand;
        public ICommand AddPlotCommand
        {
            get { return _addPlotCommand; }
            private set { _addPlotCommand = value; }
        }
        #endregion

        public XYPlotViewModel(IContext context, IDialogService dialogService, IValidationService validationService)
            : base(context, dialogService, validationService)
        {
            VMType = Enums.VMType.XYPlotViewModel;
        }
    }
}
