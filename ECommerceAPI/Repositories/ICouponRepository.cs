using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface ICouponRepository
    {
        Coupon? GetActiveByCode(string code);
    }
}
