using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace ProtoBuf.Wcf.Channels.Bindings
{
    public class ProtoBufBindingCollectionElement
        : BindingCollectionElement
    {
        // type of custom binding class
        public override Type BindingType
        {
            get { return typeof(ProtoBufBinding); }
        }

        // override ConfiguredBindings
        public override ReadOnlyCollection<IBindingConfigurationElement> ConfiguredBindings
        {
            get
            {
                return new ReadOnlyCollection<IBindingConfigurationElement>(
                    new List<IBindingConfigurationElement>());
            }
        }

        // return Binding class object
        protected override Binding GetDefault()
        {
            return new ProtoBufBinding();
        }

        public override bool ContainsKey(string name)
        {
            return true;
        }

        protected override bool TryAdd(string name, Binding binding, System.Configuration.Configuration config)
        {
            throw new NotImplementedException();
        }
    }
}