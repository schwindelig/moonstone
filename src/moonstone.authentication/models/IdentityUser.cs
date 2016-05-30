﻿using Microsoft.AspNet.Identity;
using moonstone.core.models;
using System;

namespace moonstone.authentication.models
{
    public class IdentityUser : User, IUser<Guid>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the email-address.
        /// Needed for asp.net identity.
        /// </summary>
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