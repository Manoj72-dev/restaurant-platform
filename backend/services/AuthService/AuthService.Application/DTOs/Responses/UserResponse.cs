namespace AuthService.Application.DTOs.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}