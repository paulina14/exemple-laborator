using Lab1.Domain.Models;
using System;
using static Lab1.Domain.Models.CartPaidEvent;
using static Lab1.Domain.Models.ShoppingCart;
using static Lab1.Domain.ShoppingCartOperation;

namespace Lab1.Domain
{
    public class PayCartWorkflow
    {
        public ICartPaidEvent Execute(PayShopppingCartCommand command, Func<ProductCode, bool> checkProductExists)
        {
            UnvalidatedShoppingCart unvalidCart = new UnvalidatedShoppingCart(command.InputCart);
            IShoppingCart cart = ValidateShoppingCart(checkProductExists, unvalidCart);
            cart = CalculateFinalPayment(cart);
            cart = PayCart(cart);

            return cart.Match(
                whenEmptyShoppingCart: emptyCart => new CartPaidFailEvent("Empty cart"),
                whenUnvalidatedShoppingCart: unvalidatedCart => new CartPaidFailEvent("Unexepected unvalidated state") as ICartPaidEvent,
                whenInvalidatedShoppingCart: invalidatedCart => new CartPaidFailEvent(invalidatedCart.Reason),
                whenValidatedShoppingCart: validatedCart => new CartPaidFailEvent("Unexpected validated state"),
                whenCalculatedShoppingCart: calculatedCart => new CartPaidFailEvent("Unexpected calculated state"),
                whenPaidShoppingCart: paidCart => new CartPaidSuccededEvent(paidCart.Csv, paidCart.PaymentDate)
                );
        }
    }
}
