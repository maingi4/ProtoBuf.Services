using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Channels.Bindings.Configuration
{
    public sealed class TcpProtoBufBindingElement : ProtoBufBindingElement
    {
        #region ProtoBufBindingElement Members

        protected override Type BindingElementType
        {
            get { return typeof(TcpProtoBufBinding); }
        }

        public TcpProtoBufBindingElement(string name)
            : base(name)
        { }

        public TcpProtoBufBindingElement()
            : base(null)
        { }

        #endregion

        #region Configurations

        [ConfigurationProperty("listenBacklog", DefaultValue = 10)]
        public int ListenBacklog
        {
            get
            {
                return (int)this["listenBacklog"];
            }
            set
            {
                this["listenBacklog"] = value;
            }
        }

        [ConfigurationProperty("maxConnections", DefaultValue = 10)]
        public int MaxConnections
        {
            get
            {
                return (int)this["maxConnections"];
            }
            set
            {
                this["maxConnections"] = value;
            }
        }

        [ConfigurationProperty("portSharingEnabled", DefaultValue = true)]
        public bool PortSharingEnabled
        {
            get
            {
                return (bool)this["portSharingEnabled"];
            }
            set
            {
                this["portSharingEnabled"] = value;
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

                properties.Add(new ConfigurationProperty("listenBacklog", typeof(int), 10));
                properties.Add(new ConfigurationProperty("maxConnections", typeof(int), 10));
                properties.Add(new ConfigurationProperty("portSharingEnabled", typeof(bool), true));
                properties.Add(new ConfigurationProperty("maxBufferSize", typeof(int), 65536));
                properties.Add(new ConfigurationProperty("maxBufferPoolSize", typeof(long), 524288L));
                properties.Add(new ConfigurationProperty("maxReceivedMessageSize", typeof(long), 65536L));
                properties.Add(new ConfigurationProperty("hostNameComparisonMode", typeof(HostNameComparisonMode), HostNameComparisonMode.StrongWildcard));

                return properties;
            }
        }

        #endregion

        protected override void OnApplyConfiguration(Binding binding)
        {
            var protoBinding = (TcpProtoBufBinding)binding;

            protoBinding.SetDefaultCompressionBehaviour(this.CompressionType);
            protoBinding.SetOperationBehaviours(OperationBehaviours);

            var tcpBindingElement = (TcpTransportBindingElement)protoBinding.GetBindingElement();

            tcpBindingElement.TransferMode = TransferMode.Streamed; //buffered mode requires a duplex session channel which is not supported currently.

            tcpBindingElement.ListenBacklog = this.ListenBacklog;
            tcpBindingElement.MaxPendingConnections = this.MaxConnections;
            tcpBindingElement.PortSharingEnabled = this.PortSharingEnabled;
            
            tcpBindingElement.HostNameComparisonMode = this.HostNameComparisonMode;
            tcpBindingElement.MaxBufferPoolSize = this.MaxBufferPoolSize;
            tcpBindingElement.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            tcpBindingElement.MaxBufferSize = this.MaxBufferSize;
        }
    }
}