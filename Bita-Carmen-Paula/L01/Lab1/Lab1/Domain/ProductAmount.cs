using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    public record ProductAmount
    {
        public decimal Value { get; }

        public ProductAmount(decimal value)
        {
            if (value > 0 && value <= 5)
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductAmountException($"{value:0.##} is an invalid amount value.");
            }
        }

        public ProductAmount Round()
        {
            var roundedValue = Math.Round(Value);
            return new ProductAmount(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
    }
}
