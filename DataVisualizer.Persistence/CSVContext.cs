using CsvHelper;
using DataVisualizer.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataVisualizer.Persistence
{
    public class CSVContext : ICSVContext
    {
        private string _fileName;
        Char Quote { get; set; } = '"';
        private DataTable _data;

        public CSVContext()
        {
            _data = new DataTable();
        }

        public CSVContext(string filePath ) 
        {
            this._fileName = filePath;

            _data = new DataTable();

            ReadLines();
        }

        public DataTable ReadData(string connectionString)
        {
            _fileName = connectionString;
            ReadLines();
            return _data ?? new DataTable();
        }

        private void ReadLines()
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader))
            {
                // TODO :: Log
                csv.Configuration.BadDataFound = null;
                csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                try
                {
                    using (var csvDataReader = new CsvDataReader(csv))
                    {
                        _data.Load(csvDataReader);
                    }
                }
                catch (Exception)
                {
                    // TODO :: Alert or smt
                }
            }
        }

        public DataTable GetFirstLines(int count)
        {
            if (_data == null)
            {
                ReadLines();
            }
            return _data.Rows.Cast<System.Data.DataRow>().Take(count).CopyToDataTable() ?? new DataTable();
        }

        public double[] GetNumericalColumnByIndex(int index)
        {
            var result = new List<double>();

            var column = from DataRow row in _data.Rows
                         select (row[index].ToString());
            foreach (var value in column)
            {
                //Set all cells with invalid data to 0
                if (double.TryParse(value, out double doubleCell))
                    result.Add(doubleCell);
                else throw new FormatException(string.Format("Error while reading column {0}: {1} is not recognized as number.", index, value));
            }

            return result.ToArray();
        }

        public string[] GetTextualColumnByIndex(int index)
        {
            return (from DataRow row in _data.Rows
                    select (string)row[index]).ToArray();
        }
    }
}
