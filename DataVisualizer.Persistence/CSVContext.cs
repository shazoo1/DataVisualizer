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
        Char Separator { get; set; } = ',';
        Char Quote { get; set; } = '"';
        bool ContainsHeader { get; set; }
        private DataTable _data;

        public CSVContext(string filePath, Char separator, bool containsHeader ) 
        {
            this._fileName = filePath;
            this.Separator = separator;
            this.ContainsHeader = containsHeader;

            _data = new DataTable();

            ReadLines();
        }

        public CSVContext(string filePath, Char separator, Char quote, bool containsHeader)
        {
            this._fileName = filePath;
            this.Separator = separator;
            this.ContainsHeader = containsHeader;
            Quote = quote;

            _data = new DataTable();

            ReadLines();
        }

        public void ReadLines()
        {
            List<string[]> lines = new List<string[]>();
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = ContainsHeader;
                using (var csvDataReader = new CsvDataReader(csv))
                {
                    _data.Load(csvDataReader);
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
            try
            {
                return (from DataRow row in _data.Rows
                        select double.Parse((string)row[index])).ToArray();
            } 
            catch (FormatException e)
            {
                // TODO :: Log format exception
            }
            return null;
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public void SetConnectionString(string connectionString)
        {
            _fileName = connectionString;
        }
    }
}
