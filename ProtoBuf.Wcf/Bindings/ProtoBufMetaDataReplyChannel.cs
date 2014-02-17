using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public class ProtoBufMetaDataReplyChannel : ProtoBufMetaDataChannelBase, IReplyChannel
    {
        private readonly EndpointAddress _localAddress;
        private readonly IReplyChannel _innerChannel;

        public ProtoBufMetaDataReplyChannel(EndpointAddress address, 
                                            ChannelManagerBase parent, IReplyChannel innerChannel):
                                                base(parent, innerChannel)
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
            CheckAndReplyMetaDataRequest(timeout);

            return _innerChannel.ReceiveRequest(timeout);
        }

        public RequestContext ReceiveRequest()
        {
            CheckAndReplyMetaDataRequest(TimeSpan.FromMinutes(1)); //TODO: extract to configuration.

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

        #region PRotected Members

        protected void CheckAndReplyMetaDataRequest(TimeSpan timeout)
        {
            //TODO: check if its a meta data request and reply.
        }

        #endregion
    }
}