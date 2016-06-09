using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(moonstone.ui.web.Startup))]

namespace moonstone.ui.web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OwinConfig.Configuration(app);
        }
    }
}