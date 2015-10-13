using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Services.Wcf.Bindings.Configuration
{
    public sealed class HttpProtoBufBindingElement : ProtoBufBindingElement
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
                this["maxBufferSize"] = value;
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
                this["transferMode"] = value;
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
                this["maxReceivedMessageSize"] = value;
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
                this["maxBufferPoolSize"] = value;
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
                this["hostNameComparisonMode"] = value;
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
            get { return typeof(HttpProtoBufBinding); }
        }

        public HttpProtoBufBindingElement(string name)
            : base(name)
        { }

        public HttpProtoBufBindingElement()
            : base(null)
        { }

        protected override void OnApplyConfiguration(Binding binding)
        {
            var protoBinding = (HttpProtoBufBinding)binding;

            ApplyBaseConfiguration(protoBinding);

            var httpBindingElement = (HttpTransportBindingElement)protoBinding.GetBindingElement();

            httpBindingElement.HostNameComparisonMode = this.HostNameComparisonMode;
            httpBindingElement.MaxBufferPoolSize = this.MaxBufferPoolSize;
            httpBindingElement.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            httpBindingElement.TransferMode = this.TransferMode;
            httpBindingElement.UseDefaultWebProxy = this.UseDefaultWebProxy;
            httpBindingElement.AllowCookies = this.AllowCookies;
            
            httpBindingElement.MaxBufferSize = this.MaxBufferSize;
        }

        #endregion
    }
}