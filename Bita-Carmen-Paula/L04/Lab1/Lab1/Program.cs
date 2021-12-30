using Lab1.Domain;
using Lab1.Domain.Models;
using System;
using System.Collections.Generic;
using static Lab1.Domain.Models.ShoppingCart;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Lab1.Data;
using Lab1.Data.Repositories;

namespace Lab1
{
    class Program
    {

        private static readonly Random random = new Random();
        private static string ConnectionString = "Server=(LocalDB)\\SQL_Demo;Database=PSSC;Trusted_Connection=True;MultipleActiveResultSets=true";

        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactor = ConfigureLoggerFactory();
            ILogger<PayCartWorkflow> logger = loggerFactor.CreateLogger<PayCartWorkflow>();

            var listofProd = ReadList().ToArray();
            PayShopppingCartCommand command = new(listofProd);
            var dbContextBuilder = new DbContextOptionsBuilder<CartsContext>()
                    .UseSqlServer(ConnectionString)
                    .UseLoggerFactory(loggerFactor);

            CartsContext cartContext = new CartsContext(dbContextBuilder.Options);
            ProductsRepository productsRepo = new(cartContext);
            OrdersRepository orderRepo = new(cartContext);
            PayCartWorkflow workflow = new(orderRepo, productsRepo, logger);
            var result = await workflow.ExecuteAsync(command);

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

        }

        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options => {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss";
                })
                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }

        private static List<UnvalidatedProductsCart> ReadList()
        {
            List<UnvalidatedProductsCart> list = new();
            var productCode = string.Empty;
            var productAmount = string.Empty;
            var productPrice = string.Empty;
            var clientAdress = string.Empty;

            bool state = false;
            do
            {
                productCode = ReadValue("Codul produsului: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }


                productAmount = ReadValue("Cantitatea dorita: ");
                if (string.IsNullOrEmpty(productAmount))
                {
                    break;
                }

                productPrice = ReadValue("Pretul produsului ");
                if (string.IsNullOrEmpty(productPrice))
                {
                    break;
                }

                var cont = ReadValue("Mai doriti produse? 1--DA, 0--NU: ");
                if (string.IsNullOrEmpty(cont))
                {   
                    break;
                }
                if (string.Compare(cont, "1") == 0)
                {
                    state = true;
                }
                else if (string.Compare(cont, "0") == 0)
                {
                    state = false;
                }

                clientAdress = ReadValue("Introduceti adresa: ");
                if (string.IsNullOrEmpty(clientAdress))
                {
                    break;
                }
            } while (state);

            list.Add(new(productCode, productAmount, productPrice, clientAdress));

            return list;
        }

    private static string? ReadValue(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

   
    }
}
