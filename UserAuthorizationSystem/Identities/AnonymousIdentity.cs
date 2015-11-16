using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Identities
{
    /// <summary>
    /// Reprezentuje Identity niezalogowanego użytkownika.
    /// </summary>
    public class AnonymousIdentity : CustomIdentity
    {
        /// <summary>
        /// Tworzy nową instancję niezalogowanego Identity uzytkownika nieuwierzytelnionego.
        /// </summary>
        public AnonymousIdentity() :
            base(string.Empty, string.Empty, new string[] { })
        { }
    }
}
