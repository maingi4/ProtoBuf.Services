using System.ServiceModel.Channels;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class TcpProtoBufBinding : ProtoBufBinding
    {
        protected override TransportBindingElement GetTransport()
        {
            return new TcpTransportBindingElement();
        }
    }
}