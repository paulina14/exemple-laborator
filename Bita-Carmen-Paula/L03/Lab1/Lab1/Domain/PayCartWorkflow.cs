using Lab1.Domain.Models;
using LanguageExt;
using System;
using System.Threading.Tasks;
using static Lab1.Domain.Models.CartPaidEvent;
using static Lab1.Domain.Models.ShoppingCart;
using static Lab1.Domain.ShoppingCartOperation;

namespace Lab1.Domain
{
    public class PayCartWorkflow
    {

        public async Task<ICartPaidEvent> ExecuteAsync(PayShopppingCartCommand command, Func<ProductCode, TryAsync<bool>> checkProductExists)
        {
            UnvalidatedShoppingCart unvalidCart = new UnvalidatedShoppingCart(command.InputCart);
            IShoppingCart cart = await ValidateShoppingCart(checkProductExists, unvalidCart);
            cart = CalculateFinalProducts(cart);
            cart = PayShoppingCart(cart);

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
