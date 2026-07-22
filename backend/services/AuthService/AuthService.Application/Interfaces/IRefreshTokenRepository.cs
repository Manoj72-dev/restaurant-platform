using AuthService.Domain.Entities;


namespace AuthService.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task UpdateAsync(RefreshToken token);
    }
}
