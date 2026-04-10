using ApiEstoque.DTOs;
using ApiEstoque.Models;
using ApiEstoque.Repositories.Abstractions;

namespace ApiEstoque.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                throw new ArgumentException("O SKU é obrigatório.");
            }
            var normalizedSku = NormalizeSku(request.Sku);

            var exists = await ExistsBySkuAsync(request.Sku, token);

            if (exists)
            {
                throw new InvalidOperationException("Já existe um produto com esse SKU.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("O nome é obrigatório.");
            }
            if (request.CategoryId <= 0)
            {
                throw new ArgumentException("A categoria é obrigatória.");
            }
            if (request.MinStock < 0)
            {
                throw new ArgumentException("O estoque mínimo não pode ser negativo.");
            }

            var product = new Product
            {
                Sku = normalizedSku,
                Name = request.Name.Trim(),
                Description = request.Description,
                CategoryId = request.CategoryId,
                MinStock = request.MinStock,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            var created = await _repository.CreateAsync(product, token);

            return new ProductResponse
            {
                Id = created.Id,
                Sku = created.Sku,
                Name = created.Name,
                Description = created.Description,
                CategoryId = created.CategoryId,
                IsActive = created.IsActive,
                MinStock = created.MinStock,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
        }

        public async Task<ProductResponse?> GetByIdAsync(int id, CancellationToken token = default)
        {
            var product = await _repository.GetByIdAsync(id, token);

            if (product == null)
            {
                return null;
            }

            return new ProductResponse
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                MinStock = product.MinStock,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<List<ProductResponse>> GetAllAsync(CancellationToken token = default)
        {
            var products = await _repository.GetAllAsync(token);

            var response = new List<ProductResponse>();

            foreach (var product in products)
            {
                response.Add(new ProductResponse
                {
                    Id = product.Id,
                    Sku = product.Sku,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    IsActive = product.IsActive,
                    MinStock = product.MinStock,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt
                });
            }
            return response;
        }

        public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidOperationException("O nome é obrigatório.");

            if (request.CategoryId <= 0)
                throw new InvalidOperationException("A categoria é obrigatória.");

            if (request.MinStock < 0)
                throw new InvalidOperationException("O estoque mínimo não pode ser negativo.");

            var existing = await _repository.GetByIdAsync(id, token);

            if (existing == null)
                return null;

            existing.Name = request.Name.Trim();
            existing.Description = request.Description;
            existing.CategoryId = request.CategoryId;
            existing.MinStock = request.MinStock;
            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing, token);

            if (updated is null)
                return null;

            return new ProductResponse
            {
                Id = updated.Id,
                Sku = updated.Sku,
                Name = updated.Name,
                Description = updated.Description,
                CategoryId = updated.CategoryId,
                IsActive = updated.IsActive,
                MinStock = updated.MinStock,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
        {
            return await _repository.DeleteAsync(id, token);
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(sku))
                return false;

            var normalizedSku = NormalizeSku(sku);
            return await _repository.ExistsBySkuAsync(normalizedSku, token);
        }

        private string NormalizeSku(string sku)
        {
            return sku.Trim().ToUpper();
        }

    }
}
