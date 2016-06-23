namespace moonstone.core.services
{
    public class ServiceHub
    {
        public IBankAccountService BankAccountService { get; protected set; }
        public ICategoryService CategoryService { get; protected set; }
        public IEnvironmentService EnvironmentService { get; protected set; }
        public IGroupService GroupService { get; protected set; }
        public ILoginService LoginService { get; protected set; }
        public ITransactionService TransactionService { get; protected set; }
        public IUserService UserService { get; protected set; }

        public ServiceHub(
            ILoginService loginService,
            IEnvironmentService environmentService,
            IUserService userService,
            IGroupService groupService,
            ICategoryService categoryService,
            IBankAccountService bankAccountService,
            ITransactionService transactionService)
        {
            this.LoginService = loginService;
            this.EnvironmentService = environmentService;
            this.UserService = userService;
            this.GroupService = groupService;
            this.CategoryService = categoryService;
            this.BankAccountService = bankAccountService;
            this.TransactionService = transactionService;
        }
    }
}