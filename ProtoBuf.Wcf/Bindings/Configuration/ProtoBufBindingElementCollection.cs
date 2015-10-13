using System.Configuration;

namespace ProtoBuf.Services.Wcf.Bindings.Configuration
{
    public abstract class ProtoBufBindingElementCollection : ConfigurationElementCollection
    {
        protected override abstract ConfigurationElement CreateNewElement();
        protected override abstract object GetElementKey(ConfigurationElement element);
    }
}
