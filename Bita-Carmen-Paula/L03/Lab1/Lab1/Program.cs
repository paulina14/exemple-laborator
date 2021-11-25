using Lab1.Domain;
using Lab1.Domain.Models;
using System;
using System.Collections.Generic;
using static Lab1.Domain.Models.ShoppingCart;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Lab1
{
    class Program
    {
        private static readonly Random random = new Random();
        static async Task Main(string[] args)
        {
            var listofProds = ReadList().ToArray();
            PayShopppingCartCommand command = new(listofProds);
            PayCartWorkflow workflow = new();
            var result = await workflow.ExecuteAsync(command, CheckProductExists);



            result.Match(
                whenCartPaidFailEvent: @event =>
                {
                    Console.WriteLine($"Payment failed: {@event.Reason}");
                    return @event;
                },
                whenCartPaidSuccededEvent: @event =>
                {
                    Console.WriteLine($"Payment succeded.");
                    Console.WriteLine(@event.Csv);
                    return @event;
                }
                );


            /*var adresa = ReadValue("Introduceti adresa: ");
            Console.WriteLine("Cumparaturile se vor livra la adresa "+ adresa.ToString());*/
        }
 

        private static int Sum()
        {
            Option<int> two = Some(2);
            Option<int> four = Some(4);
            Option<int> six = Some(6);
            Option<int> none = None;

            var result = from x in two
                         from y in four
                         from z in six
                         from n in none
                         select x + y + z + n;
            // This expression succeeds because all items to the right of 'in' are Some of int
            // and therefore it lands in the Some lambda.
            int r = match(result,
                           Some: v => v * 2,
                           None: () => 0);     // r == 24

            return r;
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

                list.Add(new(productCode, productAmount, productPrice));

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

        private static TryAsync<bool> CheckProductExists(ProductCode code)
        {
            Func<Task<bool>> func = async () =>
            {
                //HttpClient client = new HttpClient();

                //var response = await client.PostAsync($"www.university.com/checkRegistrationNumber?number={student.Value}", new StringContent(""));

                //response.EnsureSuccessStatusCode(); //200

                return true;
            };
            return TryAsync(func);
        }
    }
}
