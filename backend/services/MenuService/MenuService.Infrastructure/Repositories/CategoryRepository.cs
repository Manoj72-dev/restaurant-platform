using MenuService.Application.Interfaces;
using MenuService.Application.DTOs.Responses;
using MenuService.Infrastructure.Persistence;
using MenuService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Infrastructure.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly MenuDbContext _context;
        public CategoryRepository(MenuDbContext context) => _context = context;

        public async Task<Category?> GetByIdAsync(Guid id)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();

        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
