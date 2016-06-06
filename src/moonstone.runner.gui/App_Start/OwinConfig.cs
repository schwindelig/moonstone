using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(moonstone.runner.gui.App_Start.OwinConfig))]

namespace moonstone.runner.gui.App_Start
{
    public class OwinConfig
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
