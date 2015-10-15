using System.Text;
using System.Web.Http;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.WebAPI
{
    public static class ProtoConfigurator
    {
        public static void ConfigureProtoServices(HttpConfiguration config, string pathPrefix = "api")
        {
            AppMode.Mode = AppMode.ModeType.WebAPI;

            config.Routes.MapHttpRoute(
                name: "protobuf.services.routes.metadata",
                routeTemplate: GetPath(pathPrefix),
                defaults: new { name = RouteParameter.Optional },
                constraints: null,
                handler: new MetaDataHttpHandler()
            );

            config.Formatters.Add(new ProtoBufMediaTypeFormatter());
        }

        private static string GetPath(string prefix)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                builder.Append(prefix.TrimEnd('/')).Append('/');
            }
            builder.Append(Constants.ProtoMetaRouteRoot).Append("/{name}");

            return builder.ToString();
        }
    }
}
