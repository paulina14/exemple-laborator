using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    public record ClientAdress
    {
        private static readonly Regex ValidPattern = new("^Romania$");
        public string Adress { get; }

        private ClientAdress(string adress)
        {
            if (ValidPattern.IsMatch(adress))
            {
                Adress = adress;
            }
            else
            {
                throw new InvalidClientAdressException("");
            }
        }

        public override string ToString()
        {
            return Adress;
        }
    }
}
