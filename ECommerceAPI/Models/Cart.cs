using System.Collections.Generic;

namespace ECommerceAPI.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
