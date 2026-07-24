namespace MenuService.Application.DTOs.Responses
{
    public class MenuItemVariantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
    }
}
