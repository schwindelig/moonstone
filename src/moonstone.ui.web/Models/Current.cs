using moonstone.authentication;
using moonstone.authentication.managers;
using moonstone.core.services;

namespace moonstone.ui.web.Models
{
    public class Current
    {
        public AuthenticationHub AuthenticationHub { get; set; }

        public ServiceHub ServiceHub { get; set; }

        public Current(AuthenticationHub authHub, ServiceHub serviceHub)
        {
            this.AuthenticationHub = authHub;
            this.ServiceHub = serviceHub;
        }
    }
}