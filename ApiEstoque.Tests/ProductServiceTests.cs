using ApiEstoque.DTOs;
using ApiEstoque.Models;
using ApiEstoque.Repositories.Abstractions;
using ApiEstoque.Services;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;

namespace ApiEstoque.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateAsync_DeveLancarErro_QuandoSkuForVazio()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();
            var service = new ProductService(repositoryMock.Object);

            var request = new CreateProductRequest
            {
                Sku = "", // Sku Invalido
                Name = "Produto Teste",
                CategoryId = 1,
                MinStock = 10
            };

            //Act + Assert (agir + verificar)

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.CreateAsync(request);
            });
        }

        [Fact]
        public async Task CreateAsync_DeveLancarErro_QuandoSkuJaExiste()
        {
            // Arrange (Preparar)
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock.Setup(r => r.ExistsBySkuAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true); // Simula que o SKU já existe

            var service = new ProductService(repositoryMock.Object);

            var request = new CreateProductRequest
            {
                Sku = "ABC123", // SKU já existe
                Name = "Produto Teste",
                CategoryId = 1,
                MinStock = 10
            };

            // Act + Assert (Agir + Verificar)
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await service.CreateAsync(request);
            });
        }

        [Fact]
        public async Task CreateAsync_DeveCriarProduto_QuandoDadosForemValidos()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            // 1) Simula: SKU NÃO existe
            repositoryMock
                .Setup(r => r.ExistsBySkuAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // 2) Simula: CreateAsync "salva" e retorna um Product com Id
            repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product p, CancellationToken _) =>
                {
                    p.Id = 1; // finge que o banco gerou o Id
                    return p;
                });

            var service = new ProductService(repositoryMock.Object);

            var request = new CreateProductRequest
            {
                Sku = "ABC123",
                Name = "Produto Teste",
                Description = "Descrição",
                CategoryId = 1,
                MinStock = 10
            };

            //Act (Agir)

            var result = await service.CreateAsync(request);

            //Assert (Verificar)

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("ABC123", result.Sku); // seu service normaliza pra maiúsculo
            Assert.Equal("Produto Teste", result.Name);
            Assert.Equal(1, result.CategoryId);
            Assert.Equal(10, result.MinStock);
            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoProdutoNaoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var result = await service.GetByIdAsync(999); // Id que não existe

            //Assert (Verificar)

             Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarProduto_QuandoProdutoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product
                {
                    Id = 1,
                    Sku = "ABC123",
                    Name = "Produto Teste",
                    Description = "Descrição",
                    CategoryId = 1,
                    MinStock = 10,
                    IsActive = true
                });

            var result = await service.GetByIdAsync(1); // Id que existe

            //Assert (Verificar)

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("ABC123", result.Sku);
            Assert.Equal("Produto Teste", result.Name);
            Assert.Equal("Descrição", result.Description);
            Assert.Equal(1, result.CategoryId);
            Assert.Equal(10, result.MinStock);
            Assert.True(result.IsActive);

        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarNull_QuandoProdutoNaoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

            var request = new UpdateProductRequest
            {
                Name = "Produto Teste",
                Description = "Descrição",
                CategoryId = 1,
                MinStock = 10,
                IsActive = true
            };

            var result = await service.UpdateAsync(999, request); // Id que não existe

            //Assert (Verificar)

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizar_QuandoProdutoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product
            {
                Id = 1,
                Sku = "ABC123",
                Name = "Produto Teste",
                Description = "Descrição",
                CategoryId = 1,
                MinStock = 10,
                IsActive = true
            });

            repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product p, CancellationToken _) => p); // Simula que o Update retorna o produto atualizado

            var request = new UpdateProductRequest
            {
                Name = "Produto Teste Atualizado",
                Description = "Descrição Atualizada",
                CategoryId = 2,
                MinStock = 20,
                IsActive = false
            };

            var result = await service.UpdateAsync(1, request); // Id que existe

            //Assert (Verificar)

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("ABC123", result.Sku); // SKU não muda
            Assert.Equal("Produto Teste Atualizado", result.Name);
            Assert.Equal("Descrição Atualizada", result.Description);
            Assert.Equal(2, result.CategoryId);
            Assert.Equal(20, result.MinStock);
            Assert.False(result.IsActive);
                
        }
        [Fact]
        public async Task DeleteAsync_DeveRetornarFalse_QuandoProdutoNaoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); // Simula que o produto não foi encontrado para deletar

            var result = await service.DeleteAsync(999); // Id que não existe

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_DeveRetornarTrue_QuandoProdutoExiste()
        {
            //Arrange (Preparar)

            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); // Simula que o produto foi encontrado e deletado

            var result = await service.DeleteAsync(999); // Id que existe

            Assert.True(result);

        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDeProdutos()
        {
            //Arrange (Preparar)
            var repositoryMock = new Mock<IProductRepository>();

            var service = new ProductService(repositoryMock.Object);

            repositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Sku = "ABC123",
                        Name = "Produto Teste",
                        Description = "Descrição",
                        CategoryId = 1,
                        MinStock = 10,
                        IsActive = true
                    },
                    new Product
                    {
                        Id = 2,
                        Sku = "DEF456",
                        Name = "Produto Teste 2",
                        Description = "Descrição 2",
                        CategoryId = 2,
                        MinStock = 20,
                        IsActive = false
                    }
                });

            //Act

            var result = await service.GetAllAsync();

            //Assert

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("ABC123", result[0].Sku);
            Assert.Equal("Produto Teste", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("DEF456", result[1].Sku);
            Assert.Equal("Produto Teste 2", result[1].Name);
        }
    }
}
