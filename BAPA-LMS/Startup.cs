using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BAPA_LMS.Startup))]
namespace BAPA_LMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
