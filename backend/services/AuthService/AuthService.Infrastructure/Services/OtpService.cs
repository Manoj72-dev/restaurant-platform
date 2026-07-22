using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IHasher _hasher;
        private readonly ISmsSender _smsSender;
        private const int OtpLength = 6;
        private const int ExpiryMinutes = 5;
        private const int MaxAttempts = 5;

        public OtpService(IOtpRepository otpRepository, IHasher hasher, ISmsSender smsSender)
        {
            _otpRepository = otpRepository;
            _hasher = hasher;
            _smsSender = smsSender;
        }

        public async Task GenerateAndSendOtpAsync(string phoneNumber)
        {
            var otp = Random.Shared.Next(0, 1000000).ToString($"D{OtpLength}");

            var record = new OtpVerification
            {
                PhoneNumber = phoneNumber,
                OtpCodeHash = _hasher.Hash(otp),
                ExpiresAt = DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                IsUsed = false,
                AttemptCount = 0
            };

            await _otpRepository.AddAsync(record);
            await _smsSender.SendAsync(phoneNumber, $"Your verification code is {otp}. Valid for {ExpiryMinutes} minutes.");
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
        {
            var record = await _otpRepository.GetLatestActiveByPhoneAsync(phoneNumber);
            if (record is null || record.IsUsed || DateTime.UtcNow > record.ExpiresAt)
                return false;

            if (record.AttemptCount >= MaxAttempts)
                return false;

            record.AttemptCount++;
             
            if(!_hasher.Verify(otp, record.OtpCodeHash))
            {
                await _otpRepository.UpdateAsync(record);
                return false;
            }
            record.IsUsed = true;
            await _otpRepository.UpdateAsync(record);
            return true;
        }
    }
}
