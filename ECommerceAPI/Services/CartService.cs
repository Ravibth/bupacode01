using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public Cart AddOrUpdateItem(string cartId, CartItem item)
        {
            var cart = _cartRepository.GetOrCreate(cartId);

            var product = _productRepository.GetById(item.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found");

            if (item.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            if (product.Stock < item.Quantity)
                throw new ArgumentException("Insufficient stock");

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
            }
            else
            {
                item.Price = product.Price;
                cart.Items.Add(item);
            }

            RecalculateCart(cart);
            _cartRepository.Save(cart);

            return cart;
        }

        public Cart RemoveItem(string cartId, int productId)
        {
            var cart = _cartRepository.Get(cartId);

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem == null)
                throw new KeyNotFoundException("Item not found in cart");

            cart.Items.Remove(existingItem);
            RecalculateCart(cart);
            _cartRepository.Save(cart);

            return cart;
        }

        public Cart GetCart(string cartId)
        {
            return _cartRepository.Get(cartId);
        }

        private void RecalculateCart(Cart cart)
        {
            cart.Discount = 0;
            var subtotal = cart.Items.Sum(x => x.Price * x.Quantity);
            cart.TotalAmount = subtotal - cart.Discount;
        }
    }
}
