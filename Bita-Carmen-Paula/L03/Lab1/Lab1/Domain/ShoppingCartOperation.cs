using Lab1.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1.Domain.Models.ShoppingCart;

namespace Lab1.Domain
{
    public static class ShoppingCartOperation
    {
        public static Task<IShoppingCart> ValidateShoppingCart(Func<ProductCode, TryAsync<bool>> checkProductExists, UnvalidatedShoppingCart cart) =>
            cart.ProductList
                .Select(ValidateProduct(checkProductExists))
                .Aggregate(CreateEmptyValidatedProductsList().ToAsync(), ReduceValidProducts)
                .MatchAsync(
                    Right: validatedProducts => new ValidatedShoppingCart(validatedProducts),
                    LeftAsync: errorMessage => Task.FromResult((IShoppingCart)new InvalidatedShoppingCart(cart.ProductList, errorMessage))
            );

        private static Func<UnvalidatedProductsCart, EitherAsync<string, ValidatedProductsCart>> ValidateProduct(Func<ProductCode, TryAsync<bool>> checkProductExists) =>
            unvalidatedProduct => ValidateProduct(checkProductExists, unvalidatedProduct);

        private static EitherAsync<string, ValidatedProductsCart> ValidateProduct(Func<ProductCode, TryAsync<bool>> checkProductExists, UnvalidatedProductsCart unvalidatedProduct) =>
            from productCode in ProductCode.TryParseCode(unvalidatedProduct.ProductCode)
                    .ToEitherAsync(() => $"Invalid product code ({unvalidatedProduct.ProductCode})")
            from productAmount in ProductAmount.TryParseAmount(unvalidatedProduct.ProductAmount)
                    .ToEitherAsync(() => $"Invalid product amount ({unvalidatedProduct.ProductCode},{unvalidatedProduct.ProductAmount})")
            from productPrice in ProductPrice.TryParsePrice(unvalidatedProduct.ProductPrice)
                    .ToEitherAsync(() => $"Invalid product price ({unvalidatedProduct.ProductCode}, {unvalidatedProduct.ProductPrice})")
            from productExists in checkProductExists(productCode)
                    .ToEither(error => error.ToString())
            select new ValidatedProductsCart(productCode, productAmount, productPrice);

       private static Either<string, List<ValidatedProductsCart>> CreateEmptyValidatedProductsList() =>
            Right(new List<ValidatedProductsCart>());

        private static EitherAsync<string, List<ValidatedProductsCart>> ReduceValidProducts(EitherAsync<string, List<ValidatedProductsCart>> acc, EitherAsync<string, ValidatedProductsCart> next) =>
            from list in acc
            from nextProduct in next
            select list.AppendValidProduct(nextProduct);

        private static List<ValidatedProductsCart> AppendValidProduct(this List<ValidatedProductsCart> list, ValidatedProductsCart validProduct)
        {
            list.Add(validProduct);
            return list;
        }

        public static IShoppingCart CalculateFinalProducts(IShoppingCart cart) => cart.Match(
            whenEmptyShoppingCart: emptyCart => emptyCart,
            whenUnvalidatedShoppingCart: unvalidatedCart=> unvalidatedCart,
            whenInvalidatedShoppingCart: invalidatedCart=> invalidatedCart,
            whenCalculatedShoppingCart: calculatedCart=> calculatedCart,
            whenPaidShoppingCart: paidCart=> paidCart,
            whenValidatedShoppingCart: CalculateFinalPay
        );

        private static IShoppingCart CalculateFinalPay(ValidatedShoppingCart validCart) =>
            new CalculatedShoppingCart(validCart.ProductList
                    .Select(CalculateClientFinalCart)
                    .ToList()
                    .AsReadOnly());

        private static CalculatedPayment CalculateClientFinalCart(ValidatedProductsCart validCart) =>
            new CalculatedPayment(validCart.productCode, 
                validCart.productAmount,
                validCart.productPrice, 
                validCart.productPrice * validCart.productAmount);

        public static IShoppingCart PayShoppingCart(IShoppingCart cart) => cart.Match(
            whenEmptyShoppingCart: emptyCart => emptyCart,
            whenUnvalidatedShoppingCart: unvalidCart => unvalidCart,
            whenInvalidatedShoppingCart: invalidCart => invalidCart,
            whenValidatedShoppingCart: validCart => validCart,
            whenPaidShoppingCart: paidCart => paidCart,
            whenCalculatedShoppingCart: GenerateExport);

        private static IShoppingCart GenerateExport(CalculatedShoppingCart calculatedCart) =>
            new PaidShoppingCart(calculatedCart.ProductList,
                    calculatedCart.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedPayment pay) =>
            export.AppendLine($"{pay.productCode.Value}, {pay.productAmount.Value}, {pay.finalPrice.Value}");

    }
}
