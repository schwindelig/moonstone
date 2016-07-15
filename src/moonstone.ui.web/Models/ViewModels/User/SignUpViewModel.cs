using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace moonstone.ui.web.Models.ViewModels.User
{
    public class SignUpViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}