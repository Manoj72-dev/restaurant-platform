namespace MenuService.Application.DTOs.Requests
{
    public class VariantRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description = string.Empty;
        public decimal Price { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
