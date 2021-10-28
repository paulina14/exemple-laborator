using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("^CM[0-9]$");
        public string Value { get; }

        public ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);
        public override string ToString()
        {
            return Value;
        }

        public static bool TryParseProductCode(string stringValue, out ProductCode code)
        {
            bool isValid = false;
            code = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                code = new(stringValue);
            }
            return isValid;
        }
    }
}
