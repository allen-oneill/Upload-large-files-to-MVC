using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCApp.Startup))]
namespace MVCApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
