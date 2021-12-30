using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record CalculetedHeader(ClientAdress adress, ProductAmount total)
    {
        public int OrderHeaderId { get; set; }
        public bool isUpdated { get; set; }
    }
}
