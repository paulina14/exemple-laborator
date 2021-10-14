using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    [AsChoice]
    public static partial class ShoppingCart
    {
        public interface IShoppingCart { }

        public record EmptyShoppingCart(IReadOnlyCollection<UnvalidatedShoppingCart> ProductsList) : IShoppingCart;

        public record InvalidatedShoppingCart(IReadOnlyCollection<UnvalidatedShoppingCart> ProductsList, string reason) : IShoppingCart;

        public record ValidatedShoppingCart(IReadOnlyCollection<ValidatedProductsCart> ProductsList) : IShoppingCart;

        public record PaidShoppingCart(IReadOnlyCollection<ValidatedProductsCart> ProductsList, DateTime PaymentDate) : IShoppingCart;
    }
}
