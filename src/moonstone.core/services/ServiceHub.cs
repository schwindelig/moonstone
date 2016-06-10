namespace moonstone.core.services
{
    public class ServiceHub
    {
        public IEnvironmentService EnvironmentService { get; protected set; }
        public ILoginService LoginService { get; protected set; }

        public ServiceHub(ILoginService loginService, IEnvironmentService environmentService)
        {
            this.LoginService = loginService;
            this.EnvironmentService = environmentService;
        }
    }
}