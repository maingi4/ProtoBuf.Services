using System;
using System.Configuration;
using System.ServiceModel.Channels;
using ProtoBuf.Services.Wcf.Bindings.Configuration;

namespace ProtoBuf.Services.Wcf.Bindings
{
    /*in order to keep backward compatability with version 1.0, 
     * so that not everyone needs to change config files, 
     * the name prior to introduction of multiple transports still exists and defaults to http.
     */

    public sealed class ProtoBufBindingCollectionElement: ProtoBufBindingCollectionElementBase
    {
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public HttpProtoBufBindingElementCollection Bindings
        {
            get
            {
                return (HttpProtoBufBindingElementCollection)this[""];
            }
        }

        protected override ProtoBufBindingElementCollection GetBindings()
        {
            return this.Bindings;
        }

        public override Type BindingType
        {
            get { return typeof(HttpProtoBufBinding); }
        }

        protected override Binding GetDefault()
        {
            return new HttpProtoBufBinding();
        }
    }
}