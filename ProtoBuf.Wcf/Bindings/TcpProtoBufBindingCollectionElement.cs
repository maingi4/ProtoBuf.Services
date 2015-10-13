using System;
using System.Configuration;
using System.ServiceModel.Channels;
using ProtoBuf.Services.Wcf.Bindings.Configuration;

namespace ProtoBuf.Services.Wcf.Bindings
{
    public sealed class TcpProtoBufBindingCollectionElement : ProtoBufBindingCollectionElementBase
    {
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public TcpProtoBufBindingElementCollection Bindings
        {
            get
            {
                return (TcpProtoBufBindingElementCollection)this[""];
            }
        }

        protected override ProtoBufBindingElementCollection GetBindings()
        {
            return this.Bindings;
        }

        public override Type BindingType
        {
            get { return typeof(TcpProtoBufBinding); }
        }

        protected override Binding GetDefault()
        {
            return new TcpProtoBufBinding();
        }
    }
}