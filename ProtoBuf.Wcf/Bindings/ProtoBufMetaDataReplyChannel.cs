using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

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
            var context = _innerChannel.ReceiveRequest(timeout);

            var isMetaDataRequest = CheckAndReplyMetaDataRequest(context, timeout);

            return isMetaDataRequest ? null : context;
        }

        public RequestContext ReceiveRequest()
        {
            var timeout = ((IDefaultCommunicationTimeouts)this.Manager).ReceiveTimeout;
            
            var context = _innerChannel.ReceiveRequest();

            var isMetaDataRequest = CheckAndReplyMetaDataRequest(context, timeout);

            return isMetaDataRequest ? null : context;
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

        #region Protected Members

        protected bool CheckAndReplyMetaDataRequest(RequestContext context, TimeSpan timeout)
        {
            if (context == null || context.RequestMessage == null)
                return false;

            var buffer = context.RequestMessage.CreateBufferedCopy(int.MaxValue);

            var clonedMessage = buffer.CreateMessage();

            var reader = clonedMessage.GetReaderAtBodyContents();

            var isMetaDataRequest = IsMetaDataRequest(reader);

            if (isMetaDataRequest)
            {
                //TODO: send meta data reply here.
                return true;
            }

            return false;
        }

        protected bool IsMetaDataRequest(XmlReader reader)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}