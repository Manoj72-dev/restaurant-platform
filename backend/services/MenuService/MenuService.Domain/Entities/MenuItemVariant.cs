namespace MenuService.Domain.Entities;

public class MenuItemVariant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description = string.Empty;
    public decimal Price { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsAvailable { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
}