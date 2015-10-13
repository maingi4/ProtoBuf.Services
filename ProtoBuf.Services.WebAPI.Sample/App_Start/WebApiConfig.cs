using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProtoBuf.Services.WebAPI.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void MapProtoRoute(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "protobuf.services.routes.metadata",
                routeTemplate: "protobuf_services_meta/",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
