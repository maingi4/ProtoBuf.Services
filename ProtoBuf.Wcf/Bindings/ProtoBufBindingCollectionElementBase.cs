using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using ProtoBuf.Services.Wcf.Bindings.Configuration;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public abstract class ProtoBufBindingCollectionElementBase
        : BindingCollectionElement
    {
        public abstract override Type BindingType { get; }

        protected abstract ProtoBufBindingElementCollection GetBindings();

        public override ReadOnlyCollection<IBindingConfigurationElement> ConfiguredBindings
        {
            get
            {
                var list = new List<IBindingConfigurationElement>();

                foreach (ProtoBufBindingElement configurationElement in this.GetBindings())
                    list.Add(configurationElement);

                return new ReadOnlyCollection<IBindingConfigurationElement>(list);
            }
        }

        protected abstract override Binding GetDefault();

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