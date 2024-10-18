using System.Security.Cryptography;

namespace MovieApi.Utilities
{
    public class PasswordUtil
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Generate salt acak
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] key = pbkdf2.GetBytes(KeySize);
                    return (Convert.ToBase64String(key), Convert.ToBase64String(salt));
                }
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                byte[] key = pbkdf2.GetBytes(KeySize);
                return Convert.ToBase64String(key) == hashedPassword;
            }
        }
    }
}