using Lab1.Domain.Models;
using Lab1.Domain.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly CartsContext cartContext;

        public ProductsRepository (CartsContext cartContext)
        {
            this.cartContext = cartContext;
        }
        public TryAsync<List<ProductAmount>> TryGetExistingAmount(IEnumerable<string> productToCheck) => async () =>
        {
            var products = await cartContext.Products
                            .Where(product => productToCheck.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return products.Select(product => new ProductAmount(product.Stoc))
                            .ToList();
        };

        public TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<string> productToCheck) => async () =>
        {
            var products = await cartContext.Products
                            .Where(product => productToCheck.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                            .ToList();
        };
    }
}
