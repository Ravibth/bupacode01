using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        public void Add(Order order)
        {
            FakeStore.Orders.Add(order);
        }

        public Order Get(Guid orderId)
        {
            var order = FakeStore.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return order;
        }
    }
}
