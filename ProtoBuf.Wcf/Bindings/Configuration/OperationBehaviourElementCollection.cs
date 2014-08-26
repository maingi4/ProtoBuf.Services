using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    [ConfigurationCollection(typeof(OperationBehaviourElement))]
    public sealed class OperationBehaviourElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OperationBehaviourElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OperationBehaviourElement)element).OperationName;
        }
    }
}
