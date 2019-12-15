using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Services.Contracts
{
    public interface IValidationService
    {
        bool ValidateCategorical(int column);
        bool ValidateCategorical(int[] columns);
        bool ValidateCategorical(string[] values);
        bool ValidateNumerical(int column);
        bool ValidateNumerical(int[] columns);
        bool ValidateNumerical(string[] values);
        bool ValidateOrdered(int column);
        bool ValidateOrdered(int[] columns);
        bool ValidateOrdered(double[] values);
        bool ValidateUnique(int column);
        bool ValidateUnique(int[] columns);
        bool ValidateUnique(string[] values);
    }
}
