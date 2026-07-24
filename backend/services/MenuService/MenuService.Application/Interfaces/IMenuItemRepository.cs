using MenuService.Domain.Entities;

namespace MenuService.Application.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetByIdAsync(Guid id);
        Task<MenuItem?> GetByIdWithVariantsAsync(Guid id);
        Task<List<MenuItem>> GetByCategoryIdAsync(Guid categoryId);
        Task<List<MenuItem>> SearchByNameAsync(string name);
        Task AddAsync(MenuItem item);
        Task UpdateAsync(MenuItem item);
        Task DeleteAsync(MenuItem menuItemId);
    }
}
