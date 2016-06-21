namespace moonstone.core.services
{
    public class ServiceHub
    {
        public ICategoryService CategoryService { get; protected set; }
        public IEnvironmentService EnvironmentService { get; protected set; }
        public IGroupService GroupService { get; protected set; }
        public ILoginService LoginService { get; protected set; }
        public IUserService UserService { get; protected set; }

        public ServiceHub(ILoginService loginService, IEnvironmentService environmentService, IUserService userService, IGroupService groupService, ICategoryService categoryService)
        {
            this.LoginService = loginService;
            this.EnvironmentService = environmentService;
            this.UserService = userService;
            this.GroupService = groupService;
            this.CategoryService = categoryService;
        }
    }
}