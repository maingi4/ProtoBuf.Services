using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public sealed class HttpProtoBufBinding : ProtoBufBinding
    {
        protected override TransportBindingElement GetTransport()
        {
            return new HttpTransportBindingElement();
        }
    }
}