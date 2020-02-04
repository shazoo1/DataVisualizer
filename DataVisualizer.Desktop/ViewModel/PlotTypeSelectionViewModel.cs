﻿using DataVisualizer.Desktop.Enums;
using DataVisualizer.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataVisualizer.Desktop.ViewModel
{
    class PlotTypeSelectionViewModel : BaseViewModel
    {
        #region Bindables
        private ObservableCollection<SurfaceType> _graphTypes;
        public ObservableCollection<SurfaceType> GraphTypes
        {
            get => _graphTypes;
            set => _graphTypes = value;
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get => _okCommand;
            set => _okCommand = value;
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get => _cancelCommand;
            set => _cancelCommand = value;
        }

        private SurfaceType _selectedItem;
        public SurfaceType SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }
        #endregion

        public delegate void OkButtonClicked();
        public event OkButtonClicked OnOkButtonClicked;

        public delegate void CancelButtonClicked();
        public event CancelButtonClicked OnCancelButtonClicked;

        public VMType ResultingModelType { get; private set; }
        private Dictionary<string, VMType> SurfaceToViewModelMap { get; set; }

        public PlotTypeSelectionViewModel() : base()
        {
            _graphTypes = new ObservableCollection<SurfaceType>();
            _graphTypes.Add(new SurfaceType("XY Grafik", VMType.XYPlotViewModel));
            _graphTypes.Add(new SurfaceType("Kreisdiagramm", VMType.PieChartViewModel));

            OkCommand = new RelayCommand(OkButtonClick);
            CancelCommand = new RelayCommand(CancelButtonClick);
        }

        //A class for displaying in a list. It is not needed anywhere in code.
        public class SurfaceType
        {
            public SurfaceType(string label, VMType vmType)
            {
                Label = label;
                UnderlyingVmType = vmType;
            }
            public string Label { get; set; }
            public VMType UnderlyingVmType { get; set; }
        }

        private void OkButtonClick(object obj)
        {
            OnOkButtonClicked.Invoke();
        }

        private void CancelButtonClick(object obj)
        {
            OnCancelButtonClicked.Invoke();
        }
    }
}
