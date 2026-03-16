using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface ICartRepository
    {
        Cart GetOrCreate(string cartId);
        Cart Get(string cartId);
        void Save(Cart cart);
    }
}
