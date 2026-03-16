using System.Collections.Generic;

namespace ECommerceAPI.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public string CartId { get; set; }

        public List<CartItem> Items { get; set; } = new();

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; } // Assuming 0 for now

        public decimal GrandTotal { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
