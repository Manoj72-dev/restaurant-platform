namespace MenuService.Application.DTOs.Responses;

public class MenuItemDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public Guid CategoryId { get; set; }
    public decimal StartingPrice { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsAvailable { get; set; }
    public List<MenuItemVariantResponse> Variants { get; set; } = new();
}