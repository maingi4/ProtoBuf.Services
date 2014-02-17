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

        #region ChannelFactoryBase Members

        protected override IRequestChannel OnCreateChannel(EndpointAddress address, Uri via)
        {
            var innerChannel = _innerFactory.CreateChannel(address, via);

            return WrapChannel(innerChannel);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerFactory.BeginOpen(timeout, callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            _innerFactory.EndOpen(result);
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerFactory.Open();
        }

        #endregion

        #region Protected Methods

        protected IRequestChannel WrapChannel(IRequestChannel innerChannel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    //public abstract class ProtoBufMetaDataChannelBase : ChannelBase
    //{
    //    private readonly EndpointAddress _address;
    //    private readonly IReplyChannel _innerChannel;

    //    protected ProtoBufMetaDataChannelBase(EndpointAddress address, ChannelManagerBase parent,
    //        IReplyChannel innerChannel)
    //        : base(parent)
    //    {
    //        _address = address;
    //        _innerChannel = innerChannel;
    //    }

    //    public EndpointAddress RemoteAddress
    //    {
    //        get { return this._address; }
    //    }

    //    protected Message GetMetaDataFor(string typeName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void OnAbort()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void OnClose(TimeSpan timeout)
    //    {

    //    }

    //    protected override void OnEndClose(IAsyncResult result)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void OnEndOpen(IAsyncResult result)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void OnOpen(TimeSpan timeout)
    //    {

    //    }
    //}

    public class ProtoBufMetaDataReplyChannel : ChannelBase, IReplyChannel
    {
        private readonly EndpointAddress _localAddress;
        private readonly IReplyChannel _innerChannel;

        public ProtoBufMetaDataReplyChannel(EndpointAddress address, 
            ChannelManagerBase parent, IReplyChannel innerChannel):
            base(parent)
        {
            this._localAddress = address;
            _innerChannel = innerChannel;
        }

        #region IReplyChannel Members

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

        public RequestContext ReceiveRequest()
        {
            return _innerChannel.ReceiveRequest();
        }

        public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
        {
            return _innerChannel.TryReceiveRequest(timeout, out context);
        }

        public bool WaitForRequest(TimeSpan timeout)
        {
            return _innerChannel.WaitForRequest(timeout);
        }

        #endregion

        #region ChannelBase Members

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginClose(timeout, callback, state);
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerChannel.Open(timeout);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginOpen(timeout, callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            _innerChannel.EndOpen(result);
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _innerChannel.Close(timeout);
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            _innerChannel.EndClose(result);
        }

        protected override void OnAbort()
        {
            _innerChannel.Abort();
        }

        #endregion
    }
}
