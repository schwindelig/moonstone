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

            Routes.Home.Map(routes);
            Routes.Error.Map(routes);
            Routes.Logout.Map(routes);
            Routes.SelectGroup.Map(routes);
        }
    }
}