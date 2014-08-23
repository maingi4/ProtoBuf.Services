using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using ProtoBuf.Wcf.Channels.Bindings.Configuration;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufBindingCollectionElement
        : BindingCollectionElement
    {
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ProtoBufBindingElementCollection Bindings
        {
            get
            {
                return (ProtoBufBindingElementCollection)this[""];
            }
        }
        
        public override Type BindingType
        {
            get { return typeof(ProtoBufBinding); }
        }

        public override ReadOnlyCollection<IBindingConfigurationElement> ConfiguredBindings
        {
            get
            {
                var list = new List<IBindingConfigurationElement>();

                foreach (ProtoBufBindingElement configurationElement in this.Bindings)
                    list.Add(configurationElement);

                return new ReadOnlyCollection<IBindingConfigurationElement>(list);
            }
        }

        protected override Binding GetDefault()
        {
            return new ProtoBufBinding();
        }

        public override bool ContainsKey(string name)
        {
            return this.ConfiguredBindings.Any(x => x.Name.Equals(name, StringComparison.Ordinal));
        }

        protected override bool TryAdd(string name, Binding binding, System.Configuration.Configuration config)
        {
            throw new NotImplementedException();
        }
    }
}