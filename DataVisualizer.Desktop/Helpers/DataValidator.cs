using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Helpers
{
    public class DataValidator
    {
        public bool ValidateUnique(double[] values)
        {
            return values.Length == values.Distinct().Count();
        }

        public bool ValidateNumerical(string[] values)
        {
            return values.Any(x => (double.TryParse(x, out double i)));
        }

        public bool ValidateCategorical(string[] values)
        {
            // TODO :: Implement
            return true;
        }
    }
}
