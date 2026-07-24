namespace MenuService.Application.DTOs.Requests
{
    public class CreateMenuItemRequest
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVegetarian { get; set; }
        public List<VariantRequest> Variants { get; set; } = new();
    }
}
