using System.Security.Cryptography;
using System.Text;

namespace JoygameProject.Application.Helpers
{
    public static class HashingHelper
    {
        public static string Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}


