using System.ServiceModel.Channels;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class HttpProtoBufBinding : ProtoBufBinding
    {
        protected override TransportBindingElement GetTransport()
        {
            return new HttpTransportBindingElement();
        }
    }
}