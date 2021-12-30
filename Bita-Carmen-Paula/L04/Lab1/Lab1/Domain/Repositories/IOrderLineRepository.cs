using Lab1.Domain.Models;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1.Domain.Models.ShoppingCart;

namespace Lab1.Domain.Repositories
{
    public interface IOrderLineRepository
    {
        TryAsync<List<CalculatedPayment>> TryGetExistingProducts();
        TryAsync<Unit> TrySaveProducts(PaidShoppingCart cart);
    }
}
