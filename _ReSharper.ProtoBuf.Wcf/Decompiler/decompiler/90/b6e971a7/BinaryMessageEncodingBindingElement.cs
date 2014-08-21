// Type: System.ServiceModel.Channels.BinaryMessageEncodingBindingElement
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.ComponentModel;
using System.Runtime;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// The binding element that specifies the .NET Binary Format for XML used to encode messages.
  /// </summary>
  [__DynamicallyInvokable]
  public sealed class BinaryMessageEncodingBindingElement : MessageEncodingBindingElement, IWsdlExportExtension, IPolicyExportExtension
  {
    private int maxReadPoolSize;
    private int maxWritePoolSize;
    private XmlDictionaryReaderQuotas readerQuotas;
    private int maxSessionSize;
    private BinaryVersion binaryVersion;
    private MessageVersion messageVersion;
    private CompressionFormat compressionFormat;
    private long maxReceivedMessageSize;

    /// <summary>
    /// Gets or sets the compression format for the binding element.
    /// </summary>
    /// 
    /// <returns>
    /// The compression format for the binding element.
    /// </returns>
    [DefaultValue(CompressionFormat.None)]
    [__DynamicallyInvokable]
    public CompressionFormat CompressionFormat
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.compressionFormat;
      }
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.compressionFormat = value;
      }
    }

    private BinaryVersion BinaryVersion
    {
      get
      {
        return this.binaryVersion;
      }
      set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("value"));
        this.binaryVersion = value;
      }
    }

    /// <summary>
    /// Gets or sets the SOAP message and WS-Addressing versions that are used or expected.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.MessageVersion"/> that is used or expected.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The value set is null.</exception><exception cref="T:System.ArgumentOutOfRangeException">The value set is an unsupported envelope version.</exception>
    [__DynamicallyInvokable]
    public override MessageVersion MessageVersion
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.messageVersion;
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        if (value.Envelope != BinaryEncoderDefaults.EnvelopeVersion)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("UnsupportedEnvelopeVersion", (object) this.GetType().FullName, (object) BinaryEncoderDefaults.EnvelopeVersion, (object) value.Envelope)));
        else
          this.messageVersion = MessageVersion.CreateVersion(BinaryEncoderDefaults.EnvelopeVersion, value.Addressing);
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of XML readers that are allocated to a pool and are ready for use to process incoming messages.
    /// </summary>
    /// 
    /// <returns>
    /// The maximum number of readers to be kept in the pool. The default value is 64 readers.
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value set is less than or equal to zero.</exception>
    [DefaultValue(64)]
    public int MaxReadPoolSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxReadPoolSize;
      }
      set
      {
        if (value <= 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBePositive")));
        this.maxReadPoolSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of XML writers that are allocated to a pool and are ready for use to process outgoing messages.
    /// </summary>
    /// 
    /// <returns>
    /// The maximum number of writers to be kept in the pool. The default value is 16 writers.
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than or equal to zero.</exception>
    [DefaultValue(16)]
    public int MaxWritePoolSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxWritePoolSize;
      }
      set
      {
        if (value <= 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBePositive")));
        this.maxWritePoolSize = value;
      }
    }

    /// <summary>
    /// Gets constraints on the complexity of XML messages that can be processed by endpoints configured with this binding element.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> that specifies the complexity constraints on SOAP messages exchanged. The default values for these constraints are provided in the following remarks section.
    /// </returns>
    [__DynamicallyInvokable]
    public XmlDictionaryReaderQuotas ReaderQuotas
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.readerQuotas;
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        value.CopyTo(this.readerQuotas);
      }
    }

    /// <summary>
    /// Gets or sets the maximum amount of memory available within a session for optimizing transmission procedures.
    /// </summary>
    /// 
    /// <returns>
    /// The maximum size, in bytes, of a session. The default value is 2048 bytes.
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than or equal to zero.</exception>
    [DefaultValue(2048)]
    [__DynamicallyInvokable]
    public int MaxSessionSize
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxSessionSize;
      }
      [__DynamicallyInvokable] set
      {
        if (value < 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")));
        this.maxSessionSize = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.BinaryMessageEncodingBindingElement"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    public BinaryMessageEncodingBindingElement()
    {
      this.maxReadPoolSize = 64;
      this.maxWritePoolSize = 16;
      this.readerQuotas = new XmlDictionaryReaderQuotas();
      EncoderDefaults.ReaderQuotas.CopyTo(this.readerQuotas);
      this.maxSessionSize = 2048;
      this.binaryVersion = BinaryEncoderDefaults.BinaryVersion;
      this.messageVersion = MessageVersion.CreateVersion(BinaryEncoderDefaults.EnvelopeVersion);
      this.compressionFormat = CompressionFormat.None;
    }

    private BinaryMessageEncodingBindingElement(BinaryMessageEncodingBindingElement elementToBeCloned)
      : base((MessageEncodingBindingElement) elementToBeCloned)
    {
      this.maxReadPoolSize = elementToBeCloned.maxReadPoolSize;
      this.maxWritePoolSize = elementToBeCloned.maxWritePoolSize;
      this.readerQuotas = new XmlDictionaryReaderQuotas();
      elementToBeCloned.readerQuotas.CopyTo(this.readerQuotas);
      this.MaxSessionSize = elementToBeCloned.MaxSessionSize;
      this.BinaryVersion = elementToBeCloned.BinaryVersion;
      this.messageVersion = elementToBeCloned.messageVersion;
      this.CompressionFormat = elementToBeCloned.CompressionFormat;
      this.maxReceivedMessageSize = elementToBeCloned.maxReceivedMessageSize;
    }

    private void VerifyCompression(BindingContext context)
    {
      if (this.compressionFormat == CompressionFormat.None)
        return;
      ITransportCompressionSupport innerProperty = context.GetInnerProperty<ITransportCompressionSupport>();
      if (innerProperty != null && innerProperty.IsCompressionFormatSupported(this.compressionFormat))
        return;
      throw FxTrace.Exception.AsError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("TransportDoesNotSupportCompression", (object) ((object) this.compressionFormat).ToString(), (object) this.GetType().Name, (object) ((object) CompressionFormat.None).ToString())));
    }

    private void SetMaxReceivedMessageSizeFromTransport(BindingContext context)
    {
      TransportBindingElement transportBindingElement = context.Binding.Elements.Find<TransportBindingElement>();
      if (transportBindingElement == null)
        return;
      this.maxReceivedMessageSize = transportBindingElement.MaxReceivedMessageSize;
    }

    /// <summary>
    /// Builds the channel factory stack on the client that creates a specified type of channel for a specified context.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.Channels.IChannelFactory`1"/> of type <paramref name="TChannel"/> for the specified context.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the channel.</param><typeparam name="TChannel">The type of channel the channel factory produces.</typeparam>
    [__DynamicallyInvokable]
    public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
    {
      this.VerifyCompression(context);
      this.SetMaxReceivedMessageSizeFromTransport(context);
      return this.InternalBuildChannelFactory<TChannel>(context);
    }

    /// <summary>
    /// Builds the channel listener on the service that accepts a specified type of channel for a specified context.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.Channels.IChannelListener`1"/> of type <paramref name="TChannel"/> for the specified context.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the listener.</param><typeparam name="TChannel">The type of channel the channel listener accepts.</typeparam>
    public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
    {
      this.VerifyCompression(context);
      this.SetMaxReceivedMessageSizeFromTransport(context);
      return this.InternalBuildChannelListener<TChannel>(context);
    }

    /// <summary>
    /// Returns a value that indicates whether the current binding can build a listener for a specified type of channel and context.
    /// </summary>
    /// 
    /// <returns>
    /// true if the specified channel listener stack can be built on the service; otherwise, false.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the listener.</param><typeparam name="TChannel">The type of channel the channel listener accepts.</typeparam>
    public override bool CanBuildChannelListener<TChannel>(BindingContext context)
    {
      return this.InternalCanBuildChannelListener<TChannel>(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.ServiceModel.Channels.BinaryMessageEncodingBindingElement"/> object initialized from the current one.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.BinaryMessageEncodingBindingElement"/> object with property values equal to those of the current element.
    /// </returns>
    [__DynamicallyInvokable]
    public override BindingElement Clone()
    {
      return (BindingElement) new BinaryMessageEncodingBindingElement(this);
    }

    /// <summary>
    /// Creates a factory for binary message encoders that employ the SOAP and WS-Addressing versions and the character encoding specified by the current encoding binding element.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.MessageEncoderFactory"/> that this binding element creates.
    /// </returns>
    [__DynamicallyInvokable]
    public override MessageEncoderFactory CreateMessageEncoderFactory()
    {
      return (MessageEncoderFactory) new BinaryMessageEncoderFactory(this.MessageVersion, this.MaxReadPoolSize, this.MaxWritePoolSize, this.MaxSessionSize, this.ReaderQuotas, this.maxReceivedMessageSize, this.BinaryVersion, this.CompressionFormat);
    }

    /// <summary>
    /// Returns a typed object requested, if present, from the appropriate layer in the binding element stack.
    /// </summary>
    /// 
    /// <returns>
    /// The typed object <paramref name="T"/> requested if it is present or null if it is not.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the binding element.</param><typeparam name="T">The typed object for which the method is querying.</typeparam>
    [__DynamicallyInvokable]
    public override T GetProperty<T>(BindingContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      if (typeof (T) == typeof (XmlDictionaryReaderQuotas))
        return (T) this.readerQuotas;
      else
        return base.GetProperty<T>(context);
    }

    void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext policyContext)
    {
      if (policyContext == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("policyContext");
      XmlDocument xmlDocument = new XmlDocument();
      policyContext.GetBindingAssertions().Add(xmlDocument.CreateElement("msb", "BinaryEncoding", "http://schemas.microsoft.com/ws/06/2004/mspolicy/netbinary1"));
    }

    void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
    {
    }

    void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      SoapHelper.SetSoapVersion(context, exporter, MessageVersion.Soap12WSAddressing10.Envelope);
    }

    internal override bool IsMatch(BindingElement b)
    {
      if (!base.IsMatch(b))
        return false;
      BinaryMessageEncodingBindingElement encodingBindingElement = b as BinaryMessageEncodingBindingElement;
      return encodingBindingElement != null && this.maxReadPoolSize == encodingBindingElement.MaxReadPoolSize && (this.maxWritePoolSize == encodingBindingElement.MaxWritePoolSize && this.readerQuotas.MaxStringContentLength == encodingBindingElement.ReaderQuotas.MaxStringContentLength) && (this.readerQuotas.MaxArrayLength == encodingBindingElement.ReaderQuotas.MaxArrayLength && this.readerQuotas.MaxBytesPerRead == encodingBindingElement.ReaderQuotas.MaxBytesPerRead && (this.readerQuotas.MaxDepth == encodingBindingElement.ReaderQuotas.MaxDepth && this.readerQuotas.MaxNameTableCharCount == encodingBindingElement.ReaderQuotas.MaxNameTableCharCount)) && (this.MaxSessionSize == encodingBindingElement.MaxSessionSize && this.CompressionFormat == encodingBindingElement.CompressionFormat);
    }

    /// <summary>
    /// Returns whether the values of constraints placed on the complexity of SOAP message structure should be serialized.
    /// </summary>
    /// 
    /// <returns>
    /// true if reader quotas should be serialized; otherwise, false.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeReaderQuotas()
    {
      return !EncoderDefaults.IsDefaultReaderQuotas(this.ReaderQuotas);
    }

    /// <summary>
    /// Returns whether the SOAP message structure version should be serialized.
    /// </summary>
    /// 
    /// <returns>
    /// true if the SOAP message structure version should be serialized; otherwise, false.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeMessageVersion()
    {
      return !this.messageVersion.IsMatch(MessageVersion.Default);
    }
  }
}
