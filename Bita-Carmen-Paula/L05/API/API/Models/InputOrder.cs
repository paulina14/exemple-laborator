using Lab1.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class InputOrder
    {
        [Required]
        //[RegularExpression(ProductCode.Pattern)]
        public string Code { get; set; }

        [Required]
        [Range(1,10)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 6000)]
        public decimal Price { get; set; }

        [Required]
        //[RegularExpression(ClientAdress.Pattern)]
        public string Adress { get; set; }
    }
}
