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

        public MetaDataUploaderBindingElement()
        { }

        public MetaDataUploaderBindingElement(MetaDataUploaderBindingElement original)
            : base(original)
        { }

        public override string Scheme
        {
            get { return "protoBuf.Maingi"; }
        }

        public override BindingElement Clone()
        {
            return new MetaDataUploaderBindingElement(this);
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

            return (IChannelFactory<TChannel>)new MetaRequestChannelFactory();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (!CanBuildChannelListener<TChannel>(context))
            {
                throw new ArgumentException(String.Format("Unsupported channel type: {0}.", typeof(TChannel).Name));
            }

            return (IChannelListener<TChannel>)new MetaReplyChannelListener(this, context);
        }
    }

    public class MetaRequestChannelFactory : ChannelFactoryBase<IRequestChannel>
    {
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
        private readonly BufferManager _bufferManager;
        private readonly MessageEncoderFactory _encoderFactory;
        private readonly Uri _uri;

        protected long MaxReceivedMessageSize { get; set; }

        public MetaReplyChannelListener(TransportBindingElement transportElement, BindingContext context)
            : base(context.Binding)
        {
            this.MaxReceivedMessageSize = transportElement.MaxReceivedMessageSize;
            var messageElement = context.BindingParameters.Remove<MessageEncodingBindingElement>();
            this._bufferManager = BufferManager.CreateBufferManager(transportElement.MaxBufferPoolSize, (int)this.MaxReceivedMessageSize);
            this._encoderFactory = messageElement.CreateMessageEncoderFactory();
            this._uri = new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress);
        }

        protected override IReplyChannel OnAcceptChannel(TimeSpan timeout)
        {
            var address = new EndpointAddress(this.Uri);

            return new ProtoBufMetaDataReplyChannel(this._bufferManager, this._encoderFactory, address, this);
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
            //TODO: do something with the timeout at least.
        }

        protected override void OnClose(TimeSpan timeout)
        {

        }

        protected override void OnAbort()
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            Func<IReplyChannel> accepting = () => this.AcceptChannel(timeout);

            return accepting.BeginInvoke(callback, state);
        }

        protected override IReplyChannel OnEndAcceptChannel(IAsyncResult result)
        {
            return ((Func<IReplyChannel>) ((AsyncResult) result).AsyncDelegate).EndInvoke(result);
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
        private readonly MessageEncoderFactory _encoderFactory;
        private readonly EndpointAddress _address;

        protected ProtoBufMetaDataChannelBase(BufferManager bufferManager, MessageEncoderFactory encoderFactory,
            EndpointAddress address, ChannelManagerBase parent)
            : base(parent)
        {
            _bufferManager = bufferManager;
            _encoderFactory = encoderFactory;
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
        private EndpointAddress _localAddress;

        public ProtoBufMetaDataReplyChannel(BufferManager bufferManager, MessageEncoderFactory encoderFactory,
            EndpointAddress address, ChannelManagerBase parent):base(bufferManager, encoderFactory, address, parent)
        {
            this._localAddress = address;    
        }

        public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public RequestContext EndReceiveRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
        {
            throw new NotImplementedException();
        }

        public bool EndWaitForRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public EndpointAddress LocalAddress
        {
            get { return this._localAddress; }
        }

        public RequestContext ReceiveRequest(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public RequestContext ReceiveRequest()
        {
            return ReceiveRequest(DefaultReceiveTimeout);
        }

        public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
        {
            throw new NotImplementedException();
        }

        public bool WaitForRequest(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}
