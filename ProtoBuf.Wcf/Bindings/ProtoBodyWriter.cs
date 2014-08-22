using System;
using System.ServiceModel.Channels;
using System.Xml;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public sealed class ProtoBodyWriter : BodyWriter
    {
        private readonly string _operationName;
        private readonly string _serviceNamespace;
        private readonly Func<string[]> _valueGetter;

        public ProtoBodyWriter(string operationName, string serviceNamespace, Func<string[]> valueGetter)
            : base(false)
        {
            _operationName = operationName;
            _serviceNamespace = serviceNamespace;
            _valueGetter = valueGetter;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement(_operationName, _serviceNamespace);

            var values = _valueGetter();

            foreach (var value in values)
            {
                writer.WriteElementString("value", value);
            }

            writer.WriteEndElement();
        }
    }
}