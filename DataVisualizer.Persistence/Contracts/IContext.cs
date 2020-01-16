using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Persistence.Contracts
{
    public interface IContext
    {
        DataTable ReadData(string connectionString);
        DataTable ReadData(string delimiter, bool header, string connectionString);
        DataTable GetFirstLines(int count);
        string[] GetTextualColumnByIndex(int index);
        double[] GetNumericalColumnByIndex(int index);
        bool IsConnected { get; }
    }
}
