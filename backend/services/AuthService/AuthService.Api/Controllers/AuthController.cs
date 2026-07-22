using AuthService.Application.DTOs.Requests;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    private string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(SendOtpRequest request)
    {
        await _authService.SendOtpAsync(request);
        return Ok(new { message = "OTP sent." });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        try
        {
            var result = await _authService.VerifyOtpAsync(request, ClientIp);
            return Ok(result);
        }catch(InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequest request)
    {
        await _authService.ResendOtpAsync(request);
        return Ok(new { message = "OTP resent." });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request, ClientIp);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequest request)
    {
        await _authService.LogoutAsync(request.RefreshToken);
        return Ok(new { message = "Logged out." });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var result = await _authService.GetMeAsync(CurrentUserId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe(UpdateMeRequest request)
    {
        var result = await _authService.UpdateMeAsync(CurrentUserId, request);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _authService.GetAllUsersAsync();
        return Ok(result);
    }
}