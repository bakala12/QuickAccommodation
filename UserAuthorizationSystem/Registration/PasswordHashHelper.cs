using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Registration
{
    internal static class PasswordHashHelper
    {
        private static readonly uint SaltByteSize=24;

        internal static string CreateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt=new byte[SaltByteSize];
            provider.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string CalculateHash(string clearTextPassword, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hashedBytes = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
