using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record ProductAmount
    {
        public decimal Value { get; }

        public ProductAmount(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductAmountException($"{value:0.##} is an invalid amount value.");
            }
        }

        public static ProductAmount operator +(ProductAmount a, ProductAmount b) => new ProductAmount((a.Value + b.Value) / 2m);

        public ProductAmount Round()
        {
            var roundedValue = Math.Round(Value);
            return new ProductAmount(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static bool TryParseAmount(string amountString, out ProductAmount amount)
        {
            bool isValid = false;
            amount = null;
            if (decimal.TryParse(amountString, out decimal numericAmount))
            {
                if (IsValid(numericAmount))
                {
                    isValid = true;
                    amount = new(numericAmount);
                }
            }
            return isValid;
        }

        private static bool IsValid(decimal numericAmount) => numericAmount > 0 && numericAmount < 6;
    }
}
