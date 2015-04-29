using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Xml;
using ProtoBuf.Wcf.Channels.Bindings.Configuration;
using ProtoBuf.Wcf.Channels.Exceptions;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public abstract class ProtoBufBinding : Binding
    {
        private TransportBindingElement _transport;
        private BinaryMessageEncodingBindingElement _encoding;
        private ProtoBufMetaDataBindingElement _mainTransport;
        private CompressionTypeOptions _compressionTypeOptions;
        private IDictionary<string, OperationBehaviourElement> _operationBehaviours;

        protected ProtoBufBinding()
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
            this._transport = GetTransport();
            this._mainTransport = new ProtoBufMetaDataBindingElement(this._transport);
        }

        public BinaryMessageEncodingBindingElement GetEncodingElement()
        {
            return this._encoding;
        }

        protected abstract TransportBindingElement GetTransport();

        public TransportBindingElement GetBindingElement()
        {
            return this._transport;
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