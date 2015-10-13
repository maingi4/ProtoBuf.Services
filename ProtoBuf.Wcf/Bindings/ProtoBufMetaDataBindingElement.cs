using System;
using System.ServiceModel.Channels;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class ProtoBufMetaDataBindingElement : TransportBindingElement
    {
        private readonly TransportBindingElement _innerTransportElement;

        public ProtoBufMetaDataBindingElement(TransportBindingElement innerTransportElement)
        {
            _innerTransportElement = innerTransportElement;
        }

        public ProtoBufMetaDataBindingElement(TransportBindingElement innerTransportElement, TransportBindingElement original)
            : base(original)
        {
            _innerTransportElement = innerTransportElement;
        }

        #region TransportBindingElement Members

        public override string Scheme
        {
            get { return _innerTransportElement.Scheme; }
        }

        public override BindingElement Clone()
        {
            return new ProtoBufMetaDataBindingElement(this._innerTransportElement, this);
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IRequestChannel);
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IReplyChannel);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {

            if (context == null)
                throw new ArgumentNullException("context");

            if (!CanBuildChannelFactory<TChannel>(context))
            {
                throw new ArgumentException(String.Format("Unsupported channel type: {0}.", typeof(TChannel).Name));
            }

            return (IChannelFactory<TChannel>)new MetaRequestChannelFactory((IChannelFactory<IRequestChannel>)_innerTransportElement.BuildChannelFactory<TChannel>(context));
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            
            if (!CanBuildChannelListener<TChannel>(context))
            {
                throw new ArgumentException(String.Format("Unsupported channel type: {0}.", typeof(TChannel).Name));
            }

            return (IChannelListener<TChannel>)new MetaReplyChannelListener(this, context, (IChannelListener<IReplyChannel>)_innerTransportElement.BuildChannelListener<TChannel>(context));
        }

        #endregion
    }
}