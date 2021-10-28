using Lab1.Domain;
using Lab1.Domain.Models;
using System;
using System.Collections.Generic;
using static Lab1.Domain.Models.ShoppingCart;

namespace Lab1
{
    class Program
    {
        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            var listofProds = ReadList().ToArray();
            PayShopppingCartCommand command = new(listofProds);
            PayCartWorkflow workflow = new PayCartWorkflow();
            var result = workflow.Execute(command, (productCode) => true);

            result.Match(
                whenCartPaidFailEvent: @event =>
                {
                    Console.WriteLine($"PAyment failed: {@event.Reason}");
                    return @event;
                },
                whenCartPaidSuccededEvent: @event =>
                {
                    Console.WriteLine($"Payment succeded.");
                    Console.WriteLine(@event.Csv);
                    return @event;
                }
                );


            var adresa = ReadValue("Introduceti adresa: ");
            Console.WriteLine("Cumparaturile se vor livra la adresa "+ adresa.ToString());
        }
 

        private static List<UnvalidatedProductsCart> ReadList()
        {
            List<UnvalidatedProductsCart> list = new();

            bool state = false;
            do
            {
                var productCode = ReadValue("Codul produsului: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }
              

                var productAmount = ReadValue("Cantitatea dorita: ");
                if (string.IsNullOrEmpty(productAmount))
                {
                    break;
                }

                var productPrice = ReadValue("Pretul produsului ");
                if (string.IsNullOrEmpty(productPrice))
                {
                    break;
                }

                list.Add(new(productAmount, productAmount, productPrice));

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
