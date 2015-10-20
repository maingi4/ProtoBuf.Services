using System;
using System.Collections.Generic;

namespace ProtoBuf.Services.WebAPI.Client
{
    public class ProtoRequest
    {
        /// <summary>
        /// Mandatory uri of the service to be called
        /// </summary>
        public Uri ServiceUri { get; set; }
        /// <summary>
        /// If null, request will not be included in the body, this is ignored in "GET" requests.
        /// </summary>
        public object Request { get; set; }
        /// <summary>
        /// Mandatory request method
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Optional timeout of the web request, defaults to 1 minute.
        /// </summary>
        public TimeSpan Timeout { get; set; }
        /// <summary>
        /// Optional request headers to add, if any additional headers are needed to be included in the outgoing request.
        /// </summary>
        public IDictionary<string, string> RequestHeaders { get; set; }
        /// <summary>
        /// Optional meta data uri of the target service, if this needs to be provided it will be provided by the service owner, if nothing is specified it is assumed to be default behaviour.
        /// </summary>
        public Uri ServiceMetaDataUri { get; set; }
        
        /// <summary>
        /// </summary>
        /// <param name="serviceUri">uri of the service to be called</param>
        /// <param name="request">If null, request will not be included in the body, this is ignored in "GET" requests.</param>
        /// <param name="method">Mandatory request method, e.g. GET, PUT, POST, DELETE etc</param>
        public ProtoRequest(Uri serviceUri, object request, string method)
        {
            if (serviceUri == null)
                throw new ArgumentNullException("serviceUri", "The serviceUri cannot be null");

            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("method", "The method cannot be null, make sure its a valid http method e.g. GET or POST etc.");

            ServiceUri = serviceUri;
            Request = request;
            Method = method;
            Timeout = TimeSpan.FromMinutes(1d);
        }
    }
}