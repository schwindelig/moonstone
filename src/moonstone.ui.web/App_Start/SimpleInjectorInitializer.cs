[assembly: WebActivator.PostApplicationStartMethod(typeof(moonstone.ui.web.SimpleInjectorInitializer), "Initialize")]

namespace moonstone.ui.web
{
    using authentication.managers;
    using authentication.services;
    using authentication.stores;
    using core.models;
    using core.repositories;
    using core.services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using services;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using sql.context;
    using sql.repositories;
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            //container.Verify(); // otherwise we get this exception: No owin.Environment item was found in the context.

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            var config = Config.Get();

            // SQL Context
            container.Register<SqlContext>(() =>
                {
                    var ctx = new SqlContext(
                        config.Connection.Database,
                        config.Connection.Server,
                        config.Connection.Username,
                        config.Connection.Password);

                    ctx.RegisterModelDescription<User>(sql.configs.ModelDescriptions.User());
                    ctx.RegisterModelDescription<Group>(sql.configs.ModelDescriptions.Group());
                    ctx.RegisterModelDescription<GroupUser>(sql.configs.ModelDescriptions.GroupUser());
                    ctx.RegisterModelDescription<Category>(sql.configs.ModelDescriptions.Category());
                    ctx.RegisterModelDescription<BankAccount>(sql.configs.ModelDescriptions.BankAccount());

                    return ctx;
                },
                Lifestyle.Scoped);

            // Repositories
            container.Register<IUserRepository, SqlUserRepository>();
            container.Register<IGroupRepository, SqlGroupRepository>();
            container.Register<IGroupUserRepository, SqlGroupUserRepository>();
            container.Register<ICategoryRepository, SqlCategoryRepository>();
            container.Register<IBankAccountRepository, SqlBankAccountRepository>();

            // Authentication
            container.Register<IUserStore<User, Guid>, UserStore>();
            container.Register<UserManager>();
            container.Register<IAuthenticationManager>(() => HttpContext.Current.GetOwinContext().Authentication);
            container.Register<SignInManager>();

            // Services
            container.Register<ILoginService, LoginService>();
            container.Register<IEnvironmentService, EnvironmentService>();
            container.Register<IUserService, UserService>();
            container.Register<IGroupService, GroupService>();
            container.Register<ICategoryService, CategoryService>();
            container.Register<IBankAccountService, BankAccountService>();
        }
    }
}