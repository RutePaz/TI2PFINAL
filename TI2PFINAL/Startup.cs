using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TI2PFINAL.Startup))]
namespace TI2PFINAL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
