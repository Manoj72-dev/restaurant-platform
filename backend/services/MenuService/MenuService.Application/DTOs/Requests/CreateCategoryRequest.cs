namespace MenuService.Application.DTOs.Requests
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }
}
