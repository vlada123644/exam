using System;
using System.Collections.Generic;

namespace TradeAutomation.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
        public virtual Address Address { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}