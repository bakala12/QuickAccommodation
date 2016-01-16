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
    internal sealed class Authenticator
    {
        private Authenticator() { }

        private readonly IUserAuthenticationService _authenticationService = new UserAuthenticationService();
        private static readonly object SyncRoot = new object();
        private static Authenticator _instance;

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

        public SignInResult SignIn(string username, string password)
        {
            CustomIdentity identity =
                _authenticationService.AuthenticateUser<AccommodationContext>(username, password);
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

        public CustomPrincipal Authenticate(string token)
        {
            CustomPrincipal user;
            SignedInUsers.TryGetValue(token, out user);
            return user;
        }

        private static string GetToken()
        {
            byte[] data = new byte[12];
            CryptoProvider.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public sealed class SignInResult
        {
            public CustomPrincipal User { get; }
            public string Token { get; }

            public SignInResult(CustomPrincipal user, string token)
            {
                this.User = user;
                this.Token = token;
            }
        }
    }
}