using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
    }
}
