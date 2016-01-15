using System;
using System.Security.Principal;
using UserAuthorizationSystem.Identities;

namespace AccommodationWebPage.Authorization
{
    public class Authorization
    {
        private Authorization() { }

        private static Authorization _current;
        private static readonly object SyncRoot = new object();

        public static Authorization Current
        {
            get
            {
                if (_current == null)
                {
                    lock (SyncRoot)
                    {
                        if(_current==null)
                            _current = new Authorization();
                    }
                }
                return _current;
            }
        }

        public IPrincipal CurrentPrincipal { get; private set; }
        public bool IsAuthenticated => CurrentPrincipal.Identity.IsAuthenticated;
        public string AuthenticatedUser => CurrentPrincipal.Identity.Name;

        internal void RegisterCurrentPrincipal(CustomPrincipal principal)
        {
            CurrentPrincipal = principal;
        }

        public void Login(CustomIdentity identity)
        {
            if (CurrentPrincipal == null) throw new InvalidOperationException("Current principal is null");
            var customPrincipal = CurrentPrincipal as CustomPrincipal;
            if (customPrincipal != null) customPrincipal.Identity = identity;
        }

        public void Logout()
        {
            if (CurrentPrincipal == null) throw new InvalidOperationException("Current principal is null");
            var customPrincipal = CurrentPrincipal as CustomPrincipal;
            if(customPrincipal!=null) customPrincipal.Identity = new AnonymousIdentity();
        }
    }
}