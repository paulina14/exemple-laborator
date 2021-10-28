using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Domain.Models
{
    [AsChoice]
    public static partial class CartPaidEvent
    {
        public interface ICartPaidEvent { }

        public record CartPaidSuccededEvent : ICartPaidEvent
        {
            public string Csv { get; }
            public DateTime PayDate { get; }

            internal CartPaidSuccededEvent(string csv, DateTime payDate)
            {
                Csv = csv;
                PayDate = payDate;
            }
        }

        public record CartPaidFailEvent : ICartPaidEvent
        {
            public string Reason { get; }

            internal CartPaidFailEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
