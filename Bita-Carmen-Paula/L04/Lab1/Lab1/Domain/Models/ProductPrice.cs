using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record ProductPrice
    {
        public decimal Value { get;}

        public ProductPrice(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductPriceException($"{value:0.##} is an invalid price.");
            }
        }
        public static ProductPrice operator *(ProductPrice a, ProductAmount b) => new ProductPrice(a.Value * b.Value);

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        private static bool IsValid(decimal floatPrice) => floatPrice > 0 && floatPrice <= 60000;

        public static Option<ProductPrice> TryParsePrice(string priceString)
        {
            if(decimal.TryParse(priceString, out decimal numericPrice) && IsValid(numericPrice))
            {
                return Some<ProductPrice>(new(numericPrice));
            }
            else
            {
                return None;
            }
        }
    }
}
