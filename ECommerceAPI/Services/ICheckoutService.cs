using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICheckoutService
    {
        Order Checkout(string cartId);
        Order GetOrder(Guid orderId);
    }
}
