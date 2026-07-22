using AuthService.Application.Interfaces;
using Microsoft.Extensions.Logging;
namespace AuthService.Infrastructure.Services
{
    public class ConsoleSmsSender: ISmsSender
    {
        private readonly ILogger<ConsoleSmsSender> _logger;

        public ConsoleSmsSender(ILogger<ConsoleSmsSender> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string phoneNumber, string message)
        {
            _logger.LogInformation("SMS to {Phone} : {Message}", phoneNumber, message);
            return Task.CompletedTask;
        }
    }
}
