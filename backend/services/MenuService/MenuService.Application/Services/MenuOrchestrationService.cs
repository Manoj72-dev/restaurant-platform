using MenuService.Application.DTOs.Requests;
using MenuService.Application.DTOs.Responses;
using MenuService.Application.Interfaces;
using MenuService.Domain.Entities;
using MenuService.Application.Exceptions;

namespace MenuService.Application.Services
{
    public class MenuOrchestrationService : IMenuService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMenuItemVariantRepository _variantRepository;

        public MenuOrchestrationService(
            ICategoryRepository categoryRepository,
            IMenuItemRepository menuItemRepository,
            IMenuItemVariantRepository variantRepository)
        {
            _categoryRepository = categoryRepository;
            _menuItemRepository = menuItemRepository;
            _variantRepository = variantRepository;
        }

        //------------- public reads -------------------

        public async Task<List<CategoryListResponse>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToCategoryListResponse).ToList();
        }

        public async Task<List<MenuItemListResponse>> GetItemsByCategoryAsync(Guid categoryId)
        {
            var items = await _menuItemRepository.GetByCategoryIdAsync(categoryId);
            return items.Select(MapToMenuItemListResponse).ToList();
        }

        public async Task<List<MenuItemSearchResponse>> SearchItemsByNameAsync(string name)
        {
            var items = await _menuItemRepository.SearchByNameAsync(name);
            return items.Select(i => new MenuItemSearchResponse { Id = i.Id, Name = i.Name }).ToList();
        }

        public async Task<MenuItemDetailResponse> GetItemByIdAsync(Guid itemId)
        {
            var item = await _menuItemRepository.GetByIdWithVariantsAsync(itemId)
                ?? throw new NotFoundException("Menu item not found.");

            return MapToMenuItemDetailResponse(item);
        }

        // ----------------- Admin -------------------------
        public async Task<CategoryListResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name,
                DisplayOrder = request.DisplayOrder
            };
            await _categoryRepository.AddAsync(category);
            return MapToCategoryListResponse(category);
        }

        public async Task<CategoryListResponse> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Category not found.");

            category.Name = request.Name;
            category.DisplayOrder = request.DisplayOrder;
            category.IsActive = request.IsActive;

            await _categoryRepository.UpdateAsync(category);
            return MapToCategoryListResponse(category);

        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Category not found.");

            await _categoryRepository.DeleteAsync(category);
        }

        //----------------------------------------------------------------------------------------------------------------

        public async Task<MenuItemDetailResponse> CreateMenuItemAsync(CreateMenuItemRequest request)
        {
            if (request.Variants is null || request.Variants.Count == 0)
                throw new BadRequestException("At least one variant is required to create a menu item.");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                ?? throw new NotFoundException("Category not found.");

            var item = new MenuItem
            {
                CategoryId = category.Id,
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsVegetarian = request.IsVegetarian
            };

            var hasDefault = request.Variants.Any(v => v.IsDefault);
            foreach (var (v, index) in request.Variants.Select((v, i) => (v, i)))
            {
                item.Variants.Add(new MenuItemVariant
                {
                    Name = v.Name,
                    Price = v.Price,
                    IsDefault = hasDefault ? v.IsDefault : index == 0
                });
            }

            await _menuItemRepository.AddAsync(item);
            return MapToMenuItemDetailResponse(item);
        }

        public async Task<MenuItemDetailResponse> UpdateMenuItemAsync(Guid id, UpdateMenuItemRequest request)
        {
            var item = await _menuItemRepository.GetByIdWithVariantsAsync(id)
                ?? throw new NotFoundException("Menu item not found.");

            item.Name = request.Name;
            item.Description = request.Description;
            item.ImageUrl = request.ImageUrl;
            item.IsVegetarian = request.IsVegetarian;

            await _menuItemRepository.UpdateAsync(item);
            return MapToMenuItemDetailResponse(item);
        }

        public async Task DeleteMenuItemAsync(Guid id)
        {
            var item = await _menuItemRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Menu item not found.");

            await _menuItemRepository.DeleteAsync(item);
        }

        //----------------------------------------------------------------------------------------------------------

        public async Task<MenuItemDetailResponse> AddVariantAsync(Guid itemId, VariantRequest request)
        {
            var item = await _menuItemRepository.GetByIdWithVariantsAsync(itemId)
                ?? throw new NotFoundException("Menu item not found.");

            var variant = new MenuItemVariant
            {
                MenuItemId = item.Id,
                Name = request.Name,
                Price = request.Price,
                IsDefault = request.IsDefault
            };

            await _variantRepository.AddAsync(variant);

            var refreshed = await _menuItemRepository.GetByIdWithVariantsAsync(itemId)
                ?? throw new NotFoundException("Menu item not found.");

            return MapToMenuItemDetailResponse(refreshed);
        }

        public async Task<MenuItemDetailResponse> UpdateVariantAsync(Guid variantId, VariantRequest request)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId)
                ?? throw new NotFoundException("Variant not found.");

            variant.Name = request.Name;
            variant.Price = request.Price;
            variant.Description = request.Description;

            await _variantRepository.UpdateAsync(variant);

            var item = await _menuItemRepository.GetByIdWithVariantsAsync(variant.MenuItemId)
                ?? throw new NotFoundException("Menu item not found.");

            return MapToMenuItemDetailResponse(item);
        }

        public async Task DeleteVariantAsync(Guid variantId)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId)
                ?? throw new NotFoundException("Variant not found.");

            var item = await _menuItemRepository.GetByIdWithVariantsAsync(variant.MenuItemId)
                ?? throw new NotFoundException("Menu item not found.");

            if (item.Variants.Count <= 1)
                throw new BadRequestException("Cannot delete the last variant. Delete the item instead, or add another variant first.");

            await _variantRepository.DeleteAsync(variant);
        }

        //---------------------------------------------------------------------------------------------------------------------

        public async Task<MenuItemDetailResponse> ToggleItemAvailabilityAsync(Guid itemId, bool isAvailable)
        {
            var item = await _menuItemRepository.GetByIdWithVariantsAsync(itemId)
                ?? throw new NotFoundException("Menu item not found.");

            item.IsAvailable = isAvailable;
            await _menuItemRepository.UpdateAsync(item);

            return MapToMenuItemDetailResponse(item);
        }

        public async Task<MenuItemDetailResponse> ToggleVariantAvailabilityAsync(Guid variantId, bool isAvailable)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId)
                ?? throw new NotFoundException("Variant not found.");

            variant.IsAvailable = isAvailable;
            await _variantRepository.UpdateAsync(variant);

            var item = await _menuItemRepository.GetByIdWithVariantsAsync(variant.MenuItemId)
                ?? throw new NotFoundException("Menu item not found.");

            var anyVariantAvailable = item.Variants.Any(v => v.IsAvailable);

            if (anyVariantAvailable != item.IsAvailable)
            {
                item.IsAvailable = anyVariantAvailable;
                await _menuItemRepository.UpdateAsync(item);
            }

            return MapToMenuItemDetailResponse(item);
        }

        //------------------------------------------------------------------------------------------------------------------------
        private static CategoryListResponse MapToCategoryListResponse(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            DisplayOrder = c.DisplayOrder
        };

        private static MenuItemListResponse MapToMenuItemListResponse(MenuItem m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            DisplayOrder = m.DisplayOrder,
            CategoryId = m.CategoryId,
            StartingPrice = m.Variants.Count > 0 ? m.Variants.Min(v => v.Price) : 0,
            ImageUrl = m.ImageUrl,
            IsVegetarian = m.IsVegetarian,
            IsAvailable = m.IsAvailable
        };

        private static MenuItemDetailResponse MapToMenuItemDetailResponse(MenuItem m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            DisplayOrder = m.DisplayOrder,
            CategoryId = m.CategoryId,
            StartingPrice = m.Variants.Count > 0 ? m.Variants.Min(v => v.Price) : 0,
            ImageUrl = m.ImageUrl,
            IsVegetarian = m.IsVegetarian,
            IsAvailable = m.IsAvailable,
            Variants = m.Variants.Select(v => new MenuItemVariantResponse
            {
                Id = v.Id,
                Name = v.Name,
                Description = v.Description,
                Price = v.Price,
                IsDefault = v.IsDefault,
                IsAvailable = v.IsAvailable
            }).ToList()
        };


    }
}
