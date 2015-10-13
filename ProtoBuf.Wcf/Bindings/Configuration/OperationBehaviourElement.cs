using System.Configuration;
using ProtoBuf.Services.Wcf.Infrastructure;

namespace ProtoBuf.Services.Wcf.Bindings.Configuration
{
    public sealed class OperationBehaviourElement : ConfigurationElement
    {
        [ConfigurationProperty("operationName", IsKey = true, IsRequired = true)]
        public string OperationName
        {
            get
            {
                return (string)this["operationName"];
            }
            set
            {
                this["operationName"] = value;
            }
        }

        [ConfigurationProperty("compressionType", DefaultValue = null)]
        public CompressionTypeOptions? CompressionType
        {
            get
            {
                return (CompressionTypeOptions?)this["compressionType"];
            }
            set
            {
                this["compressionType"] = value;
            }
        }
    }
}
