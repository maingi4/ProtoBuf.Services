using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufClientFormatter : ProtoBufMessageFormatterBase, IClientMessageFormatter
    {
        public ProtoBufClientFormatter(IList<TypeInfo> parameterTypes, string action) : base(parameterTypes, action)
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