using LanguageExt;
using static LanguageExt.Prelude;
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
        private static readonly Regex ValidPattern = new("^Romania");
        public string Adress { get; }

        public ClientAdress(string adress)
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

        public static Option<ClientAdress> TryParseAdress(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ClientAdress>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
