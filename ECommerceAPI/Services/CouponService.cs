using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICartRepository cartRepository, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
        }

        public Cart ApplyCoupon(string cartId, string code)
        {
            var cart = _cartRepository.Get(cartId);

            var coupon = _couponRepository.GetActiveByCode(code);

            if (coupon == null)
                throw new ArgumentException("Invalid coupon");

            var subtotal = cart.Items.Sum(x => x.Price * x.Quantity);

            decimal discount = 0;

            if (coupon.Type == "FLAT50" && subtotal >= 500)
            {
                discount = 50;
            }
            else if (coupon.Type == "SAVE10" && subtotal >= 1000)
            {
                discount = Math.Min(subtotal * 0.10m, 200);
            }
            else
            {
                throw new ArgumentException("Coupon conditions not met");
            }

            cart.Discount = discount;
            cart.TotalAmount = subtotal - discount;

            _cartRepository.Save(cart);

            return cart;
        }
    }
}
