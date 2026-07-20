using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure.Security
{
    public static class HashHelper
    {
        public static string Hash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public static bool Verify(string input, string hash)
            => Hash(input) == hash;
    }
}
