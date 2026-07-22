using AuthService.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure.Security
{
    public class Sha256Hasher : IHasher
    {
        public string Hash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public bool Verify(string input, string hash)
        {
            Console.WriteLine($"Verifying input: {input} against hash: {hash}");
            return Hash(input) == hash;
        }
    }
}
