using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class InMemoryCouponRepository : ICouponRepository
    {
        public Coupon? GetActiveByCode(string code)
        {
            return FakeStore.Coupons.FirstOrDefault(c => c.Code == code && c.IsActive);
        }
    }
}
