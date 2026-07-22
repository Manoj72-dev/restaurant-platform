using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthDbContext _context;

        public RefreshTokenRepository(AuthDbContext context) => _context = context;

        public async Task AddAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
            => await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
