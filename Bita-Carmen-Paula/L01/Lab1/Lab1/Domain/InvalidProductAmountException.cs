using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    [Serializable]
    internal class InvalidProductAmountException : Exception
    {
        public InvalidProductAmountException()
        {
        }

        public InvalidProductAmountException(string? message) : base(message)
        {
        }

        public InvalidProductAmountException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductAmountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
