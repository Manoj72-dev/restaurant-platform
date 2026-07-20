using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        DateTime GetAccessTokenExpiry();
        DateTime GetRefreshTokenExpiry();
    }
}
