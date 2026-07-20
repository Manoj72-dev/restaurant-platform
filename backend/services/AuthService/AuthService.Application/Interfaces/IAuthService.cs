using AuthService.Application.DTOs.Requests;
using AuthService.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task SendOtpAsync(SendOtpRequest request);
        Task<AuthResponse> VerifyOtpAsync(VerifyOtpRequest request, string? ipAddress);
        Task ResendOtpAsync(ResendOtpRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, string? ipAddress);
        Task LogoutAsync(string refreshToken);
        Task<UserResponse> GetMeAsync(Guid userId);
        Task<UserResponse> UpdateMeAsync(Guid userId, UpdateMeRequest request);
        Task<List<UserListItemResponse>> GetAllUsersAsync();
    }
}
