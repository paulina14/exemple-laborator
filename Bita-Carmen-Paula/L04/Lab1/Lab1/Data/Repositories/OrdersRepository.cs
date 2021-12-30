using Lab1.Data.Models;
using Lab1.Domain.Models;
using Lab1.Domain.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1.Domain.Models.ShoppingCart;
using static LanguageExt.Prelude;

namespace Lab1.Data.Repositories
{
    public class OrdersRepository : IOrderLineRepository
    {
        private readonly CartsContext dbContext;
        public OrdersRepository(CartsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedPayment>> TryGetExistingProducts() => async () => (await (
            from m in dbContext.OrderHeaders
            join g in dbContext.OrderLines on m.OrderLineId equals g.OrderLineId
            join s in dbContext.Products on g.ProductId equals s.ProductId
            select new { s.Code, g.OrderLineId, g.Quantity, g.Price, g.FinalPrice, m.Adress })
            .AsNoTracking()
            .ToListAsync())
            .Select(result => new CalculatedPayment(
                productCode: new(result.Code),
                productAmount: new(result.Quantity),
                productPrice: new(result.Price),
                finalPrice: new(result.FinalPrice),
                adress: new(result.Adress))
            {
                OrderLineId = result.OrderLineId
            })
            .ToList();



        public TryAsync<Unit> TrySaveProducts(PaidShoppingCart cart) => async () =>
        {
            var products = (await dbContext.Products.ToListAsync()).ToLookup(product => product.Code);
            var newOrder = cart.ProductList
                            .Where(g => g.isUpdated && g.OrderLineId == 0)
                            .Select(g => new OrderLineDto()
                            {
                                ProductId = products[g.productCode.Value].Single().ProductId,
                                Quantity = g.productAmount.Value,
                                Price = g.productPrice.Value,
                                FinalPrice = g.finalPrice.Value,
                            });
            var updateOrder = cart.ProductList.Where(g => g.isUpdated && g.OrderLineId > 0)
                                    .Select(g => new OrderLineDto()
                                    {
                                        OrderLineId = g.OrderLineId,
                                        ProductId = products[g.productCode.Value].Single().ProductId,
                                        Quantity = g.productAmount.Value,
                                        Price = g.productPrice.Value,
                                        FinalPrice = g.finalPrice.Value,
                                    });

            var newheader = cart.ProductList
                .Where(g => g.isUpdated && g.OrderLineId == 0)
                .Select(g => new OrderHeaderDto()
                {
                    OrderLineId = g.OrderLineId,
                    Adress = g.adress.Adress,
                    Total = g.finalPrice.Value
                });
            var updateheader = cart.ProductList.Where(g => g.isUpdated && g.OrderLineId > 0)
                                    .Select(g => new OrderHeaderDto()
                                    {
                                        OrderId = g.OrderLineId,
                                        OrderLineId = g.OrderLineId,
                                        Adress = g.adress.Adress,
                                        Total = g.finalPrice.Value
                                    });


            dbContext.AddRange(newOrder);
            foreach(var entity in updateOrder)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync();

            dbContext.AddRange(newheader);
            foreach(var entity in updateheader)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
