using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace ProtoBuf.Wcf.Bindings
{
    public class MetaDataDownloaderBindingElement : BindingElement
    {
        public override BindingElement Clone()
        {
            throw new NotImplementedException();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDataUploaderBindingElement : TransportBindingElement
    {
        private readonly TransportBindingElement _innerTransportElement;

        public MetaDataUploaderBindingElement(TransportBindingElement innerTransportElement)
        {
            _innerTransportElement = innerTransportElement;
        }

        public MetaDataUploaderBindingElement(TransportBindingElement innerTransportElement, TransportBindingElement original)
            : base(original)
        {
            _innerTransportElement = innerTransportElement;
        }

        public override string Scheme
        {
            get { return "protoBuf.Maingi"; }
        }

        public override BindingElement Clone()
        {
            return new MetaDataUploaderBindingElement(this._innerTransportElement, this);
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
    }

    public class MetaRequestChannelFactory : ChannelFactoryBase<IRequestChannel>
    {
        private readonly IChannelFactory<IRequestChannel> _innerFactory;

        public MetaRequestChannelFactory(IChannelFactory<IRequestChannel> innerFactory)
        {
            _innerFactory = innerFactory;
        }

        protected override IRequestChannel OnCreateChannel(System.ServiceModel.EndpointAddress address, Uri via)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaReplyChannelListener : ChannelListenerBase<IReplyChannel>
    {
        private readonly IChannelListener<IReplyChannel> _innerChannel;
        private readonly BufferManager _bufferManager;
        //private readonly MessageEncoderFactory _encoderFactory;
        private readonly Uri _uri;

        protected long MaxReceivedMessageSize { get; set; }

        public MetaReplyChannelListener(TransportBindingElement transportElement, BindingContext context, IChannelListener<IReplyChannel> innerChannel)
            : base(context.Binding)
        {
            _innerChannel = innerChannel;
            this.MaxReceivedMessageSize = transportElement.MaxReceivedMessageSize;
            //var messageElement = context.BindingParameters.Remove<MessageEncodingBindingElement>();
            this._bufferManager = BufferManager.CreateBufferManager(transportElement.MaxBufferPoolSize, (int)this.MaxReceivedMessageSize);
            //this._encoderFactory = messageElement.CreateMessageEncoderFactory();
            this._uri = new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress);
        }

        protected override IReplyChannel OnAcceptChannel(TimeSpan timeout)
        {
            var address = new EndpointAddress(this.Uri);

            return new ProtoBufMetaDataReplyChannel(this._bufferManager, address, this, _innerChannel.AcceptChannel());
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public override Uri Uri
        {
            get { return this._uri; }
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerChannel.Open(timeout);
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _innerChannel.Close(timeout);
        }

        protected override void OnAbort()
        {
            _innerChannel.Abort();
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Func<IReplyChannel> accepting = () => this.AcceptChannel(timeout);

            return accepting.BeginInvoke(callback, state);
        }

        protected override IReplyChannel OnEndAcceptChannel(IAsyncResult result)
        {
            return ((Func<IReplyChannel>)((AsyncResult)result).AsyncDelegate).EndInvoke(result);
        }

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ProtoBufMetaDataChannelBase : ChannelBase
    {
        private readonly BufferManager _bufferManager;
        //private readonly MessageEncoderFactory _encoderFactory;
        private readonly EndpointAddress _address;

        protected ProtoBufMetaDataChannelBase(BufferManager bufferManager ,EndpointAddress address, 
            ChannelManagerBase parent)
            : base(parent)
        {
            _bufferManager = bufferManager;
            //_encoderFactory = encoderFactory;
            _address = address;
        }

        public EndpointAddress RemoteAddress
        {
            get { return this._address; }
        }

        protected Message GetMetaDataFor(string typeName)
        {
            throw new NotImplementedException();
        }

        protected override void OnAbort()
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override void OnClose(TimeSpan timeout)
        {

        }

        protected override void OnEndClose(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override void OnOpen(TimeSpan timeout)
        {

        }
    }

    public class ProtoBufMetaDataReplyChannel : ProtoBufMetaDataChannelBase, IReplyChannel
    {
        private readonly EndpointAddress _localAddress;
        private readonly IReplyChannel _innerChannel;

        public ProtoBufMetaDataReplyChannel(BufferManager bufferManager,
            EndpointAddress address, ChannelManagerBase parent, IReplyChannel innerChannel)
            : base(bufferManager, address, parent)
        {
            this._localAddress = address;
            _innerChannel = innerChannel;
        }

        public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginReceiveRequest(timeout, callback, state);
        }

        public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
        {
            return _innerChannel.BeginReceiveRequest(callback, state);
        }

        public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginTryReceiveRequest(timeout, callback, state);
        }

        public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginWaitForRequest(timeout, callback, state);
        }

        public RequestContext EndReceiveRequest(IAsyncResult result)
        {
            return _innerChannel.EndReceiveRequest(result);
        }

        public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
        {
            return _innerChannel.EndTryReceiveRequest(result, out context);
        }

        public bool EndWaitForRequest(IAsyncResult result)
        {
            return _innerChannel.EndWaitForRequest(result);
        }

        public EndpointAddress LocalAddress
        {
            get { return this._localAddress; }
        }

        public RequestContext ReceiveRequest(TimeSpan timeout)
        {
            return _innerChannel.ReceiveRequest(timeout);
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerChannel.Open(timeout);
            base.OnOpen(timeout);
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _innerChannel.Close(timeout);
            base.OnClose(timeout);
        }

        protected override void OnAbort()
        {
            _innerChannel.Abort();
            base.OnAbort();
        }

        public RequestContext ReceiveRequest()
        {
            return ReceiveRequest(DefaultReceiveTimeout);
        }

        public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
        {
            return _innerChannel.TryReceiveRequest(timeout, out context);
        }

        public bool WaitForRequest(TimeSpan timeout)
        {
            return _innerChannel.WaitForRequest(timeout);
        }
    }
}
