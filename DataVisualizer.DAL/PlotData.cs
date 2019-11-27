using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVisualizer.Common.Enums;

namespace DataVisualizer.DAL
{
    public class PlotData
    {
        public string[] Header { get; }
        public List<string> Rows { get; }
        public CellType[] CellTypes { get; } 
    }
}
