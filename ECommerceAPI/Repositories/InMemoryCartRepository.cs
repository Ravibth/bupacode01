using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class InMemoryCartRepository : ICartRepository
    {
        public Cart GetOrCreate(string cartId)
        {
            if (!FakeStore.Carts.ContainsKey(cartId))
            {
                FakeStore.Carts[cartId] = new Cart { Id = cartId };
            }

            return FakeStore.Carts[cartId];
        }

        public Cart Get(string cartId)
        {
            if (!FakeStore.Carts.ContainsKey(cartId))
            {
                throw new KeyNotFoundException("Cart not found");
            }

            return FakeStore.Carts[cartId];
        }

        public void Save(Cart cart)
        {
            FakeStore.Carts[cart.Id] = cart;
        }
    }
}
