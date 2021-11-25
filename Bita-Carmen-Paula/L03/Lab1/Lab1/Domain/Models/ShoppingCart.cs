using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    [AsChoice]
    public static partial class ShoppingCart
    {
        public interface IShoppingCart { }

        public record EmptyShoppingCart : IShoppingCart
        {

        }

        public record UnvalidatedShoppingCart : IShoppingCart
        {
            public UnvalidatedShoppingCart(IReadOnlyCollection<UnvalidatedProductsCart> productList)
            {
                ProductList = productList;
            }

            public IReadOnlyCollection<UnvalidatedProductsCart> ProductList { get; }
        }

        public record InvalidatedShoppingCart : IShoppingCart
        {
            internal InvalidatedShoppingCart(IReadOnlyCollection<UnvalidatedProductsCart> productList, string reason)
            {
                ProductList = productList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedProductsCart> ProductList { get; }
            public string Reason { get; }
        }

        public record ValidatedShoppingCart : IShoppingCart
        {
            internal ValidatedShoppingCart(IReadOnlyCollection<ValidatedProductsCart> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<ValidatedProductsCart> ProductList { get; }
        }

        public record CalculatedShoppingCart : IShoppingCart
        {
            internal CalculatedShoppingCart(IReadOnlyCollection<CalculatedPayment> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<CalculatedPayment> ProductList { get; }
        }

        public record PaidShoppingCart : IShoppingCart
        {
            internal PaidShoppingCart(IReadOnlyCollection<CalculatedPayment> productList, string csv, DateTime paymentDate)
            {
                ProductList = productList;
                PaymentDate = paymentDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedPayment> ProductList { get; }
            public DateTime PaymentDate { get; }
            public string Csv { get; }
        }

    }
}
