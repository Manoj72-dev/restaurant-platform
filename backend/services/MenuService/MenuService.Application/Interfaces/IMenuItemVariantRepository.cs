using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces
{
    public interface IMenuItemVariantRepository
    {
        Task<MenuItemVariant?> GetByIdAsync(Guid id);
        Task AddAsync(MenuItemVariant variant);
        Task UpdateAsync(MenuItemVariant variant);
        Task DeleteAsync(MenuItemVariant variant);
    }
}
