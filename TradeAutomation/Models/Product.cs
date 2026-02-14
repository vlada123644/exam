using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAutomation.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int QuantityInStock { get; set; }
        public string UnitOfMeasure { get; set; } = "шт";
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Computed property
        public decimal PriceWithDiscount => Discount.HasValue && Discount > 0
            ? Price * (1 - Discount.Value / 100)
            : Price;

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

