using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bloghost.Web.Other
{
    public class GuidIDRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var routeData = requestContext.RouteData;
            var strValue = routeData.Values["id"].ToString();
            Guid guidValue;
            var action = routeData.Values["action"];
            if (Guid.TryParse(strValue, out guidValue) && guidValue != Guid.Empty)
            {
                routeData.Values["action"] = action + "Guid";
            }
            var handler = new MvcHandler(requestContext);
            return handler;
        }
    }
}