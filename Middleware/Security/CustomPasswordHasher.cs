using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Security
{
    public class CustomPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 600_000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be empty.");
            }

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Ye .NET ki built-in cryptographic API hai jo PBKDF2 (Password-Based Key Derivation Function 2) algorithm implement karti hai.
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string storedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(storedPassword))
                return false;

            try
            {
                string[] parts = storedPassword.Split('.');

                if (parts.Length != 3 ||
                    !int.TryParse(parts[0], out int iterations) ||
                    iterations < 1 ||
                    iterations > Iterations)
                    return false;

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                if (salt.Length != SaltSize || expectedHash.Length != HashSize)
                    return false;

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    HashSize);

                return CryptographicOperations.FixedTimeEquals(actualHash,expectedHash);
            }

            catch (FormatException)
            {
                return false;
            }

            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}

    
