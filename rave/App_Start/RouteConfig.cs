using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rave
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MyFirst", action = "login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "Default1",
                 url: "{controller}/{action}/{id}",
                 defaults: new
                 {
                     controller = "Signup",
                     action = "rave_signup",
                     id = UrlParameter.Optional
                 }
                );
        }
    }
}
