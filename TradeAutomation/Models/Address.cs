using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAutomation.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string House { get; set; } = string.Empty;
        public string? Apartment { get; set; }
        public string? PostalCode { get; set; }

        public string FullAddress => $"{City}, ул. {Street}, д. {House}" +
            (string.IsNullOrEmpty(Apartment) ? "" : $", кв. {Apartment}");
    }
}
