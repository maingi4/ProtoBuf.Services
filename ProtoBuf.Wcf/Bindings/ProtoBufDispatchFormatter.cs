using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ProtoBuf.Services.Wcf.Infrastructure;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class ProtoBufDispatchFormatter : ProtoBufMessageFormatterBase, IDispatchMessageFormatter
    {
        public ProtoBufDispatchFormatter(IList<TypeInfo> parameterTypes, string action,
            CompressionTypeOptions defaultCompressionType) 
            : base(parameterTypes, action, defaultCompressionType)
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