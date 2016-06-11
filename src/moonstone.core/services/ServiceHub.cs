﻿namespace moonstone.core.services
{
    public class ServiceHub
    {
        public IEnvironmentService EnvironmentService { get; protected set; }
        public ILoginService LoginService { get; protected set; }
        public IUserService UserService { get; protected set; }

        public ServiceHub(ILoginService loginService, IEnvironmentService environmentService, IUserService userService)
        {
            this.LoginService = loginService;
            this.EnvironmentService = environmentService;
            this.UserService = userService;
        }
    }
}