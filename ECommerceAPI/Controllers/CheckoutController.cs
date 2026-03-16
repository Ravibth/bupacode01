using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [EnableCors("AllowAll")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("{cartId}/checkout")]
        public IActionResult Checkout(string cartId)
        {
            try
            {
                var order = _checkoutService.Checkout(cartId);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
