using ApiEstoque.Data;
using ApiEstoque.Models;
using ApiEstoque.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ApiEstoque.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken token = default)
        {
            await _context.Products.AddAsync(product, token);
            await _context.SaveChangesAsync(token);
            return product;
        }

        public async Task<Product?> GetByIdAsync (int id, CancellationToken token = default)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, token);
             
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.Products.AsNoTracking().ToListAsync(token);
        }

        public async Task<Product?> UpdateAsync(Product product, CancellationToken token = default)
        {
            _context.Products.Update(product);

            await _context.SaveChangesAsync(token);
            return product;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
        {
            var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, token);
            if (existing == null)
            {
                return false;
            }
            _context.Products.Remove(existing);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken token = default)
        {
            return await _context.Products.AnyAsync(p => p.Sku == sku, token);
        }
    }
}
