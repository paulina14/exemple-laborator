using Lab1.Domain.Models;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Repositories
{
    public interface IProductsRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<string> productToCheck);
        TryAsync<List<ProductAmount>> TryGetExistingAmount(IEnumerable<string> productToCheck);
    }
}
