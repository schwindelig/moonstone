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
        public static RouteEntry Error = new RouteEntry("Error", "Error", "Index");
        public static RouteEntry Home = new RouteEntry("Home", "Home", "Index");
        public static RouteEntry Logout = new RouteEntry("Logout", "User", "Logout");
        public static RouteEntry SelectGroup = new RouteEntry("SelectGroup", "User", "SelectGroup");
    }
}