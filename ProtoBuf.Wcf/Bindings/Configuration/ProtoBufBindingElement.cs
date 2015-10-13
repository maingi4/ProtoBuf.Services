using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.Xml;
using ProtoBuf.Services.Wcf.Infrastructure;

namespace ProtoBuf.Services.Wcf.Bindings.Configuration
{
    public abstract class ProtoBufBindingElement : StandardBindingElement
    {
        protected ProtoBufBindingElement(string name)
            : base(name)
        {
        }

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

        [ConfigurationProperty("readerQuotas")]
        public XmlDictionaryReaderQuotasElement ReaderQuotas
        {
            get
            {
                return (XmlDictionaryReaderQuotasElement)this["readerQuotas"];
            }
            set
            {
                this["readerQuotas"] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                var properties = base.Properties;

                properties.Add(new ConfigurationProperty("compressionType", typeof(CompressionTypeOptions), CompressionTypeOptions.None));
                properties.Add(new ConfigurationProperty("operationBehaviours", typeof(OperationBehaviourElementCollection)));
                properties.Add(new ConfigurationProperty("readerQuotas", typeof(XmlDictionaryReaderQuotasElement)));

                return properties;
            }
        }

        protected override abstract void OnApplyConfiguration(Binding binding);

        protected void ApplyBaseConfiguration(ProtoBufBinding binding)
        {
            binding.SetDefaultCompressionBehaviour(this.CompressionType);
            binding.SetOperationBehaviours(this.OperationBehaviours);

            var encoding = binding.GetEncodingElement();

            var setReaderQuota = this.ReaderQuotas;

            if (setReaderQuota != null)
            {
                encoding.ReaderQuotas = new XmlDictionaryReaderQuotas()
                                            {
                                                MaxArrayLength = setReaderQuota.MaxArrayLength == 0 ? 16384 : setReaderQuota.MaxArrayLength,
                                                MaxBytesPerRead = setReaderQuota.MaxBytesPerRead == 0 ? 4096 : setReaderQuota.MaxBytesPerRead,
                                                MaxDepth = setReaderQuota.MaxDepth == 0 ? 32 : setReaderQuota.MaxDepth,
                                                MaxNameTableCharCount = setReaderQuota.MaxNameTableCharCount == 0 ? 16384 : setReaderQuota.MaxNameTableCharCount,
                                                MaxStringContentLength = setReaderQuota.MaxStringContentLength == 0 ? 8192 : setReaderQuota.MaxStringContentLength
                                            };
            }

        }
    }
}
