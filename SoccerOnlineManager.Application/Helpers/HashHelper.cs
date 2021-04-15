using SoccerOnlineManager.Application.Exceptions;
using System;
using System.Text;

namespace SoccerOnlineManager.Application.Helpers
{
    public class HashHelper
    {
        public static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(ExceptionCodes.Empty, nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(ExceptionCodes.Empty, nameof(password));

            if (storedHash.Length != 64)
                throw new ArgumentException(ExceptionCodes.InvalidLength, nameof(storedHash));

            if (storedSalt.Length != 128)
                throw new ArgumentException(ExceptionCodes.InvalidLength, nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
