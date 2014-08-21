using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ProtoBuf.ServiceModel;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufMessageFormatter : IDispatchMessageFormatter
    {
        private readonly Type[] _parameterTypes;

        public ProtoBufMessageFormatter(Type[] parameterTypes)
        {
            _parameterTypes = parameterTypes;
        }


        public void DeserializeRequest(Message message, object[] parameters)
        {
            var reader = message.GetReaderAtBodyContents();

            var provider = ObjectBuilder.GetModelProvider();

            for (int i = 0; i < parameters.Length; i++)
            {
                var serializer = new XmlProtoSerializer(provider.CreateModelInfo(_parameterTypes[i]).Model,
                                                        _parameterTypes[i]);

                var retVal = serializer.ReadObject(reader);

                parameters[i] = retVal;
            }
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            var provider = ObjectBuilder.GetModelProvider();

            var message = Message.CreateMessage(messageVersion, string.Empty, parameters[0],
                                                new XmlProtoSerializer(provider.CreateModelInfo(_parameterTypes[0]).Model,
                                                                       _parameterTypes[0]));

            return message;
        }
    }
}