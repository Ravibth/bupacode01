using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICouponService
    {
        Cart ApplyCoupon(string cartId, string code);
    }
}
