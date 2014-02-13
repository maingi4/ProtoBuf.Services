using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace ProtoBuf.Wcf.Bindings
{
    public class ProtoBufBindingCollectionElement
        : BindingCollectionElement
    {
        // type of custom binding class
        public override Type BindingType
        {
            get { return typeof(ProtoBufBinding); }
        }

        // override ConfiguredBindings
        public override ReadOnlyCollection<IBindingConfigurationElement> ConfiguredBindings
        {
            get
            {
                return new ReadOnlyCollection<IBindingConfigurationElement>(
                new List<IBindingConfigurationElement>());
            }
        }

        // return Binding class object
        protected override Binding GetDefault()
        {
            return new ProtoBufBinding();
        }

        public override bool ContainsKey(string name)
        {
            return false;
        }

        protected override bool TryAdd(string name, Binding binding, System.Configuration.Configuration config)
        {
            throw new NotImplementedException();
        }
    }

    public class ProtoBufBinding : Binding
    {
        private HttpTransportBindingElement _transport;
        private BinaryMessageEncodingBindingElement _encoding;
        private MetaDataUploaderBindingElement _metaDataComponent;

        public ProtoBufBinding()
            : base()
        {
            this.InitializeValue();
        }

        public override BindingElementCollection CreateBindingElements()
        {
            var elements = new BindingElementCollection
                {
                    this._encoding,
                    this._metaDataComponent
                };

            return elements;
        }

        public override string Scheme
        {
            get { return this._metaDataComponent.Scheme; }
        }

        private void InitializeValue()
        {
            this._encoding = new BinaryMessageEncodingBindingElement();
            this._metaDataComponent = new MetaDataUploaderBindingElement();
        }
    }


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

            return (IChannelListener<TChannel>)new MetaReplyChannelListener();
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

        protected override IReplyChannel OnAcceptChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override IReplyChannel OnEndAcceptChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public override Uri Uri
        {
            get { throw new NotImplementedException(); }
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

        protected override void OnOpen(TimeSpan timeout)
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

        protected override void OnOpen(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }

    //public class ProtoBufMetaDataDownloaderChannel : ProtoBufMetaDataChannelBase
    //{

    //}

    //public class ProtoBufEncodingElement : MessageEncodingBindingElement
    //{
    //    public ProtoBufEncodingElement()
    //    {

    //    }

    //    private ProtoBufEncodingElement(ProtoBufEncodingElement clone)
    //        : base(clone)
    //    {

    //    }

    //    public override BindingElement Clone()
    //    {
    //        return new ProtoBufEncodingElement(this);
    //    }

    //    public override MessageEncoderFactory CreateMessageEncoderFactory()
    //    {
    //        return new ProtoBugMessageEncoderFactory();
    //    }

    //    public override MessageVersion MessageVersion { get; set; }
    //}

    //public class ProtoBugMessageEncoderFactory : MessageEncoderFactory
    //{
    //    #region MessageEncoderFactory Members

    //    private MessageEncoder _messageEncoder;

    //    public override MessageEncoder Encoder
    //    {
    //        get
    //        {
    //            return _messageEncoder ??
    //                    (_messageEncoder = new BinaryMessageEncodingBindingElement());
    //        }
    //    }

    //    public override MessageVersion MessageVersion
    //    {
    //        get { return MessageVersion.None; }
    //    }

    //    public string MediaType
    //    {
    //        get { return null; }
    //    }

    //    #endregion
    //}

    //public class ProtoBufMessageEncoder : MessageEncoder
    //{
    //    #region Fields

    //    private readonly ProtoBugMessageEncoderFactory _protoBugMessageEncoderFactory;

    //    #endregion

    //    #region Constructors

    //    public ProtoBufMessageEncoder(ProtoBugMessageEncoderFactory protoBugMessageEncoderFactory)
    //    {
    //        _protoBugMessageEncoderFactory = protoBugMessageEncoderFactory;
    //    }

    //    #endregion

    //    #region MessageEncoder Members

    //    public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
    //    {
    //        var data = new byte[maxSizeOfHeaders];
    //        stream.Write(data, 0, data.Length);

    //        return Message.CreateMessage()
    //    }

    //    public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager,
    //        string contentType)
    //    {
    //        var msgContents = new byte[buffer.Count];

    //        Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);

    //        bufferManager.ReturnBuffer(buffer.Array);

    //        var stream = new MemoryStream(msgContents);

    //        return ReadMessage(stream, int.MaxValue);
    //    }

    //    public override void WriteMessage(Message message, Stream stream)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string ContentType
    //    {
    //        get { return "binary"; }
    //    }

    //    public override string MediaType
    //    {
    //        get
    //        {
    //            return _protoBugMessageEncoderFactory.MediaType;
    //        }
    //    }

    //    public override MessageVersion MessageVersion
    //    {
    //        get { return _protoBugMessageEncoderFactory.MessageVersion; }
    //    }

    //    #endregion
    //}
}
