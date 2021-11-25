using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record PayShopppingCartCommand
    {
        public PayShopppingCartCommand(IReadOnlyCollection<UnvalidatedProductsCart> inputCart)
        {
            InputCart = inputCart;
        }

        public IReadOnlyCollection<UnvalidatedProductsCart> InputCart { get; }
    }
}
