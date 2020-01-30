using DataVisualizer.Desktop.Helpers;
using Dragablz;
using Dragablz.Dockablz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.ViewModel
{
    class TabHostViewModel
    {
        #region Bindables
        private readonly IInterTabClient _interTabClient;
        public IInterTabClient InterTabClient
        {
            get { return _interTabClient; }
        }
        
        private readonly object _partition;
        public object Partition
        {
            get { return _partition; }
        }
        #endregion

        #region Constructors
        public TabHostViewModel(IInterTabClient interTabClient, object partition)
        {
            _interTabClient = interTabClient;
            _partition = partition;
        }
        //public TabHostViewModel()
        //{
        //    _items = new ObservableCollection<HeaderedItemViewModel>();
        //}

        //public TabHostViewModel(params HeaderedItemViewModel[] items)
        //{
        //    _items = new ObservableCollection<HeaderedItemViewModel>(items);
        //}
        #endregion

        
    }
}
