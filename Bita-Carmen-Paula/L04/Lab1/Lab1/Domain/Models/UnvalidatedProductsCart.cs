using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record UnvalidatedProductsCart(string ProductCode, string ProductAmount, string ProductPrice, string Adress);
}
