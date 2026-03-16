namespace ECommerceAPI.Models
{
    public class Coupon
    {
        public string Code { get; set; }

        public string Type { get; set; } // "FLAT50" or "SAVE10"

        public bool IsActive { get; set; }
    }
}
