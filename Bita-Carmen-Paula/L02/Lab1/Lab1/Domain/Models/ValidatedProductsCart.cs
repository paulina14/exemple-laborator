﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record ValidatedProductsCart(ProductCode productCode, ProductAmount productAmount, ProductPrice productPrice);
}
