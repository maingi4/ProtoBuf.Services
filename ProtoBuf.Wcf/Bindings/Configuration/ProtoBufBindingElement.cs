using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    public class ProtoBufBindingElement : StandardBindingElement
    {
        #region Configuration Properties

        [ConfigurationProperty("maxBufferSize", DefaultValue = 65536)]
        public int MaxBufferSize
        {
            get
            {
                return (int)this["maxBufferSize"];
            }
            set
            {
                this["maxBufferSize"] = (object)value;
            }
        }

        [ConfigurationProperty("allowCookies", DefaultValue = false)]
        public bool AllowCookies
        {
            get
            {
                return (bool)this["allowCookies"];
            }
            set
            {
                this["allowCookies"] = value;
            }
        }

        [ConfigurationProperty("useDefaultWebProxy", DefaultValue = true)]
        public bool UseDefaultWebProxy
        {
            get
            {
                return (bool)this["useDefaultWebProxy"];
            }
            set
            {
                this["useDefaultWebProxy"] = value;
            }
        }

        [ConfigurationProperty("transferMode", DefaultValue = TransferMode.Buffered)]
        public TransferMode TransferMode
        {
            get
            {
                return (TransferMode)this["transferMode"];
            }
            set
            {
                this["transferMode"] = (object)value;
            }
        }

        [ConfigurationProperty("maxReceivedMessageSize", DefaultValue = 65536L)]
        public long MaxReceivedMessageSize
        {
            get
            {
                return (long)this["maxReceivedMessageSize"];
            }
            set
            {
                this["maxReceivedMessageSize"] = (object)value;
            }
        }

        [ConfigurationProperty("maxBufferPoolSize", DefaultValue = 524288L)]
        public long MaxBufferPoolSize
        {
            get
            {
                return (long)this["maxBufferPoolSize"];
            }
            set
            {
                this["maxBufferPoolSize"] = (object)value;
            }
        }

        [ConfigurationProperty("hostNameComparisonMode", DefaultValue = HostNameComparisonMode.StrongWildcard)]
        public HostNameComparisonMode HostNameComparisonMode
        {
            get
            {
                return (HostNameComparisonMode)this["hostNameComparisonMode"];
            }
            set
            {
                this["hostNameComparisonMode"] = (object)value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                var properties = base.Properties;

                properties.Add(new ConfigurationProperty("maxBufferSize", typeof(int), 65536));
                properties.Add(new ConfigurationProperty("allowCookies", typeof(bool), false));
                properties.Add(new ConfigurationProperty("useDefaultWebProxy", typeof(bool), true));
                properties.Add(new ConfigurationProperty("transferMode", typeof(TransferMode), TransferMode.Buffered));
                properties.Add(new ConfigurationProperty("maxBufferPoolSize", typeof(long), 524288L));
                properties.Add(new ConfigurationProperty("maxReceivedMessageSize", typeof(long), 65536L));
                properties.Add(new ConfigurationProperty("hostNameComparisonMode", typeof(HostNameComparisonMode), HostNameComparisonMode.StrongWildcard));
                
                return properties;
            }
        }

        #endregion

        #region StandardBindingElement Members

        protected override Type BindingElementType
        {
            get { return typeof(ProtoBufBinding); }
        }

        public ProtoBufBindingElement(string name)
            : base(name)
        { }

        public ProtoBufBindingElement()
            : base(null)
        { }

        protected override void OnApplyConfiguration(Binding binding)
        {
            var protoBinding = (ProtoBufBinding)binding;

            var httpBindingBase = protoBinding.GetHttpBindingElement();

            httpBindingBase.HostNameComparisonMode = this.HostNameComparisonMode;
            httpBindingBase.MaxBufferPoolSize = this.MaxBufferPoolSize;
            httpBindingBase.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            httpBindingBase.TransferMode = this.TransferMode;
            httpBindingBase.UseDefaultWebProxy = this.UseDefaultWebProxy;
            httpBindingBase.AllowCookies = this.AllowCookies;

            httpBindingBase.MaxBufferSize = this.MaxBufferSize;
        }

        #endregion
    }
}
