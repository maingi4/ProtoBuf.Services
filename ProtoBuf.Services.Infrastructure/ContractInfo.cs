namespace ProtoBuf.Services.Wcf.Infrastructure
{
    public sealed class ContractInfo
    {
        public string ServiceContractName { get; set; }
        public string ServiceNamespace { get; set; }
        public string OperationContractName { get; set; }
        public string Action { get; set; }

        private ContractInfo(string action)
        {
            Action = action;

            ServiceContractName = action.Substring(0, action.LastIndexOf('/')).Trim('/');

            ServiceNamespace = ServiceContractName.Substring(0, ServiceContractName.LastIndexOf('/')).Trim('/');

            OperationContractName = action.Substring(action.LastIndexOf('/')).Trim('/');
        }

        public static ContractInfo FromAction(string action)
        {
            return new ContractInfo(action);
        }
    }
}
