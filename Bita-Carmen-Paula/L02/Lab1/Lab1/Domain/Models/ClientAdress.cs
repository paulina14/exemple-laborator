using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    public record ClientAdress
    {
        private static readonly Regex ValidPattern = new("^Romania$");
        public string Adress { get; }

        private ClientAdress(string adress)
        {
            if (IsValid(adress))
            {
                Adress = adress;
            }
            else
            {
                throw new InvalidClientAdressException("");
            }
        }
        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Adress;
        }
        public static bool TryParseClientAdress(string stringValue, out ClientAdress adress)
        {
            bool isValid = false;
            adress = null;

            if (IsValid(stringValue))
            {
                isValid = true;
                adress = new(stringValue);
            }
            return isValid;
        }
    }
}
