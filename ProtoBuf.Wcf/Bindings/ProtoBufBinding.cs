using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using ProtoBuf.Wcf.Channels.Exceptions;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufBinding : Binding
    {
        private HttpTransportBindingElement _httpTransport;
        private BinaryMessageEncodingBindingElement _encoding;
        private ProtoBufMetaDataBindingElement _mainTransport;

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
                    this._mainTransport
                };

            return elements;
        }

        public override string Scheme
        {
            get { return this._mainTransport.Scheme; }
        }

        private void InitializeValue()
        {
            this._encoding = new BinaryMessageEncodingBindingElement();
            this._httpTransport = new HttpTransportBindingElement();
            this._mainTransport = new ProtoBufMetaDataBindingElement(this._httpTransport);
        }

        public HttpTransportBindingElement GetHttpBindingElement()
        {
            return this._httpTransport;
        }
    }
}