using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Identities
{
    /// <summary>
    /// Implementacja IPrincipal używana w aplikacji do przechowywania informacji o zalogowanym użytkowniku.
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        /// <summary>
        /// Tworzy nową instancję klasy CustomPrincipal.
        /// </summary>
        public CustomPrincipal() { }

        /// <summary>
        /// Tworzy nową instancję klasy CustomPrincipal jednocześnie inicjalizując właściwość Identity.
        /// </summary>
        /// <param name="username"></param>
        public CustomPrincipal(string username)
        {
            Identity = new CustomIdentity(username);
        }

        private CustomIdentity _identity;

        /// <summary>
        /// Zwraca lub ustawia Identity usera jako CustomIdentity.
        /// </summary>
        public CustomIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        #region IPrincipal Members
        /// <summary>
        /// Zwraca Identity usera.
        /// </summary>
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        /// <summary>
        /// Sprawdza czy użytkownik posiada pewne uprawnienia. Na razie nieużywane.
        /// </summary>
        /// <param name="role">Nazwa uprawnienia.</param>
        /// <returns>Prawda gdy użytkownik posiada uprawnienia, fałsz w przeciwnym przypadku.</returns>
        public bool IsInRole(string role)
        {
            return _identity.Roles.Contains(role);
        }
        #endregion
    }
}
