using UserAuthorizationSystem.Identities;

namespace AccommodationWebPage
{
    public static class AuthorizationConfig
    {
        public static void RegisterAuthorization()
        {
            CustomPrincipal principal = new CustomPrincipal();
            Authorization.Authorization.Current.RegisterCurrentPrincipal(principal);
        } 
    }
}