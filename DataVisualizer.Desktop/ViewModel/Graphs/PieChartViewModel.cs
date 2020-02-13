using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abt.Controls.SciChart;
using DataVisualizer.Desktop.Helpers;
using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using DavaVisualizer.Desktop.Services.Contracts;

namespace DataVisualizer.Desktop.ViewModel
{
    internal class PieChartViewModel : BaseGraphViewModel
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
            SurfaceType = Enums.SurfaceType.PieChartSurface;
            ChartTitle = "Pie Chart";
        }

        protected override void AddSeries (object obj)
        {
            var selectionModel = new SelectPieChartDataViewModel(_context, _dialogService, _validationService);
            var selection = _dialogService.SelectPieChartData(selectionModel).GetValueOrDefault();
            if (_validationService.ValidateCategorical(selection.categories))
            {
                var categories = _context.GetTextualColumnByIndex(selection.categories);
                if (selection.values != -1 && _validationService.ValidateNumerical(selection.values))
                {
                    var values = _context.GetNumericalColumnByIndex(selection.values);
                    Segments = BuildPieChartSegments(categories, values);
                }
                else
                {
                    Segments = BuildPieChartSegments(categories);
                }
            }
        }

        public ObservableCollection<KeyValuePair<string, double>> BuildPieChartSegments(string[] categories)
        {
            var aggregation = categories.GroupBy(x => x)
                .Select(val => new Tuple<string, double>(val.Key, val.Count()));
            var preparedCategories = aggregation.Select(x => x.Item1).ToArray();
            var preparedValues = aggregation.Select(x => x.Item2).ToArray();

            return BuildPieChartSegments(preparedCategories, preparedValues);
        }

        public ObservableCollection<KeyValuePair<string, double>> BuildPieChartSegments(string[] categories, double[] values)
        {
            var result = new ObservableCollection<KeyValuePair<string, double>>();
            for (int i = 0; i < categories.Length; i++)
            {
                result.Add(new KeyValuePair<string, double>(categories[i], values[i]));
            }
            return result;
        }
    }
}
