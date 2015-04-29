using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    [ConfigurationCollection(typeof(HttpProtoBufBindingElement), AddItemName = "binding")]
    public abstract class ProtoBufBindingElementCollection : ConfigurationElementCollection
    {
        protected override abstract ConfigurationElement CreateNewElement();
        protected override abstract object GetElementKey(ConfigurationElement element);
    }
}
