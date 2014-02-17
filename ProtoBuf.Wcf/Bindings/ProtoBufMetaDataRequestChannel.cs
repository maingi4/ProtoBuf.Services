using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public class ProtoBufMetaDataRequestChannel : ProtoBufMetaDataChannelBase, IRequestChannel
    {
        private readonly IRequestChannel _innerChannel;

        public ProtoBufMetaDataRequestChannel(ChannelManagerBase parent, IRequestChannel innerChannel):
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
            CheckAndMakeMetaDataRequest(message, timeout);

            return _innerChannel.Request(message, timeout);
        }

        public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginRequest(message, callback, state);
        }

        public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            CheckAndMakeMetaDataRequest(message, timeout);

            return _innerChannel.BeginRequest(message, timeout, callback, state);
        }

        public Message EndRequest(IAsyncResult result)
        {
            return _innerChannel.EndRequest(result);
        }
        
        #endregion

        #region Proteted Methods

        protected void CheckAndMakeMetaDataRequest(Message originalMessage, TimeSpan timeout)
        {
            //TODO: Check if the meta data request is being made here, this function will call innerchannel.request, which will again call this function itself.
            //TODO: check for existence of meta data here, before sending the request.
            //TODO: If request does not exist, send a custom request beforehand to download the meta data.
            //TODO: Save the meta data in a store. -- Abstract.
            //TODO: Upon recieving the meta data, continue with the original request.
            //TODO: Extend the protoBuf ProtoXmlSerializer, to consider meta data (from store), -- can we just use formatter?
        }

        #endregion
    }
}