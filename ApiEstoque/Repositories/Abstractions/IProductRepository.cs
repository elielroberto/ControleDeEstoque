using ApiEstoque.Models;

namespace ApiEstoque.Repositories.Abstractions
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product, CancellationToken token = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken token = default);
        Task <List<Product>> GetAllAsync(CancellationToken token = default);
        Task <Product?> UpdateAsync(Product product, CancellationToken token = default);
        Task <bool> DeleteAsync(int id, CancellationToken token = default);
        Task <bool> ExistsBySkuAsync(string sku, CancellationToken token = default);
    }
}
