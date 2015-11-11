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
        public CustomIdentity(string username, string email=null, string[] roles=null)
        {
            Username = username;
            Email = email;
            Roles = roles ?? new string[]{};
        }

        public string Username { get; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Username); } }

        public string Name { get { return Username; } }
        #endregion
    }
}
