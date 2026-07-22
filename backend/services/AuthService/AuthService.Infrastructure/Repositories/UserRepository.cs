using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
            => _context = context;

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
            => await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

        public async Task<List<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
