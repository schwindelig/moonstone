using Microsoft.AspNet.Identity;
using moonstone.authentication;
using moonstone.authentication.managers;
using moonstone.core.models;
using moonstone.core.services;
using System;
using System.Web;

namespace moonstone.ui.web.Models
{
    public class Current
    {
        public AuthenticationHub Authentication { get; protected set; }

        public ServiceHub Services { get; protected set; }

        public User User
        {
            get
            {
                if (this.UserId.HasValue)
                {
                    return this.Services.UserService.GetUserById(this.UserId.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        public Guid? UserId
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                }
                else
                {
                    return null;
                }
            }
        }

        public Current(AuthenticationHub authHub, ServiceHub serviceHub)
        {
            this.Authentication = authHub;
            this.Services = serviceHub;
        }
    }
}