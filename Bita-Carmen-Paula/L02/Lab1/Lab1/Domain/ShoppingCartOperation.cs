using Lab1.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Lab1.Domain.Models.ShoppingCart;

namespace Lab1.Domain
{
    public static class ShoppingCartOperation
    {
        public static IShoppingCart ValidateShoppingCart(Func<ProductCode, bool> checkProductExists, UnvalidatedShoppingCart cart)
        {
            List<ValidatedProductsCart> validatedCart = new();
            bool isValidList = true;
            string invalidReason = string.Empty;
            foreach(var unvalidatedProduct in cart.ProductList)
            {
                if(!ProductAmount.TryParseAmount(unvalidatedProduct.ProductAmount, out ProductAmount amount))
                {
                    invalidReason = $"Invalid amount ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductAmount})";
                    isValidList = false;
                    break;
                }

                if(!ProductPrice.TryParsePrice(unvalidatedProduct.ProductPrice, out ProductPrice price))
                {
                    invalidReason = $"Invalid product price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductPrice})";
                    isValidList = false;
                    break;
                }
                if(!ProductCode.TryParseProductCode(unvalidatedProduct.ProductCode, out ProductCode code) && checkProductExists(code))
                {
                    invalidReason = $"Invalid product code({unvalidatedProduct.ProductCode})";
                    isValidList = false;
                    break;
                }
                ValidatedProductsCart validCart = new(code, amount, price);
                validatedCart.Add(validCart);
            }

            if (isValidList)
            {
                return new ValidatedShoppingCart(validatedCart);
            }
            else
            {
                return new InvalidatedShoppingCart(cart.ProductList, invalidReason);
            }
        }

        public static IShoppingCart CalculateFinalPayment(IShoppingCart cart) => cart.Match(
            whenEmptyShoppingCart: emptyCart => emptyCart,
            whenUnvalidatedShoppingCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedShoppingCart: invalidatedCart => invalidatedCart,
            whenCalculatedShoppingCart: calculatedCart => calculatedCart,
            whenPaidShoppingCart: paidCart => paidCart,
            whenValidatedShoppingCart: validatedCart =>
            {
                var calculatedList = validatedCart.ProductList.Select(validCart =>
                                              new CalculatedPayment(validCart.productCode,
                                                                    validCart.productAmount,
                                                                    validCart.productPrice));
                return new CalculatedShoppingCart(calculatedList.ToList().AsReadOnly());
            });

        public static IShoppingCart PayCart(IShoppingCart cart) => cart.Match(
            whenEmptyShoppingCart: emptyCart => emptyCart,
            whenUnvalidatedShoppingCart: unvalidatedCart => unvalidatedCart,
            whenInvalidatedShoppingCart: invalidatedCart => invalidatedCart,
            whenValidatedShoppingCart: validatedCart => validatedCart,
            whenPaidShoppingCart: paidCart => paidCart,
            whenCalculatedShoppingCart: calculatedCart =>
            {
                StringBuilder csv = new();
                calculatedCart.ProductList.Aggregate(csv, (export, list) => export.AppendLine($"{list.productCode.Value}, {list.productAmount.Value}, {list.productPrice.Value}"));

                PaidShoppingCart paidShoppingCart = new(calculatedCart.ProductList, csv.ToString(), DateTime.Now);

                return paidShoppingCart;
            }
            );
    }
}
