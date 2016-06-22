using Microsoft.AspNet.Identity;
using System;

namespace moonstone.core.models
{
    public class User : IUser<Guid>
    {
        public int AccessFailedCount { get; set; }

        public DateTime CreateDateUtc { get; set; }
        public string Culture { get; set; }
        public Guid? CurrentGroupId { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public bool IsLockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
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