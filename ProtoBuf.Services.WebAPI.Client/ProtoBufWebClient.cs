using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Serialization;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Services.WebAPI.Client
{
    public class ProtoBufWebClient : IWebClient
    {
        #region Fields

        private static readonly ConcurrentDictionary<string, TypeMetaData> MetaDatas = new ConcurrentDictionary<string, TypeMetaData>();

        #endregion

        #region IWebClient Members

        public TRS SendRequest<TRS>(ProtoRequest protoRequest)
        {
            if (protoRequest == null)
                throw new ArgumentNullException("protoRequest");

            var requestPayload = protoRequest.Request == null ? null : JsonSerializer.ConvertToJson(protoRequest.Request);

            var requestHeaders = protoRequest.RequestHeaders ?? new Dictionary<string, string>();

            string existingAcceptTypes;
            requestHeaders.TryGetValue("accept", out existingAcceptTypes);

            requestHeaders["accept"] = RestfulServiceConstants.ProtoContentType +
                                       (string.IsNullOrEmpty(existingAcceptTypes)
                                            ? string.Empty
                                            : "," + existingAcceptTypes);

            requestHeaders["content-type"] = RestfulServiceConstants.JsonContentType;

            byte[] response;
            WebHeaderCollection responseHeaders;
            using (var client = new ExtendedWebClient(protoRequest.Timeout))
            {
                foreach (var requestHeader in requestHeaders)
                {
                    client.Headers.Add(requestHeader.Key, requestHeader.Value);
                }

                var method = protoRequest.Method ?? "GET";

                if (string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    response = client.DownloadData(protoRequest.ServiceUri);
                }
                else
                {
                    byte[] requestPayloadBytes;

                    if (string.IsNullOrEmpty(requestPayload))
                    {
                        requestPayloadBytes = new byte[0];
                    }
                    else
                    {
                        requestPayloadBytes = Encoding.UTF8.GetBytes(requestPayload);
                    }
                    response = client.UploadData(protoRequest.ServiceUri, method, requestPayloadBytes);
                }
                
                responseHeaders = client.ResponseHeaders;
            }

            var responseContentType = responseHeaders.Get("content-type");

            if (responseContentType.Equals(RestfulServiceConstants.ProtoContentType, StringComparison.OrdinalIgnoreCase))
            {
                var modelKey = responseHeaders.Get(RestfulServiceConstants.RqModelTypeHeaderKey);

                var metaDataKey = GetMetaDataKey(protoRequest, typeof (TRS));

                var metaData = MetaDatas.GetOrAdd(metaDataKey, s => GetMetaData(protoRequest, modelKey));

                var serializer = ObjectBuilder.GetSerializer();

                try
                {
                    return serializer.Deserialize<TRS>(response, metaData);
                }
                catch
                {
                    ClearMetaData(protoRequest, typeof(TRS));
                    throw;
                }
            }
            
            if (IsContentTypeSupported(responseContentType)) //deliberately put before json, in case custom json serialization is required.
            {
                return DeserializeData<TRS>(responseContentType, response);
            }

            if (responseContentType.Equals(RestfulServiceConstants.JsonContentType, StringComparison.OrdinalIgnoreCase))
            {
                return JsonSerializer.FromJson<TRS>(response);
            }
            
            throw new InvalidDataException(string.Format(
                "The response returned by the server did not contain a supported content-type header value, the returned content type was '{0}', supported types are '{1}' & {2}",
                responseContentType, RestfulServiceConstants.ProtoContentType, RestfulServiceConstants.JsonContentType));
        }
        
        #endregion

        #region Custom Content Type

        protected virtual bool IsContentTypeSupported(string contentType)
        {
            return false;
        }

        protected virtual T DeserializeData<T>(string contentType, byte[] data)
        {
            throw new NotSupportedException("This operation is not supported on this class, a child class should override this method.");
        }

        #endregion

        #region Meta Data Related

        private static string GetMetaDataKey(ProtoRequest request, Type targetType)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (targetType == null)
                throw new ArgumentNullException("targetType");

            return string.Join("-", request.ServiceUri.AbsoluteUri, targetType.FullName);
        }

        private static void ClearMetaData(ProtoRequest request, Type targetType)
        {
            var key = GetMetaDataKey(request, targetType);

            TypeMetaData metaData;
            MetaDatas.TryRemove(key, out metaData);
        }

        private static TypeMetaData GetMetaData(ProtoRequest request, string modelKey)
        {
            var metaDataUri = request.ServiceMetaDataUri ?? (GetDefaultMetaDataUri(request));

            byte[] response;
            using (var client = new ExtendedWebClient(request.Timeout)) //yes, the timeout was meant for the entire request, however meta request is made the first time only, something has to be taken to take into account gateway timeouts etc configured in the system.
            {
                client.Headers.Add(RestfulServiceConstants.RqModelTypeHeaderKey, modelKey);
                client.Headers.Add("accept", RestfulServiceConstants.JsonContentType);

                response = client.DownloadData(metaDataUri);
            }

            if (response == null || response.Length == 0)
                throw new KeyNotFoundException(string.Format("The meta data for model key '{0}' was not found on the server, if retries fails, contact the server administrator.", modelKey));

            return JsonSerializer.FromJson<TypeMetaData>(response);
        }

        private static Uri GetDefaultMetaDataUri(ProtoRequest request)
        {
            var serviceUri = request.ServiceUri;

            var url = string.Format("{0}://{1}{2}/api/{3}",
                                    serviceUri.Scheme,
                                    serviceUri.Host,
                                    serviceUri.Port == 80
                                        ? string.Empty
                                        : ":" + serviceUri.Port,
                                        RestfulServiceConstants.ProtoMetaRouteRoot);

            return new Uri(url);
        }

        #endregion
    }
}
