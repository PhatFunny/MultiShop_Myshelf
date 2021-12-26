using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiShop_Myself.Startup))]
namespace MultiShop_Myself
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
