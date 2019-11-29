using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataVisualizer.Persistence
{
    public class CSVContext
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

        public void ReadLines()
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader))
            {
                // TODO :: Log
                csv.Configuration.BadDataFound = null;
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

        public string[] GetStringColumnByIndex(int index)
        {
            return (from DataRow row in _data.Rows
                    select (string)row[index]).ToArray();
        }

        public double[] GetNumericalColumnByIndex(int index)
        {
            var result = new List<double>();

            var column = from DataRow row in _data.Rows
                         select (row[index].ToString());
            foreach (var value in column)
            {
                //Set all cells with invalid data to 0
                if (double.TryParse(value, out double doubleCell)) { result.Add(doubleCell); }
                else result.Add(0);
            }

            return result.ToArray();
        }

        public string[] GetTextualColumnByIndex(int index)
        {
            return (from DataRow row in _data.Rows
                    select (string)row[index]).ToArray();
        }

        public void ReadFile(string filepath)
        {
            _fileName = filepath;
            _data = new DataTable();
            ReadLines();
        }
    }
}
