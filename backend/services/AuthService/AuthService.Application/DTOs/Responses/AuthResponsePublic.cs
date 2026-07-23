namespace AuthService.Application.DTOs.Responses
{
    public class AuthResponsePublic
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
    }
}
