using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    public sealed class TcpProtoBufBindingElement : ProtoBufBindingElement
    {
        protected override Type BindingElementType
        {
            get { return typeof(TcpProtoBufBinding); }
        }

        public TcpProtoBufBindingElement(string name)
            : base(name)
        { }

        public TcpProtoBufBindingElement()
            : base(null)
        { }

        protected override void OnApplyConfiguration(Binding binding)
        {
            var protoBinding = (TcpProtoBufBinding)binding;

            protoBinding.SetDefaultCompressionBehaviour(this.CompressionType);
            protoBinding.SetOperationBehaviours(OperationBehaviours);
            
            var tcpBindingElement = (TcpTransportBindingElement)protoBinding.GetBindingElement();

            tcpBindingElement.TransferMode = TransferMode.Streamed; //buffered mode requires a duplex session channel which is not supported currently.

            //httpBindingElement.HostNameComparisonMode = this.HostNameComparisonMode;
            //httpBindingElement.MaxBufferPoolSize = this.MaxBufferPoolSize;
            //httpBindingElement.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            //httpBindingElement.TransferMode = this.TransferMode;
            //httpBindingElement.UseDefaultWebProxy = this.UseDefaultWebProxy;
            //httpBindingElement.AllowCookies = this.AllowCookies;
            
            //httpBindingElement.MaxBufferSize = this.MaxBufferSize;
        }
    }
}