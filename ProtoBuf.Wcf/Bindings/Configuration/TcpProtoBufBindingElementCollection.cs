using System.Configuration;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    [ConfigurationCollection(typeof(TcpProtoBufBindingElement), AddItemName = "binding")]
    public sealed class TcpProtoBufBindingElementCollection : ProtoBufBindingElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TcpProtoBufBindingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TcpProtoBufBindingElement)element).Name;
        }
    }
}