using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    public abstract class ProtoBufBindingElement : StandardBindingElement
    {
        protected ProtoBufBindingElement(string name) : base(name)
        {
        }

        [ConfigurationProperty("operationBehaviours")]
        public OperationBehaviourElementCollection OperationBehaviours
        {
            get
            {
                return (OperationBehaviourElementCollection)this["operationBehaviours"];
            }
            set
            {
                this["operationBehaviours"] = value;
            }
        }

        [ConfigurationProperty("compressionType", DefaultValue = CompressionTypeOptions.None)]
        public CompressionTypeOptions CompressionType
        {
            get
            {
                return (CompressionTypeOptions)this["compressionType"];
            }
            set
            {
                this["compressionType"] = value;
            }
        }

        protected override abstract void OnApplyConfiguration(Binding binding);
    }
}
