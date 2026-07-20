using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Domain.Enums;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } 
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool IsPhoneVerified { get; set; } = false;
        public UserRole Role { get; set; } = UserRole.Customer;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
