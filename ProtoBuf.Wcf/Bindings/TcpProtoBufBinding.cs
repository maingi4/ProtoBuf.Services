using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public sealed class TcpProtoBufBinding : ProtoBufBinding
    {
        protected override TransportBindingElement GetTransport()
        {
            return new TcpTransportBindingElement();
        }
    }
}