﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProtoBuf.Services.WebAPI.Sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            ProtoConfigurator.ConfigureProtoServices(config, new ProtoBufConfig("1234567891234567"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = "Index" }
            );
        }
    }
}
