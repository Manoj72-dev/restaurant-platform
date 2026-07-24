

using MenuService.Application.Interfaces;
using MenuService.Domain.Entities;
using MenuService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Repositories
{
    public class MenuItemVariantRepository : IMenuItemVariantRepository
    {
        private readonly MenuDbContext _context;

        public MenuItemVariantRepository(MenuDbContext contex) => _context = contex;

        public async Task<MenuItemVariant?> GetByIdAsync(Guid id)
            => await _context.MenuItemVariants.FirstOrDefaultAsync(v => v.Id == id);
        public async Task AddAsync(MenuItemVariant variant)
        {
            _context.MenuItemVariants.Add(variant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuItemVariant variant)
        {
            _context.MenuItemVariants.Update(variant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MenuItemVariant variant)
        {
            _context.MenuItemVariants.Remove(variant);
            await _context.SaveChangesAsync();
        }
    }
}
