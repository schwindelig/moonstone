namespace moonstone.core.services
{
    public class ServiceHub
    {
        public ILoginService LoginService { get; set; }

        public ServiceHub(ILoginService loginService)
        {
            this.LoginService = loginService;
        }
    }
}