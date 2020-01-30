using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;

namespace DataVisualizer.Desktop.ViewModel
{
    internal class PieChartViewModel : BasePlotViewModel
    {
        private ObservableCollection<KeyValuePair<string, double>> _segments;
        public ObservableCollection<KeyValuePair<string, double>> Segments
        {
            get => _segments;
            set
            {
                _segments = value;
                RaisePropertyChanged("Segments");
            }
        }

        public PieChartViewModel(IContext context, IDialogService dialog,IValidationService validation)
            : base(context, dialog, validation) 
        {
            VMType = Enums.VMType.PieChartViewModel;
            ChartTitle = "Pie Chart";
        }
    }
}
