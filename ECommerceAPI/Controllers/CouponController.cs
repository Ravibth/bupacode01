using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Cors;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [EnableCors("AllowAll")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost("{cartId}/apply-coupon")]
        public IActionResult ApplyCoupon(string cartId, [FromBody] ApplyCouponRequest request)
        {
            try
            {
                var cart = _couponService.ApplyCoupon(cartId, request.Code);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class ApplyCouponRequest
    {
        public string Code { get; set; }
    }
}
