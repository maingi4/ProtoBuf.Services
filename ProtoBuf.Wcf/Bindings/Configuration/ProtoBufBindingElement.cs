using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    public class ProtoBufBindingElement : StandardBindingElement
    {
        #region Configuration Properties

        [ConfigurationProperty("operationBehaviours")]
        public OperationBehaviourElementCollection OperationBehaviours
        {
            get
            {
                return (OperationBehaviourElementCollection)this["operationBehaviours"];
            }
            set
            {
                this["operationBehaviours"] = value;
            }
        }

        [ConfigurationProperty("compressionType", DefaultValue = CompressionTypeOptions.None)]
        public CompressionTypeOptions CompressionType
        {
            get
            {
                return (CompressionTypeOptions)this["compressionType"];
            }
            set
            {
                this["compressionType"] = value;
            }
        }

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
                properties.Add(new ConfigurationProperty("compressionType", typeof(CompressionTypeOptions), CompressionTypeOptions.None));
                properties.Add(new ConfigurationProperty("operationBehaviours", typeof(OperationBehaviourElementCollection)));
                
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

            protoBinding.SetDefaultCompressionBehaviour(this.CompressionType);
            protoBinding.SetOperationBehaviours(OperationBehaviours);

            var httpBindingElement = protoBinding.GetHttpBindingElement();

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
