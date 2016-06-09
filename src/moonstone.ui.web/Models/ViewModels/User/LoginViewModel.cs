namespace moonstone.ui.web.Models.ViewModels.User
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string ReturnUrl { get; set; }
    }
}