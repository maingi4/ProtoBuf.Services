using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using ProtoBuf.Wcf.Channels.Bindings.Configuration;
using ProtoBuf.Wcf.Channels.Exceptions;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public sealed class ProtoBufBinding : Binding
    {
        private HttpTransportBindingElement _httpTransport;
        private BinaryMessageEncodingBindingElement _encoding;
        private ProtoBufMetaDataBindingElement _mainTransport;
        private CompressionTypeOptions _compressionTypeOptions;
        private IDictionary<string, OperationBehaviourElement> _operationBehaviours;

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
            _compressionTypeOptions = CompressionTypeOptions.None;
            _operationBehaviours = new Dictionary<string, OperationBehaviourElement>();

            this._encoding = new BinaryMessageEncodingBindingElement();
            this._httpTransport = new HttpTransportBindingElement();
            this._mainTransport = new ProtoBufMetaDataBindingElement(this._httpTransport);
        }

        public HttpTransportBindingElement GetHttpBindingElement()
        {
            return this._httpTransport;
        }

        public void SetOperationBehaviours(OperationBehaviourElementCollection operationBehaviourElements)
        {
            foreach (OperationBehaviourElement operationBehaviourElement in operationBehaviourElements)
            {
                _operationBehaviours.Add(operationBehaviourElement.OperationName, operationBehaviourElement);
            }
        }

        public CompressionTypeOptions GetOperationCompressionBehaviour(string operationName)
        {
            OperationBehaviourElement operationBehaviour;
            if (_operationBehaviours.TryGetValue(operationName, out operationBehaviour))
            {
                return operationBehaviour.CompressionType.HasValue ?
                    operationBehaviour.CompressionType.Value : GetDefaultCompressionBehaviour();
            }

            return GetDefaultCompressionBehaviour();
        }

        public void SetDefaultCompressionBehaviour(CompressionTypeOptions compressionTypeOptions)
        {
            _compressionTypeOptions = compressionTypeOptions;
        }

        public CompressionTypeOptions GetDefaultCompressionBehaviour()
        {
            return _compressionTypeOptions;
        }
    }
}