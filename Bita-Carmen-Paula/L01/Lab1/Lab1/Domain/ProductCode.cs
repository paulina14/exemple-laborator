using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("^CO[0-9]{7}$");
        public string Value { get; }

        public ProductCode(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
