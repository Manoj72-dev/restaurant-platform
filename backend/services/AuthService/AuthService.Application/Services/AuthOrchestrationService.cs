using AuthService.Application.DTOs.Requests;
using AuthService.Application.DTOs.Responses;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;

namespace AuthService.Application.Services;

public class AuthOrchestrationService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpService _otpService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IHasher _hasher;

    public AuthOrchestrationService(
        IUserRepository userRepository,
        IOtpService otpService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IHasher hasher)
    {
        _userRepository = userRepository;
        _otpService = otpService;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _hasher = hasher;
    }

    public async Task SendOtpAsync(SendOtpRequest request)
        => await _otpService.GenerateAndSendOtpAsync(request.PhoneNumber);

    public async Task ResendOtpAsync(ResendOtpRequest request)
        => await _otpService.GenerateAndSendOtpAsync(request.PhoneNumber);

    public async Task<AuthResponse> VerifyOtpAsync(VerifyOtpRequest request, string? ipAddress)
    {
        var isValid = await _otpService.VerifyOtpAsync(request.PhoneNumber, request.Otp);

        if (!isValid)
            throw new InvalidOperationException("Invalid or expired OTP.");

        var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber.Trim());
        if (user is null)
        {
            user = new User
            {
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Customer,
                IsPhoneVerified = true
            };
            await _userRepository.AddAsync(user);
        }
        else if (!user.IsPhoneVerified)
        {
            user.IsPhoneVerified = true;
            await _userRepository.UpdateAsync(user);
        }

        return await IssueTokensAsync(user, ipAddress);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, string? ipAddress)
    {
        var tokenHash = _hasher.Hash(refreshToken);
        var existingToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (existingToken is null || !existingToken.IsActive)
            throw new InvalidOperationException("Invalid or expired refresh token.");

        existingToken.IsRevoked = true;

        var newAccessToken = _tokenService.GenerateAccessToken(existingToken.User!);
        var newRefreshTokenPlain = _tokenService.GenerateRefreshToken();

        existingToken.ReplacedByToken = newRefreshTokenPlain;
        await _refreshTokenRepository.UpdateAsync(existingToken);

        var newRefreshToken = new RefreshToken
        {
            UserId = existingToken.UserId,
            TokenHash = _hasher.Hash(newRefreshTokenPlain),
            ExpiresAt = _tokenService.GetRefreshTokenExpiry(),
            CreatedByIp = ipAddress
        };
        await _refreshTokenRepository.AddAsync(newRefreshToken);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenPlain,
            AccessTokenExpiresAt = _tokenService.GetAccessTokenExpiry()
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var tokenHash = _hasher.Hash(refreshToken);
        var existingToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);
        if (existingToken is not null)
        {
            existingToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(existingToken);
        }
    }

    public async Task<UserResponse> GetMeAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        return MapToUserResponse(user);
    }

    public async Task<UserResponse> UpdateMeAsync(Guid userId, UpdateMeRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        if (request.Name is not null) user.Name = request.Name;
        if (request.Email is not null) user.Email = request.Email;

        await _userRepository.UpdateAsync(user);
        return MapToUserResponse(user);
    }

    public async Task<List<UserListItemResponse>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserListItemResponse { Name = u.Name, Email = u.Email }).ToList();
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, string? ipAddress)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshTokenPlain = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _hasher.Hash(refreshTokenPlain),
            ExpiresAt = _tokenService.GetRefreshTokenExpiry(),
            CreatedByIp = ipAddress
        };
        await _refreshTokenRepository.AddAsync(refreshToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenPlain,
            AccessTokenExpiresAt = _tokenService.GetAccessTokenExpiry()
        };
    }

    private static UserResponse MapToUserResponse(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        PhoneNumber = user.PhoneNumber,
        Email = user.Email,
        Role = user.Role.ToString(),
        CreatedAt = user.CreatedAt
    };

}