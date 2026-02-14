using System.Collections.Generic;

namespace TradeAutomation.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Color { get; set; } = "#FFA500";

        // Навигационное свойство
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
