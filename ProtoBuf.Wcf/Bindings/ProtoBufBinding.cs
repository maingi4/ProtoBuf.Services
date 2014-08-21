using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufBinding : Binding
    {
        private HttpTransportBindingElement _transport;
        private BinaryMessageEncodingBindingElement _encoding;
        private ProtoBufMetaDataBindingElement _metaDataComponent;

        public ProtoBufBinding()
            : base()
        {
            this.InitializeValue();
        }

        public override BindingElementCollection CreateBindingElements()
        {
            var elements = new BindingElementCollection
                {
                    this._encoding,
                    this._metaDataComponent
                };

            return elements;
        }

        public override string Scheme
        {
            get { return this._metaDataComponent.Scheme; }
        }

        private void InitializeValue()
        {
            this._encoding = new BinaryMessageEncodingBindingElement();
            this._metaDataComponent = new ProtoBufMetaDataBindingElement(new HttpTransportBindingElement());
        }
    }
}