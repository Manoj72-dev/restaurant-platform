using MenuService.Application.Interfaces;
using MenuService.Domain.Entities;
using MenuService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly MenuDbContext _context;
        public MenuItemRepository(MenuDbContext context) => _context = context;

        public async Task<MenuItem?> GetByIdAsync(Guid id)
        => await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<MenuItem?> GetByIdWithVariantsAsync(Guid id)
            => await _context.MenuItems.Include(m => m.Variants)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<MenuItem>> GetByCategoryIdAsync(Guid categoryId)
            => await _context.MenuItems.Include(m => m.Variants)
                .Where(m => m.CategoryId == categoryId)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

        public async Task<List<MenuItem>> SearchByNameAsync(string name)
            => await _context.MenuItems
                .Where(m => EF.Functions.ILike(m.Name, $"%{name}%"))
                .ToListAsync();
        public async Task AddAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(MenuItem item)
        {
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(MenuItem item)
        {
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
