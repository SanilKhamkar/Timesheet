using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace emptime
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
                name: "Registraton",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Registration", action = "RegistrationPage", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "LoginPage", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Timesheet",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Timesheet", action = "Punchin", id = UrlParameter.Optional }
            );

        }
    }
}
