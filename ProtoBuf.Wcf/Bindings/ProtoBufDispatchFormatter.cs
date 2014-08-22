using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufDispatchFormatter : ProtoBufMessageFormatterBase, IDispatchMessageFormatter
    {
        public ProtoBufDispatchFormatter(IList<TypeInfo> parameterTypes, string action) 
            : base(parameterTypes, action)
        { }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            this.DeserializeRequestInternal(message, parameters);
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            return this.SerializeReplyInternal(messageVersion, parameters, result);
        }
    }
}