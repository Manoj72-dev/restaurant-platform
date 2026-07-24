namespace MenuService.Application.DTOs.Requests
{
    public class UpdateMenuItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVegetarian { get; set; } = false;
    }
}
