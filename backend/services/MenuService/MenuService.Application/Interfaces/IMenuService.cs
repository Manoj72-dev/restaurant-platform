using MenuService.Application.DTOs.Responses;
using MenuService.Application.DTOs.Requests;

namespace MenuService.Application.Interfaces
{
    public interface IMenuService
    {
        // Public read endpoints (match your notes)
        Task<List<CategoryListResponse>> GetCategoriesAsync();
        Task<List<MenuItemListResponse>> GetItemsByCategoryAsync(Guid categoryId);
        Task<List<MenuItemSearchResponse>> SearchItemsByNameAsync(string name);
        Task<MenuItemDetailResponse> GetItemByIdAsync(Guid itemId);

        // Admin - Category CRUD
        Task<CategoryListResponse> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryListResponse> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
        Task DeleteCategoryAsync(Guid id);

        // Admin - MenuItem CRUD
        Task<MenuItemDetailResponse> CreateMenuItemAsync(CreateMenuItemRequest request);
        Task<MenuItemDetailResponse> UpdateMenuItemAsync(Guid id, UpdateMenuItemRequest request);
        Task DeleteMenuItemAsync(Guid id);

        // Admin - Availability toggles
        Task<MenuItemDetailResponse> ToggleItemAvailabilityAsync(Guid itemId, bool isAvailable);
        Task<MenuItemDetailResponse> ToggleVariantAvailabilityAsync(Guid variantId, bool isAvailable);

        // Admin - Variant CRUD (since items can gain/lose variants independently over time)
        Task<MenuItemDetailResponse> AddVariantAsync(Guid itemId, VariantRequest request);
        Task<MenuItemDetailResponse> UpdateVariantAsync(Guid variantId, VariantRequest request);
        Task DeleteVariantAsync(Guid variantId);
    }
}
