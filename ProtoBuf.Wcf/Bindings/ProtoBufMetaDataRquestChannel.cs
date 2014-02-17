using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public class ProtoBufMetaDataRquestChannel : ProtoBufMetaDataChannelBase, IRequestChannel
    {
        private readonly IRequestChannel _innerChannel;

        public ProtoBufMetaDataRquestChannel(ChannelManagerBase parent, IRequestChannel innerChannel):
            base(parent, innerChannel)
        {
            _innerChannel = innerChannel;
        }

        #region IRequestChannel Members

        public EndpointAddress RemoteAddress
        {
            get
            {
                return _innerChannel.RemoteAddress;
            }
        }

        public Uri Via { 
            get
            {
                return _innerChannel.Via;
            } 
        }

        public Message Request(Message message)
        {
            return _innerChannel.Request(message);
        }

        public Message Request(Message message, TimeSpan timeout)
        {
            return _innerChannel.Request(message, timeout);
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginRequest(message, callback, state);
        }

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginRequest(message, timeout, callback, state);
        }

        public Message EndRequest(IAsyncResult result)
        {
            return _innerChannel.EndRequest(result);
        }
        
        #endregion
    }
}