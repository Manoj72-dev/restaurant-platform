namespace MenuService.Application.DTOs.Requests
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; }
    }
}
