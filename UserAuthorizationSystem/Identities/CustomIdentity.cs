using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthorizationSystem.Identities
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string username, string email, string[] roles)
        {
            Username = username;
            Email = email;
            Roles = roles;
        }

        public string Username { get; private set; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Username); } }

        public string Name { get { return Username; } }
        #endregion
    }
}
