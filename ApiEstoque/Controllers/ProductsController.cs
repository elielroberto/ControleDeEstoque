using ApiEstoque.DTOs;
using ApiEstoque.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstoque.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
        {
            try
            {
                var created = await _service.CreateAsync(request, token);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message }); // 409
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken token)
        {
            var product = await _service.GetByIdAsync(id, token);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var products = await _service.GetAllAsync(token);
            return Ok(products);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request, CancellationToken token)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request, token);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message }); // 400
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message }); // 409
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            var deleted = await _service.DeleteAsync(id, token);
            return deleted ? NoContent() : NotFound();
        }
    }
}
