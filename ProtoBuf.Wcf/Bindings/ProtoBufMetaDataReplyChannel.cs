using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Serialization;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class ProtoBufMetaDataReplyChannel : ProtoBufMetaDataChannelBase, IReplyChannel
    {
        private readonly EndpointAddress _localAddress;
        private readonly IReplyChannel _innerChannel;

        public ProtoBufMetaDataReplyChannel(EndpointAddress address,
                                            ChannelManagerBase parent, IReplyChannel innerChannel) :
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
            //return new ChainedAsyncResult(_innerChannel, timeout, callback, state);
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
            _innerChannel.EndTryReceiveRequest(result, out context);

            var timeout = ((IDefaultCommunicationTimeouts)this.Manager).ReceiveTimeout;

            context = ProcessContext(context, timeout);

            return context != null;
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

            return ProcessContext(context, timeout);
        }

        public RequestContext ReceiveRequest()
        {
            var timeout = ((IDefaultCommunicationTimeouts)this.Manager).ReceiveTimeout;

            var context = _innerChannel.ReceiveRequest();

            return ProcessContext(context, timeout);
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

        #region Private Members

        private RequestContext ProcessContext(RequestContext context, TimeSpan timeout)
        {
            var isMetaDataRequest = CheckAndReplyMetaDataRequest(context, timeout);

            return isMetaDataRequest ? null : context;
        }

        private bool CheckAndReplyMetaDataRequest(RequestContext context, TimeSpan timeout)
        {
            if (context == null || context.RequestMessage == null)
                return false;

            var isMetaDataRequest = IsMetaDataRequest(context.RequestMessage);

            if (isMetaDataRequest)
            {
                ReplyWithMetaData(context);

                return true;
            }

            return false;
        }

        private bool IsMetaDataRequest(Message message)
        {
            var headerLocation = message.Headers.FindHeader(Constants.MetaDataHeaderKey, Constants.DefaultCustomHeaderNamespace);

            if (headerLocation > -1)
                return message.Headers.GetHeader<string>(headerLocation) == "Y";

            return false;
        }

        private void ReplyWithMetaData(RequestContext context)
        {
            var action = context.RequestMessage.Headers.Action;

            var contractInfo = ContractInfo.FromAction(action);

            var contractType = TypeFinder.FindServiceContract(contractInfo.ServiceContractName);

            var paramTypes = TypeFinder.GetContractParamTypes(contractType, contractInfo.OperationContractName, contractInfo.Action);

            var modelProvider = ObjectBuilder.GetModelProvider();

            var typeMetaDatas = new Dictionary<string, string>();
            var serializer = ObjectBuilder.GetSerializer();

            foreach (var paramType in paramTypes)
            {
                if (typeMetaDatas.ContainsKey(paramType.Name))
                    continue;

                var modelInfo = modelProvider.CreateModelInfo(paramType.Type, ModeType.Wcf);

                var metaData = modelInfo.MetaData;

                var result = serializer.Serialize(metaData, ModeType.Wcf);

                var val = BinaryConverter.ToString(result.Data);

                typeMetaDatas.Add(paramType.Name, val);
            }

            var replyMessage = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, context.RequestMessage.Headers.Action);

            foreach (var typeMetaData in typeMetaDatas)
            {
                replyMessage.Headers.Add(MessageHeader.CreateHeader(Constants.MetaDataHeaderKeySuffix + typeMetaData.Key, 
                    Constants.DefaultCustomHeaderNamespace, typeMetaData.Value));
            }

            context.Reply(replyMessage);

            context.Close();
        }

        #endregion
    }
}