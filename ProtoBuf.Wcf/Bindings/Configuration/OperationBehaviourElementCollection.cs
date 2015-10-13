using System.Configuration;

namespace ProtoBuf.Services.Wcf.Bindings.Configuration
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
