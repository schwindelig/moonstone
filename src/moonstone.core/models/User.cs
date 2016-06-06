using Microsoft.AspNet.Identity;
using System;

namespace moonstone.core.models
{
    public class User : IUser<Guid>
    {
        public string Email { get; set; }

        public Guid Id { get; set; }

        public string PasswordHash { get; set; }

        public string UserName
        {
            get
            {
                return this.Email;
            }
            set
            {
                this.Email = value;
            }
        }
    }
}