using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
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
