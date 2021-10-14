using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain
{
    class InvalidClientAdressException : Exception
    {
        public InvalidClientAdressException()
        {
        }

        public InvalidClientAdressException(string? message) : base(message)
        {
        }

        public InvalidClientAdressException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidClientAdressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
