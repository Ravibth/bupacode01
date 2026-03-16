using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public Order Checkout(string cartId)
        {
            var cart = _cartRepository.Get(cartId);

            // Check stock again
            foreach (var item in cart.Items)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException("Insufficient stock for checkout");
                }
            }

            // Reduce stock atomically
            foreach (var item in cart.Items)
            {
                var product = _productRepository.GetById(item.ProductId)!;
                product.Stock -= item.Quantity;
                _productRepository.Update(product);
            }

            var subtotal = cart.Items.Sum(x => x.Price * x.Quantity);

            // Create order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CartId = cartId,
                Items = new List<CartItem>(cart.Items),
                Subtotal = subtotal,
                Discount = cart.Discount,
                Tax = 0, // No tax
                GrandTotal = subtotal - cart.Discount,
                CreatedAt = DateTime.UtcNow
            };

            _orderRepository.Add(order);

            // Clear cart
            cart.Items.Clear();
            cart.Discount = 0;
            cart.TotalAmount = 0;

            _cartRepository.Save(cart);

            return order;
        }

        public Order GetOrder(Guid orderId)
        {
            return _orderRepository.Get(orderId);
        }
    }
}
