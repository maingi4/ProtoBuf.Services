using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using ProtoBuf.Wcf.Channels.Infrastructure;
using ProtoBuf.Wcf.Channels.Serialization;

namespace ProtoBuf.Wcf.Channels.Bindings
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
            var action = originalMessage.Headers.Action;

            var contractInfo = ContractInfo.FromAction(action);

            var serviceContract = TypeFinder.FindServiceContract(contractInfo.ServiceContractName);

            var paramTypes = TypeFinder.GetContractParamTypes(serviceContract, contractInfo.OperationContractName, 
                contractInfo.Action);

            var store = ObjectBuilder.GetModelStore();

            var metaDataRequired = paramTypes.Any(paramType => store.GetModel(paramType.Type) == null);

            if (!metaDataRequired)
                return;

            var metadataRequestMessage = GetMetaDataRequestMessage(action);

            var metaDataReply = _innerChannel.Request(metadataRequestMessage);

            var headers = metaDataReply.Headers;

            var serializer = ObjectBuilder.GetSerializer();
            var modelProvider = ObjectBuilder.GetModelProvider();

            foreach (var messageHeader in headers)
            {
                if (messageHeader.Name.StartsWith("MetaData-"))
                {
                    var contractNamespace = messageHeader.Name.Replace("MetaData-", string.Empty);

                    var contractType = TypeFinder.FindDataContract(contractNamespace, contractInfo.ServiceContractName,
                                                                   contractInfo.Action);

                    var contractMetaDataString = headers.GetHeader<string>(messageHeader.Name, messageHeader.Namespace);

                    var contractMeta = BinaryConverter.FromString(contractMetaDataString);

                    var metaData = serializer.Deserialize<TypeMetaData>(contractMeta);

                    modelProvider.CreateModelInfo(contractType, metaData);
                }
            }
            //TODO: Extend the protoBuf ProtoXmlSerializer, to consider meta data (from store), -- can we just use formatter?
        }

        protected Message GetMetaDataRequestMessage(string action)
        {
            var message = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, action);

            message.Headers.Add(MessageHeader.CreateHeader("MetaData", "Maingi", "Y"));

            return message;
        }

        #endregion
    }
}