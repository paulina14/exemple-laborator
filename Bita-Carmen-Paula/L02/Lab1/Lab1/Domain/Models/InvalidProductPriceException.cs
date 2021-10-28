using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    [Serializable]
    internal class InvalidProductPriceException : Exception
    {
        public InvalidProductPriceException()
        {
        }

        public InvalidProductPriceException(string? message) : base(message)
        {
        }

        public InvalidProductPriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductPriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
