using System;
using System.Security.Cryptography;

namespace bai_4._1
{
    public static class PasswordHelper
    {
        public static string NewSalt()
        {
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string Hash(string password, string salt)
        {
            using (var kdf = new Rfc2898DeriveBytes(
                password, Convert.FromBase64String(salt), 100000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(kdf.GetBytes(32));
            }
        }
    }
}