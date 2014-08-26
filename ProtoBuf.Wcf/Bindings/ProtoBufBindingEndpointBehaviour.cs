using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Wcf.Channels.Exceptions;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufBindingEndpointBehaviour : BehaviorExtensionElement, IEndpointBehavior
    {
        public void Validate(ServiceEndpoint endpoint)
        { }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var protoBinding = endpoint.Binding as ProtoBufBinding;

            if (protoBinding == null)
                throw new ConfigurationException("The endpoint behaviour, ProtoBufBindingEndpointBehaviour, can only be applied to an endpoint which has ProtoBufBinding as its binding.");

            foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                var compressionBehaviour = protoBinding.GetOperationCompressionBehaviour(operation.Name);

                var contractInfo = ContractInfo.FromAction(operation.Action);

                var serviceContract = TypeFinder.FindServiceContract(contractInfo.ServiceContractName);

                var paramTypes = TypeFinder.GetContractParamTypes(serviceContract, contractInfo.OperationContractName,
                                                                  contractInfo.Action);

                var formatter = new ProtoBufDispatchFormatter(new List<TypeInfo>(paramTypes), contractInfo.Action, 
                    compressionBehaviour);

                operation.Formatter = formatter;
                operation.DeserializeRequest = true;
                operation.SerializeReply = true;
            }
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var protoBinding = endpoint.Binding as ProtoBufBinding;

            if (protoBinding == null)
                throw new ConfigurationException("The endpoint behaviour, ProtoBufBindingEndpointBehaviour, can only be applied to an endpoint which has ProtoBufBinding as its binding.");

            foreach (var clientOperation in clientRuntime.ClientOperations)
            {
                var compressionBehaviour = protoBinding.GetOperationCompressionBehaviour(clientOperation.Name);

                var contractInfo = ContractInfo.FromAction(clientOperation.Action);

                var serviceContract = TypeFinder.FindServiceContract(contractInfo.ServiceContractName);

                var paramTypes = TypeFinder.GetContractParamTypes(serviceContract, contractInfo.OperationContractName,
                                                                  contractInfo.Action);

                var formatter = new ProtoBufClientFormatter(new List<TypeInfo>(paramTypes), contractInfo.Action, 
                    compressionBehaviour);

                clientOperation.Formatter = formatter;
                clientOperation.SerializeRequest = true;
                clientOperation.DeserializeReply = true;
            }
        }

        protected override object CreateBehavior()
        {
            return new ProtoBufBindingEndpointBehaviour();
        }

        public override Type BehaviorType
        {
            get { return typeof(ProtoBufBindingEndpointBehaviour); }
        }
    }
}