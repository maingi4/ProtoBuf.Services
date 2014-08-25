using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class MetaReplyChannelListener : ChannelListenerBase<IReplyChannel>
    {
        private readonly IChannelListener<IReplyChannel> _innerListener;
        //private readonly BufferManager _bufferManager;
        ////private readonly MessageEncoderFactory _encoderFactory;
        //private readonly Uri _uri;

        public MetaReplyChannelListener(TransportBindingElement transportElement, BindingContext context, IChannelListener<IReplyChannel> innerListener)
            : base(context.Binding)
        {
            _innerListener = innerListener;
            //this.MaxReceivedMessageSize = transportElement.MaxReceivedMessageSize;
            //var messageElement = context.BindingParameters.Remove<MessageEncodingBindingElement>();
            //this._bufferManager = BufferManager.CreateBufferManager(transportElement.MaxBufferPoolSize, (int)this.MaxReceivedMessageSize);
            ////this._encoderFactory = messageElement.CreateMessageEncoderFactory();
            //this._uri = new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress);
        }

        #region ChannelListenerBase Members

        protected override IReplyChannel OnAcceptChannel(TimeSpan timeout)
        {
            var innerChannel = _innerListener.AcceptChannel();

            return this.WrapChannel(innerChannel);
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            return _innerListener.WaitForChannel(timeout);
        }

        public override Uri Uri
        {
            get { return this._innerListener.Uri; }
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerListener.Open(timeout);
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _innerListener.Close(timeout);
        }

        protected override void OnAbort()
        {
            _innerListener.Abort();
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerListener.BeginAcceptChannel(timeout, callback, state);
        }

        protected override IReplyChannel OnEndAcceptChannel(IAsyncResult result)
        {
            var innerChannel = _innerListener.EndAcceptChannel(result);

            return WrapChannel(innerChannel);
        }

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerListener.BeginWaitForChannel(timeout, callback, state);
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            return _innerListener.EndWaitForChannel(result);
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerListener.BeginClose(timeout, callback, state);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerListener.BeginOpen(timeout, callback, state);
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            _innerListener.EndClose(result);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            _innerListener.EndOpen(result);
        }

        #endregion

        #region Protected Methods

        protected IReplyChannel WrapChannel(IReplyChannel innerChannel)
        {
            if (innerChannel == null)
                return null;

            var address = new EndpointAddress(this.Uri);

            return new ProtoBufMetaDataReplyChannel(address, this, innerChannel);
        }

        #endregion
    }
}