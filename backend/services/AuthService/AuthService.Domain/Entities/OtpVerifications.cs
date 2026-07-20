using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Entities;

public class OtpVerification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PhoneNumber { get; set; } = string.Empty;
    public string OtpCodeHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public int AttemptCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
