using System.Web.Http;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.WebAPI
{
    public static class ProtoConfigurator
    {
        public static void ConfigureProtoServices(HttpConfiguration config)
        {
            AppMode.Mode = AppMode.ModeType.WebAPI;

            config.Routes.MapHttpRoute(
                name: "protobuf.services.routes.metadata",
                routeTemplate: "api/" + Constants.ProtoMetaRouteRoot + "/{name}",
                defaults: new { name = RouteParameter.Optional },
                constraints: null,
                handler: new MetaDataHttpHandler()
            );
        }
    }
}
