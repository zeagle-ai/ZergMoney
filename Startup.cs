using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZergMoney.Startup))]
namespace ZergMoney
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
