using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ProtoBuf.Services.Wcf.Infrastructure;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class ProtoBufClientFormatter : ProtoBufMessageFormatterBase, IClientMessageFormatter
    {
        public ProtoBufClientFormatter(IList<TypeInfo> parameterTypes, string action,
            CompressionTypeOptions defaultCompressionType)
            : base(parameterTypes, action, defaultCompressionType)
        { }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return this.SerializeRequestInternal(messageVersion, parameters);
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            return this.DeserializeReplyInternal(message, parameters);
        }
    }
}