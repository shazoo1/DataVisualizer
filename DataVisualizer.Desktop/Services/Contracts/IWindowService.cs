using DataVisualizer.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavaVisualizer.Desktop.Services.Contracts
{
    public interface IWindowService
    {
        (int x, int[] y)? SelectColumns(SelectDataViewModel model);
    }
}
