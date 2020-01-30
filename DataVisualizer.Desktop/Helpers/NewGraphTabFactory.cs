using Dragablz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Helpers
{
    class NewGraphTabFactory
    {
        public static Func<HeaderedItemViewModel> Factory
        {
            get
            {
                return
                    () =>
                    {
                        return new HeaderedItemViewModel()
                        {
                            Header = "New graph"
                        };
                    };
            }
        }
    }
}
