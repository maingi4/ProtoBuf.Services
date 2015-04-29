using System.Configuration;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    [ConfigurationCollection(typeof(HttpProtoBufBindingElement), AddItemName = "binding")]
    public sealed class HttpProtoBufBindingElementCollection : ProtoBufBindingElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpProtoBufBindingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HttpProtoBufBindingElement) element).Name;
        }
    }
}