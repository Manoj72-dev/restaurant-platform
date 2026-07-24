namespace MenuService.Application.DTOs.Responses;

public class CategoryListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}