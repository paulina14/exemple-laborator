using Lab1.Domain.Models;
using Lab1.Domain.Repositories;
using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Lab1.Domain.Models.CartPaidEvent;
using static Lab1.Domain.Models.ShoppingCart;
using static Lab1.Domain.ShoppingCartOperation;
using static LanguageExt.Prelude;

namespace Lab1.Domain
{
    public class PayCartWorkflow
    {
        private readonly IOrderLineRepository orderLineRepo;
        private readonly IProductsRepository productsRepo;
        private readonly ILogger<PayCartWorkflow> logger;

        public PayCartWorkflow(IOrderLineRepository orderLineRepo, IProductsRepository productsRepo, ILogger<PayCartWorkflow> logger)
        {
            this.orderLineRepo = orderLineRepo;
            this.productsRepo = productsRepo;
            this.logger = logger;
        }

        public async Task<ICartPaidEvent> ExecuteAsync(PayShopppingCartCommand command)
        {
            UnvalidatedShoppingCart unvalidCart = new UnvalidatedShoppingCart(command.InputCart);

            var result = from products in productsRepo.TryGetExistingProducts(unvalidCart.ProductList.Select(prod => prod.ProductCode))
                            .ToEither(ex => new FailedCart(unvalidCart.ProductList, ex) as IShoppingCart)
                         from productAmount in productsRepo.TryGetExistingAmount(unvalidCart.ProductList.Select(prod => prod.ProductCode))
                            .ToEither(ex => new FailedCart(unvalidCart.ProductList, ex) as IShoppingCart)
                         from existingOrders in orderLineRepo.TryGetExistingProducts()
                             .ToEither(ex => new FailedCart(unvalidCart.ProductList, ex) as IShoppingCart)
                         let checkProductExists = (Func<ProductCode, Option<ProductCode>>)(product => CheckProductExists(products, product))
                         let checkAmountExists = (Func<ProductAmount, Option<ProductAmount>>)(amount => CheckAmountExists(productAmount, amount))
                         from paidCart in ExecuteWorkflowAsync(unvalidCart, existingOrders, checkProductExists, checkAmountExists).ToAsync()
                         from _ in orderLineRepo.TrySaveProducts(paidCart)
                             .ToEither(ex => new FailedCart(unvalidCart.ProductList, ex) as IShoppingCart)
                         select paidCart;

            return await result.Match(
                    Left: orders => GenerateFailedEvent(orders) as ICartPaidEvent,
                    Right: paidOrder => new CartPaidSuccededEvent(paidOrder.Csv, paidOrder.PaymentDate)
                    );
        }

        private async Task<Either<IShoppingCart, PaidShoppingCart>> ExecuteWorkflowAsync(UnvalidatedShoppingCart unvalidCart, IEnumerable<CalculatedPayment> existingOrders, Func<ProductCode, Option<ProductCode>> checkProductExists, Func<ProductAmount, Option<ProductAmount>> checkAmountExists)
        {
            IShoppingCart cart = await ValidateShoppingCart(checkProductExists, checkAmountExists, unvalidCart);
            cart = CalculateFinalProducts(cart);
            cart = MergeProducts(cart, existingOrders);
            cart = PayShoppingCart(cart);

            return cart.Match<Either<IShoppingCart, PaidShoppingCart>>(
                whenEmptyShoppingCart: emptyCart => Left(emptyCart as IShoppingCart),
                whenUnvalidatedShoppingCart: unvalidCart => Left(unvalidCart as IShoppingCart),
                whenInvalidatedShoppingCart: invalidCart => Left(invalidCart as IShoppingCart),
                whenFailedCart: failCart => Left(failCart as IShoppingCart),
                whenValidatedShoppingCart: validCart => Left(validCart as IShoppingCart),
                whenCalculatedShoppingCart: calculatedCart => Left(calculatedCart as IShoppingCart),
                whenPaidShoppingCart: paidCart => Right(paidCart)
                );
        }


        private Option<ProductCode> CheckProductExists(IEnumerable<ProductCode> products, ProductCode code)
        {
            if(products.Any(s=>s == code))
            {
                return Some(code);
            }
            else
            {
                return None;
            }
        }

        private Option<ProductAmount> CheckAmountExists(IEnumerable<ProductAmount> productAmount, ProductAmount amount)
        {
            /*if (productAmount.Any(s => amount.Value <= s.Value))
            {
                return Some(amount);
            }
            else
            {
                return None;
            }*/
            if(productAmount.Any(s => s.Value >= amount.Value))
            {
                return Some(amount);
            }
            else
            {
                return None;
            }
        }

        private CartPaidFailEvent GenerateFailedEvent(IShoppingCart cart) =>
            cart.Match<CartPaidFailEvent>(
                whenEmptyShoppingCart: emptyCart => new($"Invalid state {nameof(EmptyShoppingCart)}"),
                whenUnvalidatedShoppingCart: unvalidCart => new($"Invalid state {nameof(UnvalidatedShoppingCart)}"),
                whenInvalidatedShoppingCart: invalidCart => new(invalidCart.Reason),
                whenValidatedShoppingCart: validCart => new($"Invalid state {nameof(ValidatedShoppingCart)}"),
                whenFailedCart: failCart =>
                    {
                        logger.LogError(failCart.Exception, failCart.Exception.Message);
                        return new(failCart.Exception.Message);
                    },
                whenCalculatedShoppingCart: calculatedCart => new($"Invalid state {nameof(CalculatedShoppingCart)}"),
                whenPaidShoppingCart: paidCart => new($"Invalid state {nameof(PaidShoppingCart)}"));
    }
}
