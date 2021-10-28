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

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static bool TryParsePrice(string priceString, out ProductPrice price)
        {
            bool isValid = false;
            price = null;
            if (decimal.TryParse(priceString, out decimal floatPrice))
            {
                if (IsValid(floatPrice))
                {
                    isValid = true;
                    price = new(floatPrice);
                }
            }
            return isValid;
        }

        private static bool IsValid(decimal floatPrice) => floatPrice > 0 && floatPrice <= 60000;
    }
}
