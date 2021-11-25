using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record CalculatedPayment(ProductCode productCode, ProductAmount productAmount, ProductPrice productPrice, ProductPrice finalPrice);
}
