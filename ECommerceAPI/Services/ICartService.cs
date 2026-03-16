using ECommerceAPI.Models;

public interface ICartService
{
    Cart AddOrUpdateItem(string cartId, CartItem item);
    Cart RemoveItem(string cartId, int productId);
    Cart GetCart(string cartId);
}