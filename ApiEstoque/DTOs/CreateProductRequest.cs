namespace ApiEstoque.DTOs
{
    public class CreateProductRequest
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int MinStock { get; set; }

    }
}
