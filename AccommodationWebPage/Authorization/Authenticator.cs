using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Domain;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace AccommodationWebPage.Authorization
{
    /// <summary>
    /// Singleton wspomagający autoryzację i autentykację użytkownika.
    /// </summary>
    internal sealed class Authenticator
    {
        /// <summary>
        /// Prywatny konstruktor.
        /// </summary>
        private Authenticator() { }

        private readonly IUserAuthenticationService _authenticationService = new UserAuthenticationService();
        private static readonly object SyncRoot = new object();
        private static Authenticator _instance;

        /// <summary>
        /// Pobiera instancję singletonu.
        /// </summary>
        internal static Authenticator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Authenticator();
                    }
                }
                return _instance;
            }
        }

        private static readonly RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider();
        private static readonly ConcurrentDictionary<string, CustomPrincipal> SignedInUsers = new ConcurrentDictionary<string, CustomPrincipal>();

        /// <summary>
        /// Loguje uzytkownika w aplikacji.
        /// </summary>
        /// <param name="context">Kontekst bazy danych.</param>
        /// <param name="username">Nazwa użytkownika.</param>
        /// <param name="password">Hasło użytkownika.</param>
        /// <returns>SignInResult dla usera, jeśli wszystko przebiegło poprawnie, w przeciwnym razie null.</returns>
        public SignInResult SignIn(IUsersContext context, string username, string password)
        {
            CustomIdentity identity =
                _authenticationService.AuthenticateUser(context, username, password);
            if (identity != null)
            {
                var user = new CustomPrincipal(username);
                var token = GetToken();
                while (!SignedInUsers.TryAdd(token, user)) //trzeba wygenerować unikalny token
                {
                    token = GetToken();
                }
                return new SignInResult(user, token);
            }
            return null;
        }

        /// <summary>
        /// Autentykuje podany token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>CustomPrincipal dla uzytkownika lub null.</returns>
        public CustomPrincipal Authenticate(string token)
        {
            CustomPrincipal user;
            SignedInUsers.TryGetValue(token, out user);
            return user;
        }

        /// <summary>
        /// Zwraca nowy kryptograficznie bezpieczny token.
        /// </summary>
        /// <returns>Token.</returns>
        private static string GetToken()
        {
            byte[] data = new byte[12];
            CryptoProvider.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Klasa reprezentująca status zalogowania.
        /// </summary>
        public sealed class SignInResult
        {
            /// <summary>
            /// CustomPrincipal usera.
            /// </summary>
            public CustomPrincipal User { get; }

            /// <summary>
            /// Token uzytkownika.
            /// </summary>
            public string Token { get; }

            /// <summary>
            /// Inicjalizuje nową instancję klasy SignInResult.
            /// </summary>
            /// <param name="user">CustomPrincipal usera.</param>
            /// <param name="token">Token użytkownika.</param>
            public SignInResult(CustomPrincipal user, string token)
            {
                this.User = user;
                this.Token = token;
            }
        }
    }
}