using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bloghost.Web.Other;

namespace Bloghost.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                name: "0",
                url: "Entry/{id}",
                defaults: new { controller = "Entry", action = "Entry" }
            );

            routes.MapRoute(
                name: "1",
                url: "NewEntry",
                defaults: new { controller = "Entry", action = "Create" }
            );

            routes.MapRoute(
                name: "4",
                url: "EditEntry/{id}",
                defaults: new { controller = "Entry", action = "Edit" }
            );

            routes.MapRoute(
                name: "5",
                url: "DeleteEntry/{id}",
                defaults: new { controller = "Entry", action = "Delete" }
            );

            routes.MapRoute(
                name: "xx",
                url: "EditTag/{id}",
                defaults: new { controller = "Administrator", action = "EditTag" }
            );

            routes.MapRoute(
                name: "2",
                url: "Blog/{id}",
                defaults: new { controller = "Blog", action = "Entries" }
            );

            routes.MapRoute(
                name: "3",
                url: "Blogs",
                defaults: new { controller = "Blog", action = "Blogs" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}