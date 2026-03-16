using ECommerceAPI.Data;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [EnableCors("AllowAll")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        public IActionResult AddOrUpdateItem([FromBody] AddCartItemRequest request)
        {
            try
            {
                var cart = _cartService.AddOrUpdateItem(request.CartId, request.Item);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{cartId}/items/{productId}")]
        public IActionResult RemoveItem(string cartId, int productId)
        {
            try
            {
                var cart = _cartService.RemoveItem(cartId, productId);
                return Ok(cart);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{cartId}")]
        public IActionResult GetCart(string cartId)
        {
            try
            {
                var cart = _cartService.GetCart(cartId);
                return Ok(cart);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

    public class AddCartItemRequest
    {
        public string CartId { get; set; }
        public CartItem Item { get; set; }
    }
}
