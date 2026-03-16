using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [EnableCors("AllowAll")]
    public class OrdersController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public OrdersController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(Guid orderId)
        {
            try
            {
                var order = _checkoutService.GetOrder(orderId);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}