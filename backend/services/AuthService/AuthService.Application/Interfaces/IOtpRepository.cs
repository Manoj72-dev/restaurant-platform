using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(OtpVerification otp);
        Task<OtpVerification?> GetLatestActiveByPhoneAsync(string phoneNumber);
        Task UpdateAsync(OtpVerification otp);
    }
}
