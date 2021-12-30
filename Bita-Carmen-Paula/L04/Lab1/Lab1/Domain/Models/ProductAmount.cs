using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

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


        public static Option<ProductAmount> TryParseAmount(string amountString)
        {
            if(decimal.TryParse(amountString, out decimal numericAmount) && IsValid(numericAmount))
            {
                return Some<ProductAmount>(new(numericAmount));
            }
            else
            {
                return None;
            }
        }
        private static bool IsValid(decimal numericAmount) => numericAmount > 0 && numericAmount < 10;

    }
}
