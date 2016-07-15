using moonstone.authentication;
using moonstone.core.i18n;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using moonstone.services;
using moonstone.sql.configs;
using moonstone.sql.context;
using moonstone.sql.repositories;
using System;

namespace moonstone.tests.common
{
    public class TestProvider
    {
        public const string NEW_USER_DEFAULT_LANGUAGE = "de-CH";
        private const string DATABASE = "moonstone_dev_tests";
        private const string PASSWORD = "moonstone_ui";
        private const string SERVER = ".";
        private const string USER = "moonstone_ui";

        public static BankAccount CreateNewBankAccount(IBankAccountRepository bankAccountRepo, BankAccount bankAccount)
        {
            return bankAccountRepo.GetById(
                bankAccountRepo.Create(bankAccount));
        }

        public static Category CreateNewCategory(ICategoryRepository categoryRepository, Guid createUserId, Guid groupId)
        {
            return categoryRepository.GetById(
                categoryRepository.Create(TestProvider.GetNewCategory(createUserId, groupId)));
        }

        public static Group CreateNewGroup(IGroupRepository groupRepo, Guid creatorId)
        {
            return groupRepo.GetById(
                groupRepo.Create(new Group { CreateDateUtc = DateTime.UtcNow, Description = "Description ...", CreateUserId = creatorId, Name = Guid.NewGuid().ToString() }));
        }

        public static User CreateNewUser(IUserRepository userRepo)
        {
            return userRepo.GetById(
                userRepo.Create(TestProvider.GetNewUser()));
        }

        public static BankAccount GetNewBankAccount(Guid createUserId, Guid groupId)
        {
            return new BankAccount
            {
                CreateUserId = createUserId,
                Description = $"Description_{Guid.NewGuid()}",
                GroupId = groupId,
                Name = $"BankAccount_{Guid.NewGuid()}"
            };
        }

        public static Category GetNewCategory(Guid createUserId, Guid groupId)
        {
            var category = new Category()
            {
                CreateUserId = createUserId,
                GroupId = groupId,
                Name = $"Category_{Guid.NewGuid()}"
            };

            return category;
        }

        public static Group GetNewGroup(Guid createUserId)
        {
            return new Group
            {
                CreateDateUtc = DateTime.UtcNow,
                CreateUserId = createUserId,
                Description = "Description " + Guid.NewGuid(),
                Name = $"Group {Guid.NewGuid().ToString()}"
            };
        }

        public static User GetNewUser(Guid? currentGroupId = null)
        {
            var user = new User
            {
                Email = $"{Guid.NewGuid()}@schwindelig.ch",
                PasswordHash = "h4$h",
                Culture = NEW_USER_DEFAULT_LANGUAGE,
                CreateDateUtc = DateTime.UtcNow,
                CurrentGroupId = currentGroupId
            };

            return user;
        }

        public static RepositoryHub GetRepositoryHub(SqlContext context)
        {
            return new RepositoryHub(
                new SqlUserRepository(context),
                new SqlGroupRepository(context),
                new SqlGroupUserRepository(context),
                new SqlCategoryRepository(context),
                new SqlBankAccountRepository(context),
                new SqlTransactionRepository(context));
        }

        public static ServiceHub GetServiceHub(RepositoryHub repoHub)
        {
            ILoginService loginService = null;
            IEnvironmentService environmentService = new EnvironmentService(repoHub, new CultureNinja());
            IGroupService groupService = new GroupService(repoHub);
            IUserService userService = new UserService(repoHub, groupService);
            ICategoryService categoryService = new CategoryService(repoHub, groupService);
            IBankAccountService bankAccountservice = new BankAccountService(repoHub, groupService);
            ITransactionService transactionService = new TransactionService(repoHub);
            ICurrencyService currencyService = new CurrencyService(repoHub);

            return new ServiceHub(
                loginService: loginService, /* until we figure out how to get the crap owin context thingy working in nunit */
                environmentService: environmentService,
                userService: userService,
                groupService: groupService,
                categoryService: categoryService,
                bankAccountService: bankAccountservice,
                transactionService: transactionService,
                currencyService: currencyService);
        }

        public static SqlContext GetSqlContext()
        {
            var context = new SqlContext(DATABASE, SERVER, USER, PASSWORD);
            context.RegisterModelDescription(ModelDescriptions.User());
            context.RegisterModelDescription(ModelDescriptions.Group());
            context.RegisterModelDescription(ModelDescriptions.GroupUser());
            context.RegisterModelDescription(ModelDescriptions.Category());
            context.RegisterModelDescription(ModelDescriptions.BankAccount());

            return context;
        }
    }
}