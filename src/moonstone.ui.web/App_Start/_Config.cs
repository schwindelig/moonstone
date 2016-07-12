using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace moonstone.ui.web
{
    public class Config
    {
        public Connection Connection { get; set; }

        public Routes Routes { get; set; }

        public static Config Get()
        {
            return new Config()
            {
                Connection = new Connection()
                {
                    Server = ".",
                    Database = "moonstone_dev",
                    Username = "moonstone_ui",
                    Password = "moonstone_ui"
                },
                Routes = new Routes()
            };
        }
    }

    public class Connection
    {
        public string Database { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
    }

    public class RouteEntry
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Name { get; set; }

        public RouteEntry(string name, string controller, string action)
        {
            this.Name = name;
            this.Controller = controller;
            this.Action = action;
        }

        public string GetActionLink(UrlHelper helper)
        {
            return helper.Action(this.Action, this.Controller);
        }

        public bool IsSame(RouteData routeData)
        {
            return
                routeData.Values["controller"].ToString().Equals(this.Controller, System.StringComparison.InvariantCultureIgnoreCase) &&
                routeData.Values["action"].ToString().Equals(this.Action, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public void Map(RouteCollection routes)
        {
            routes.MapRoute(
                name: this.Name,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = this.Controller, action = this.Action, id = UrlParameter.Optional });
        }
    }

    public class Routes
    {
        public static RouteEntry CreateBankAccount = new RouteEntry("CreateBankAccount", "BankAccount", "Create");
        public static RouteEntry CreateCategory = new RouteEntry("CreateCategory", "Category", "Create");
        public static RouteEntry CreateGroup = new RouteEntry("CreateGroup", "Group", "Create");
        public static RouteEntry CreateTransaction = new RouteEntry("CreateTransaction", "Transaction", "Create");
        public static RouteEntry Error = new RouteEntry("Error", "Error", "Index");
        public static RouteEntry Home = new RouteEntry("Home", "Home", "Index");
        public static RouteEntry IndexBankAccounts = new RouteEntry("IndexBankAccounts", "BankAccount", "Index");
        public static RouteEntry IndexCategories = new RouteEntry("IndexCategories", "Category", "Index");
        public static RouteEntry IndexGroups = new RouteEntry("IndexGroups", "Group", "Index");
        public static RouteEntry IndexTransactions = new RouteEntry("IndexTransactions", "Transaction", "Index");
        public static RouteEntry LoggedOut = new RouteEntry("LoggedOut", "User", "LoggedOut");
        public static RouteEntry Login = new RouteEntry("Login", "User", "Login");
        public static RouteEntry Logout = new RouteEntry("Logout", "User", "Logout");
        public static RouteEntry SelectGroup = new RouteEntry("SelectGroup", "User", "SelectGroup");
    }
}