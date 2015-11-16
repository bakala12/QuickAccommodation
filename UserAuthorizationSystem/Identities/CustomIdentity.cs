using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Identities
{
    /// <summary>
    /// Reprezentuje Identity usera.
    /// </summary>
    public class CustomIdentity : IIdentity
    {
        /// <summary>
        /// Tworzy nową instancję klasy CustomIdentity.
        /// </summary>
        /// <param name="username">Nazwa użytkownika</param>
        /// <param name="email">Email użytkownika</param>
        /// <param name="roles">Role użytkownika (nieużywane).</param>
        public CustomIdentity(string username, string email=null, string[] roles=null)
        {
            Username = username;
            Email = email;
            Roles = roles ?? new string[]{};
        }

        /// <summary>
        /// Pobiera nazwę użytkownika.
        /// </summary>
        public string Username { get; }
        /// <summary>
        /// Pobiera email użytkownika.
        /// </summary>
        public string Email { get; private set; }
        /// <summary>
        /// Pobiera role użytkownika. (nieuzywane).
        /// </summary>
        public string[] Roles { get; private set; }

        #region IIdentity Members
        /// <summary>
        /// Pobiera typ autoryzacji.
        /// </summary>
        public string AuthenticationType { get { return "Custom authentication"; } }

        /// <summary>
        /// Informuje czy obecna instancja jest uwierzytelniona.
        /// </summary>
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Username); } }

        /// <summary>
        /// Pobiera nazwę użytkownika.
        /// </summary>
        public string Name { get { return Username; } }
        #endregion
    }
}
