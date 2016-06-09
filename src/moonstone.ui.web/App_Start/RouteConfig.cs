using System.Web.Mvc;
using System.Web.Routing;

namespace moonstone.ui.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: Config.Get().Routes.Home,
                url: "Home/Index");

            routes.MapRoute(
                name: Config.Get().Routes.Logout,
                url: "User/Logout");
        }
    }
}