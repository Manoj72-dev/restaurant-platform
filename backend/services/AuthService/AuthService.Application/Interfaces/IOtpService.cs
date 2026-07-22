namespace AuthService.Application.Interfaces
{
    public interface IOtpService
    {
        Task GenerateAndSendOtpAsync(string phoneNumber);
        Task<bool> VerifyOtpAsync(string phoneNumber, string otp);
    }
}