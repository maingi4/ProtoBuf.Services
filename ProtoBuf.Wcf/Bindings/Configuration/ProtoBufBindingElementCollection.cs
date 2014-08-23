using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    [ConfigurationCollection(typeof(ProtoBufBindingElement), AddItemName = "binding")]
    public sealed class ProtoBufBindingElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProtoBufBindingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProtoBufBindingElement) element).Name;
        }
    }
}
