using Lab1.Domain;
using System;
using System.Collections.Generic;
using static Lab1.Domain.ShoppingCart;

namespace Lab1
{
    class Program
    {
        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            var listofProds = ReadList().ToArray();
            EmptyShoppingCart unvalidCart = new(listofProds);
            IShoppingCart result = ValidatedShoppingCart(unvalidCart);
            result.Match(
                whenEmptyShoppingCart: emptyResult => emptyResult,
                whenPaidShoppingCart: paidCart => paidCart,
                whenInvalidatedShoppingCart: invalidCart => invalidCart,
                whenValidatedShoppingCart: validCart => PayShoppingCart(validCart)
                );
            var adresa = ReadValue("Introduceti adresa: ");
            Console.WriteLine("Cumparaturile se vor livra la adresa "+ adresa.ToString());
        }

        private static IShoppingCart PayShoppingCart(ValidatedShoppingCart validCart) =>
            new PaidShoppingCart(new List<ValidatedProductsCart>(), DateTime.Now);
        private static IShoppingCart ValidatedShoppingCart(EmptyShoppingCart unvalidCart) =>
            random.Next(100) > 50 ?
            new InvalidatedShoppingCart(new List<UnvalidatedShoppingCart>(), "random error")
            : new ValidatedShoppingCart(new List<ValidatedProductsCart>());

        private static List<UnvalidatedShoppingCart> ReadList()
        {
            List<UnvalidatedShoppingCart> list = new();
            //ProductAmount amount;
            //ProductCode code;
            bool state = false;
            do
            {
                var productCode = ReadValue("Codul produsului: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }
                //code = new(productCode.ToString());

                var productAmount = ReadValue("Cantitatea dorita: ");
                if (string.IsNullOrEmpty(productAmount))
                {
                    break;
                }
                //amount = new(decimal.Parse(productAmount));

                list.Add(new(productAmount, productAmount));

                var cont = ReadValue("Mai doriti produse? 1--DA, 0--NU: ");
                if (string.IsNullOrEmpty(cont))
                {
                    break;
                }
                if(string.Compare(cont, "1") == 0)
                {
                    state = true;
                }
                else if(string.Compare(cont, "0") == 0)
                {
                    state = false;
                }
            } while (state);

            return list;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
