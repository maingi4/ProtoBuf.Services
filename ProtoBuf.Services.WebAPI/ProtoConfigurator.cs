using System;
using System.Text;
using System.Web.Http;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Infrastructure.Encryption;
using ProtoBuf.Services.Infrastructure.Exceptions;

namespace ProtoBuf.Services.WebAPI
{
    /// <summary>
    /// Configures the protobuf behaviours for the application
    /// </summary>
    public static class ProtoConfigurator
    {
        public static void ConfigureProtoServices(HttpConfiguration config, ProtoBufConfig protoBufConfig)
        {
            if (protoBufConfig == null)
                throw new ArgumentNullException("protoBufConfig");

            if (config == null)
                throw new ArgumentNullException("config");

            config.Routes.MapHttpRoute(
                name: "protobuf.services.routes.metadata",
                routeTemplate: GetPath(protoBufConfig.PathPrefix),
                defaults: null,
                constraints: null,
                handler: new MetaDataHttpHandler()
            );

            config.Formatters.Add(new ProtoBufMediaTypeFormatter());

            EncryptionManager.EncryptionKey = protoBufConfig.EncryptionKey;
        }

        private static string GetPath(string prefix)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                builder.Append(prefix.TrimEnd('/')).Append('/');
            }
            builder.Append(RestfulServiceConstants.ProtoMetaRouteRoot);

            return builder.ToString();
        }
    }
}
