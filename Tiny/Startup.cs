using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tiny.Startup))]
namespace Tiny
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
