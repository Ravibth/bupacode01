using ECommerceAPI.Data;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            return FakeStore.Products;
        }

        public Product? GetById(int id)
        {
            return FakeStore.Products.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Product product)
        {
            var existing = FakeStore.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Description = product.Description;
            existing.ImageUrl = product.ImageUrl;
        }
    }
}
