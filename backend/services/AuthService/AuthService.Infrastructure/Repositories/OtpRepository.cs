using AuthService.Application.DTOs.Requests;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    
    public class OtpRepository : IOtpRepository
    {
        private readonly AuthDbContext _context;

        public OtpRepository(AuthDbContext context) => 
            _context = context;
        

        public async Task AddAsync(OtpVerification otp)
        {
            _context.OtpVerifications.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<OtpVerification?> GetLatestActiveByPhoneAsync(string phoneNumber)
            => await _context.OtpVerifications
                .Where(o => o.PhoneNumber == phoneNumber && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

        public async Task UpdateAsync(OtpVerification otp)
        {
            _context.OtpVerifications.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}
