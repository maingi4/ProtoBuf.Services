using System;
using System.Net;

namespace ProtoBuf.Services.WebAPI.Client
{
    internal sealed class ExtendedWebClient : WebClient
    {
        private TimeSpan _timeout;

        public ExtendedWebClient(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var rq = base.GetWebRequest(address);

            if (rq != null)
            {
                rq.Timeout = Convert.ToInt32(_timeout.TotalMilliseconds);
            }

            return rq;
        }
    }
}