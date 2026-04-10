namespace ApiEstoque.DTOs
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int MinStock { get; set; }
        public bool IsActive { get; set; }
    }
}
