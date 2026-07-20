namespace AuthService.Application.DTOs.Requests;
public class ResendOtpRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
}