using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Registration
{
    /// <summary>
    /// Klasa pomocna przy haszowaniu haseł
    /// </summary>
    internal static class PasswordHashHelper
    {
        private static readonly uint SaltByteSize=24;

        /// <summary>
        /// Tworzy losową kryptograficznie bezpieczną sól do hasła
        /// </summary>
        /// <returns>Kryptograficznie bezpieczna sól do hasła</returns>
        internal static string CreateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt=new byte[SaltByteSize];
            provider.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Wylicza hasz dla podanego hasła i soli.
        /// </summary>
        /// <param name="clearTextPassword">Hasło użytkownika jako zwykły tekst</param>
        /// <param name="salt">Sól do hasła</param>
        /// <returns>Zahaszowane i osolone hasło gotowe do zapisu w bazie danych</returns>
        public static string CalculateHash(string clearTextPassword, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearTextPassword + salt);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hashedBytes = algorithm.ComputeHash(bytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
