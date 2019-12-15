﻿using DataVisualizer.Desktop.Services.Contracts;
using DataVisualizer.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Desktop.Services.Classes
{
    internal class ValidationService : IValidationService
    {
        private IContext _context;

        public ValidationService(IContext context)
        {
            _context = context;
        }

        #region Validation itself
        public bool ValidateCategorical(int column)
        {
            throw new NotImplementedException();
        }
        public bool ValidateCategorical(int[] column)
        {
            throw new NotImplementedException();
        }
        public bool ValidateCategorical(string[] values)
        {
            // TODO :: Implement
            return true;
        }

        public bool ValidateNumerical(int column)
        {
            try
            {
                _context.GetNumericalColumnByIndex(column);
            }
            catch (FormatException e)
            {
                return false;
            }
            return true;
        }
        public bool ValidateNumerical(int[] columns)
        {
            foreach (var column in columns)
            {
                try
                {
                    _context.GetNumericalColumnByIndex(column);
                }
                catch (FormatException e)
                {
                    return false;
                }
            }
            return true;
        }
        public bool ValidateNumerical(string[] values)
        {
            return values.Any(x => (double.TryParse(x, out double i)));
        }

        public bool ValidateOrdered(int column)
        {
            return ValidateOrdered(_context.GetNumericalColumnByIndex(column));
        }
        public bool ValidateOrdered(int[] columns)
        {
            foreach (var column in columns)
            {
                if (!ValidateOrdered(_context.GetNumericalColumnByIndex(column)))
                    return false;
            }
            return true;
        }
        public bool ValidateOrdered(double[] values)
        {
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < values[i - 1])
                    return false;
            }
            return true;
        }

        public bool ValidateUnique(int column)
        {
            return ValidateUnique(_context.GetTextualColumnByIndex(column));
        }
        public bool ValidateUnique(int[] columns)
        {
            foreach (var column in columns)
            {
                if (!ValidateUnique(_context.GetTextualColumnByIndex(column)))
                    return false;
            }
            return true;
        }
        public bool ValidateUnique(string[] values)
        {
            return values.Length == values.Distinct().Count();
        }
        #endregion
    }
}
