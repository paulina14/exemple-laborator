﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Data.Models
{
    public class OrderHeaderDto
    {
        public int OrderId { get; set; }
        public int OrderLineId { get; set; }
        public string Adress { get; set; }
        public decimal Total { get; set; }
    }
}
