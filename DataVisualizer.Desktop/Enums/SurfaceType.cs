using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Enums
{
    public enum SurfaceType
    {
        //For common charts, like Line, Column and so on
        XYPlotSurface,
        //Pie chart surface. There is no PieChart in SciChart v3
        PieChartSurface,
        // For vertical charts, like BarChart
        VerticalSurface
    }
}
