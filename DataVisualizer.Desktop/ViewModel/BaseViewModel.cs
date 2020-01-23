using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.ViewModel
{
    public abstract class BaseViewModel : BindableBase
    {
        public virtual void InitializeViewModel()
        {

        }
    }
}
