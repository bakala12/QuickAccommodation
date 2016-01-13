using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccommodationWebPage.Startup))]
namespace AccommodationWebPage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
