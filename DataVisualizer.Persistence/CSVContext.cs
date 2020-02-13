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
        private string _delimiter = ";";
        private bool _header = false;
        Char Quote { get; set; } = '"';
        private DataTable _data;
        public bool IsConnected
        {
            get;
            private set;
        }

        public CSVContext()
        {
            _data = new DataTable();
        }

        public CSVContext(string filePath) 
        {
            this._fileName = filePath;

            _data = new DataTable();

            ReadLinesFromFile(_delimiter, _header);
        }

        public DataTable ReadData(string connectionString)
        {
            _fileName = connectionString;
            ReadLinesFromFile(_delimiter, _header);
            return _data ?? new DataTable();
        }
        public DataTable ReadData(string delimiter, bool header, string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
                _fileName = connectionString;

            _delimiter = delimiter;
            _header = header;

            ReadLinesFromFile(_delimiter, _header);
            return _data ?? new DataTable();

        }

        private void ReadLinesFromFile(string delimiter, bool header)
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader))
            {
                // TODO :: Handle bad data
                csv.Configuration.BadDataFound = null;
                csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                csv.Configuration.Delimiter = delimiter;
                //A CSVHelper bug: https://github.com/JoshClose/CsvHelper/issues/1240
                csv.Configuration.HasHeaderRecord = true;
                
                DataTable data = new DataTable();

                if (header)
                {
                    using (CsvDataReader csvDataReader = new CsvDataReader(csv))
                    {
                        data.Load(csvDataReader);
                    }
                }
                else
                {
                    bool createFakeHeader = true;
                    while (csv.Read())
                    {
                        if (createFakeHeader)
                        {
                            for (int i = 0; i < csv.Context.Record.Length; i++)
                                data.Columns.Add(string.Format("Column {0}", i.ToString()));
                        }
                        createFakeHeader = false;

                        DataRow row = data.NewRow();
                        for (int i = 0; i < csv.Context.Record.Length; i++)
                            try
                            {
                                row[i] = csv.Context.Record[i];
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                row.RowError = "Parsed row contains more columns than existing.";
                            }
                        data.Rows.Add(row);
                    }
                }
                _data = data;
                IsConnected = true;
            }
        }

        private void ReadLinesFromFile()
        {
            using (var reader = new StreamReader(_fileName))
            using (var csv = new CsvReader(reader))
            {
                // TODO :: Log
                csv.Configuration.BadDataFound = null;
                csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
                csv.Configuration.Delimiter = ",";
                try
                {
                    using (var csvDataReader = new CsvDataReader(csv))
                    {
                        _data.Load(csvDataReader);
                    }
                    IsConnected = true;
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
                ReadLinesFromFile();
            }
            if (_data.Rows.Count > 0)
                return _data.Rows.Cast<System.Data.DataRow>().Take(count).CopyToDataTable();
            else
                return new DataTable();
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
