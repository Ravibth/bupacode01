using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories

{
    public interface IOrderRepository
    {
        void Add(Order order);
        Order Get(Guid orderId);
    }
}
