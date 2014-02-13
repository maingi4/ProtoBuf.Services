// Type: System.ServiceModel.Channels.HttpTransportBindingElement
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.Runtime;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Services.Description;
using System.Xml;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Represents the binding element used to specify an HTTP transport for transmitting messages.
  /// </summary>
  [__DynamicallyInvokable]
  public class HttpTransportBindingElement : TransportBindingElement, IWsdlExportExtension, IPolicyExportExtension, ITransportPolicyImport
  {
    private bool allowCookies;
    private AuthenticationSchemes authenticationScheme;
    private bool bypassProxyOnLocal;
    private bool decompressionEnabled;
    private HostNameComparisonMode hostNameComparisonMode;
    private bool keepAliveEnabled;
    private bool inheritBaseAddressSettings;
    private int maxBufferSize;
    private bool maxBufferSizeInitialized;
    private string method;
    private Uri proxyAddress;
    private AuthenticationSchemes proxyAuthenticationScheme;
    private string realm;
    private TimeSpan requestInitializationTimeout;
    private TransferMode transferMode;
    private bool unsafeConnectionNtlmAuthentication;
    private bool useDefaultWebProxy;
    private WebSocketTransportSettings webSocketSettings;
    private IWebProxy webProxy;
    private ExtendedProtectionPolicy extendedProtectionPolicy;
    private HttpAnonymousUriPrefixMatcher anonymousUriPrefixMatcher;
    private HttpMessageHandlerFactory httpMessageHandlerFactory;
    private int maxPendingAccepts;

    /// <summary>
    /// Gets or sets a value that indicates whether the client accepts cookies and propagates them on future requests.
    /// </summary>
    /// 
    /// <returns>
    /// true if cookies are allowed; otherwise, false. The default is false.
    /// </returns>
    [DefaultValue(false)]
    [__DynamicallyInvokable]
    public bool AllowCookies
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.allowCookies;
      }
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.allowCookies = value;
      }
    }

    /// <summary>
    /// Gets or sets the authentication scheme used to authenticate client requests being processed by an HTTP listener.
    /// </summary>
    /// 
    /// <returns>
    /// One of the enumeration values of the <see cref="T:System.Net.AuthenticationSchemes"/> enumeration that specifies the protocols used for client authentication. The default is <see cref="F:System.Net.AuthenticationSchemes.Anonymous"/>.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">The value for the <see cref="T:System.Net.AuthenticationSchemes"/> was already set.</exception>
    [DefaultValue(AuthenticationSchemes.Anonymous)]
    [__DynamicallyInvokable]
    public AuthenticationSchemes AuthenticationScheme
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.authenticationScheme;
      }
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.authenticationScheme = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether proxies are ignored for local addresses.
    /// </summary>
    /// 
    /// <returns>
    /// true if proxies are ignored for local addresses; otherwise, false. The default is false.
    /// </returns>
    [DefaultValue(false)]
    public bool BypassProxyOnLocal
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.bypassProxyOnLocal;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.bypassProxyOnLocal = value;
      }
    }

    /// <summary>
    /// Gets or sets whether the process for returning compressed message data to its original size and format is enabled.
    /// </summary>
    /// 
    /// <returns>
    /// true if decompression is enabled; otherwise, false.
    /// </returns>
    [DefaultValue(true)]
    public bool DecompressionEnabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.decompressionEnabled;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.decompressionEnabled = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the hostname is used to reach the service when matching on the URI.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.HostnameComparisonMode"/> enumeration value that indicates whether the hostname is included when routing incoming requests to an endpoint URI. The default value is <see cref="F:System.ServiceModel.HostnameComparisonMode.StrongWildcard"/>, which ignores the hostname in the match.
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value set is not defined.</exception>
    [DefaultValue(HostNameComparisonMode.StrongWildcard)]
    public HostNameComparisonMode HostNameComparisonMode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.hostNameComparisonMode;
      }
      set
      {
        HostNameComparisonModeHelper.Validate(value);
        this.hostNameComparisonMode = value;
      }
    }

    /// <summary>
    /// Gets or sets the Http transport message handler factory.
    /// </summary>
    /// 
    /// <returns>
    /// The Http transport message handler factory.
    /// </returns>
    public HttpMessageHandlerFactory MessageHandlerFactory
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.httpMessageHandlerFactory;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.httpMessageHandlerFactory = value;
      }
    }

    /// <summary>
    /// Gets or sets the value of the extended security policy used by the server to validate incoming client connections.
    /// </summary>
    /// 
    /// <returns>
    /// The value of the extended security policy used by the server to validate incoming client connections.
    /// </returns>
    public ExtendedProtectionPolicy ExtendedProtectionPolicy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.extendedProtectionPolicy;
      }
      set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        if (value.PolicyEnforcement == PolicyEnforcement.Always && !ExtendedProtectionPolicy.OSSupportsExtendedProtection)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new PlatformNotSupportedException(System.ServiceModel.SR.GetString("ExtendedProtectionNotSupported")));
        this.extendedProtectionPolicy = value;
      }
    }

    internal bool InheritBaseAddressSettings
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.inheritBaseAddressSettings;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.inheritBaseAddressSettings = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether to make a persistent connection to a service endpoint.
    /// </summary>
    /// 
    /// <returns>
    /// true if the request to the service endpoint should contain a Connection HTTP header with the value Keep-alive; otherwise, false. The default is true.
    /// </returns>
    [DefaultValue(true)]
    public bool KeepAliveEnabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.keepAliveEnabled;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.keepAliveEnabled = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum size of the buffer to use. For buffered messages this value is the same as <see cref="P:System.ServiceModel.Channels.TransportBindingElement.MaxReceivedMessageSize"/>. For streamed messages, this value is the maximum size of the SOAP headers, which must be read in buffered mode.
    /// </summary>
    /// 
    /// <returns>
    /// The maximum size, in bytes, of the buffer.
    /// </returns>
    [DefaultValue(65536)]
    [__DynamicallyInvokable]
    public int MaxBufferSize
    {
      [__DynamicallyInvokable] get
      {
        if (this.maxBufferSizeInitialized || this.TransferMode != TransferMode.Buffered)
          return this.maxBufferSize;
        long receivedMessageSize = this.MaxReceivedMessageSize;
        if (receivedMessageSize > (long) int.MaxValue)
          return int.MaxValue;
        else
          return (int) receivedMessageSize;
      }
      [__DynamicallyInvokable] set
      {
        if (value <= 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBePositive")));
        this.maxBufferSizeInitialized = true;
        this.maxBufferSize = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of connections the service can accept simultaneously.
    /// </summary>
    /// 
    /// <returns>
    /// The maximum number of connections the service can accept simultaneously.
    /// </returns>
    [DefaultValue(0)]
    public int MaxPendingAccepts
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxPendingAccepts;
      }
      set
      {
        if (value < 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")));
        if (value > 100000)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("HttpMaxPendingAcceptsTooLargeError", new object[1]
          {
            (object) 100000
          })));
        else
          this.maxPendingAccepts = value;
      }
    }

    internal string Method
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.method;
      }
      set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        this.method = value;
      }
    }

    /// <summary>
    /// Gets or sets a URI that contains the address of the proxy to use for HTTP requests.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Uri"/> that contains the address for the proxy. The default value is null.
    /// </returns>
    [DefaultValue(null)]
    [TypeConverter(typeof (UriTypeConverter))]
    public Uri ProxyAddress
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.proxyAddress;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.proxyAddress = value;
      }
    }

    /// <summary>
    /// Gets or sets the authentication scheme used to authenticate client requests being processed by an HTTP proxy.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Net.AuthenticationSchemes"/> enumeration that specifies the protocols used for client authentication on the proxy. The default is <see cref="F:System.Net.AuthenticationSchemes.Anonymous"/>.
    /// </returns>
    [DefaultValue(AuthenticationSchemes.Anonymous)]
    public AuthenticationSchemes ProxyAuthenticationScheme
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.proxyAuthenticationScheme;
      }
      set
      {
        if (!AuthenticationSchemesHelper.IsSingleton(value))
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("value", System.ServiceModel.SR.GetString("HttpProxyRequiresSingleAuthScheme", new object[1]
          {
            (object) value
          }));
        else
          this.proxyAuthenticationScheme = value;
      }
    }

    internal IWebProxy Proxy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.webProxy;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.webProxy = value;
      }
    }

    /// <summary>
    /// Gets or sets the authentication realm.
    /// </summary>
    /// 
    /// <returns>
    /// The authentication realm. The default value is "".
    /// </returns>
    [DefaultValue("")]
    public string Realm
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.realm;
      }
      set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        this.realm = value;
      }
    }

    /// <summary>
    /// Gets or sets the requested initialization time out.
    /// </summary>
    /// 
    /// <returns>
    /// The requested initialization time out.
    /// </returns>
    [DefaultValue(typeof (TimeSpan), "00:00:00")]
    public TimeSpan RequestInitializationTimeout
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.requestInitializationTimeout;
      }
      set
      {
        if (value < TimeSpan.Zero)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
        if (TimeoutHelper.IsTooLarge(value))
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRangeTooBig")));
        this.requestInitializationTimeout = value;
      }
    }

    /// <summary>
    /// Gets the URI scheme for the transport.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="F:System.Uri.UriSchemeHttp"/> object that represents the URI scheme for the transport.
    /// </returns>
    [__DynamicallyInvokable]
    public override string Scheme
    {
      [__DynamicallyInvokable] get
      {
        return "http";
      }
    }

    /// <summary>
    /// Gets or sets the transfer mode.
    /// </summary>
    /// 
    /// <returns>
    /// One of the following member values of <see cref="P:System.ServiceModel.Channels.HttpTransportBindingElement.TransferMode"/>:BufferedStreamedStreamedRequestStreamedResponse
    /// </returns>
    [DefaultValue(TransferMode.Buffered)]
    [__DynamicallyInvokable]
    public TransferMode TransferMode
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.transferMode;
      }
      [__DynamicallyInvokable] set
      {
        TransferModeHelper.Validate(value);
        this.transferMode = value;
      }
    }

    /// <summary>
    /// Gets or sets the web socket configuration of the binding element.
    /// </summary>
    /// 
    /// <returns>
    /// The web socket settings.
    /// </returns>
    [__DynamicallyInvokable]
    public WebSocketTransportSettings WebSocketSettings
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.webSocketSettings;
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        this.webSocketSettings = value;
      }
    }

    internal HttpAnonymousUriPrefixMatcher AnonymousUriPrefixMatcher
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.anonymousUriPrefixMatcher;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether Unsafe Connection Sharing is enabled on the server. If enabled, NTLM authentication is performed once on each TCP connection.
    /// </summary>
    /// 
    /// <returns>
    /// true if Unsafe Connection Sharing is enabled; otherwise, false. The default is false.
    /// </returns>
    [DefaultValue(false)]
    public bool UnsafeConnectionNtlmAuthentication
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.unsafeConnectionNtlmAuthentication;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.unsafeConnectionNtlmAuthentication = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the machine-wide proxy settings are used rather than the user specific settings.
    /// </summary>
    /// 
    /// <returns>
    /// true if the machine-wide proxy settings are used; otherwise, false. The default is true.
    /// </returns>
    [DefaultValue(true)]
    public bool UseDefaultWebProxy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.useDefaultWebProxy;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.useDefaultWebProxy = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.HttpTransportBindingElement"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    public HttpTransportBindingElement()
    {
      this.allowCookies = false;
      this.authenticationScheme = AuthenticationSchemes.Anonymous;
      this.bypassProxyOnLocal = false;
      this.decompressionEnabled = true;
      this.hostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
      this.keepAliveEnabled = true;
      this.maxBufferSize = 65536;
      this.maxPendingAccepts = 0;
      this.method = string.Empty;
      this.proxyAuthenticationScheme = AuthenticationSchemes.Anonymous;
      this.proxyAddress = (Uri) null;
      this.realm = "";
      this.requestInitializationTimeout = HttpTransportDefaults.RequestInitializationTimeout;
      this.transferMode = TransferMode.Buffered;
      this.unsafeConnectionNtlmAuthentication = false;
      this.useDefaultWebProxy = true;
      this.webSocketSettings = HttpTransportDefaults.GetDefaultWebSocketTransportSettings();
      this.webProxy = (IWebProxy) null;
      this.extendedProtectionPolicy = ChannelBindingUtility.DefaultPolicy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.HttpTransportBindingElement"/> class using another binding element.
    /// </summary>
    /// <param name="elementToBeCloned">An <see cref="T:System.ServiceModel.Channels.HttpTransportBindingElement"/> object used to initialize this instance.</param>
    [__DynamicallyInvokable]
    protected HttpTransportBindingElement(HttpTransportBindingElement elementToBeCloned)
      : base((TransportBindingElement) elementToBeCloned)
    {
      this.allowCookies = elementToBeCloned.allowCookies;
      this.authenticationScheme = elementToBeCloned.authenticationScheme;
      this.bypassProxyOnLocal = elementToBeCloned.bypassProxyOnLocal;
      this.decompressionEnabled = elementToBeCloned.decompressionEnabled;
      this.hostNameComparisonMode = elementToBeCloned.hostNameComparisonMode;
      this.inheritBaseAddressSettings = elementToBeCloned.InheritBaseAddressSettings;
      this.keepAliveEnabled = elementToBeCloned.keepAliveEnabled;
      this.maxBufferSize = elementToBeCloned.maxBufferSize;
      this.maxBufferSizeInitialized = elementToBeCloned.maxBufferSizeInitialized;
      this.maxPendingAccepts = elementToBeCloned.maxPendingAccepts;
      this.method = elementToBeCloned.method;
      this.proxyAddress = elementToBeCloned.proxyAddress;
      this.proxyAuthenticationScheme = elementToBeCloned.proxyAuthenticationScheme;
      this.realm = elementToBeCloned.realm;
      this.requestInitializationTimeout = elementToBeCloned.requestInitializationTimeout;
      this.transferMode = elementToBeCloned.transferMode;
      this.unsafeConnectionNtlmAuthentication = elementToBeCloned.unsafeConnectionNtlmAuthentication;
      this.useDefaultWebProxy = elementToBeCloned.useDefaultWebProxy;
      this.webSocketSettings = elementToBeCloned.webSocketSettings.Clone();
      this.webProxy = elementToBeCloned.webProxy;
      this.extendedProtectionPolicy = elementToBeCloned.ExtendedProtectionPolicy;
      if (elementToBeCloned.anonymousUriPrefixMatcher != null)
        this.anonymousUriPrefixMatcher = new HttpAnonymousUriPrefixMatcher(elementToBeCloned.anonymousUriPrefixMatcher);
      this.MessageHandlerFactory = elementToBeCloned.MessageHandlerFactory;
    }

    internal virtual bool GetSupportsClientAuthenticationImpl(AuthenticationSchemes effectiveAuthenticationSchemes)
    {
      if (effectiveAuthenticationSchemes != AuthenticationSchemes.None)
        return AuthenticationSchemesHelper.IsNotSet(effectiveAuthenticationSchemes, AuthenticationSchemes.Anonymous);
      else
        return false;
    }

    internal virtual bool GetSupportsClientWindowsIdentityImpl(AuthenticationSchemes effectiveAuthenticationSchemes)
    {
      if (effectiveAuthenticationSchemes != AuthenticationSchemes.None)
        return AuthenticationSchemesHelper.IsNotSet(effectiveAuthenticationSchemes, AuthenticationSchemes.Anonymous);
      else
        return false;
    }

    internal string GetWsdlTransportUri(bool useWebSocketTransport)
    {
      return useWebSocketTransport ? "http://schemas.microsoft.com/soap/websocket" : "http://schemas.xmlsoap.org/soap/http";
    }

    /// <summary>
    /// Creates a new instance that is a copy of the current binding element.
    /// </summary>
    /// 
    /// <returns>
    /// A new instance that is a copy of the current binding element.
    /// </returns>
    [__DynamicallyInvokable]
    public override BindingElement Clone()
    {
      return (BindingElement) new HttpTransportBindingElement(this);
    }

    /// <summary>
    /// Gets a property from the specified <see cref="T:System.ServiceModel.Channels.BindingContext"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The property from the specified <see cref="T:System.ServiceModel.Channels.BindingContext"/>.
    /// </returns>
    /// <param name="context">A <see cref="T:System.ServiceModel.Channels.BindingContext"/>.</param><typeparam name="T">The type of the property to get.</typeparam>
    [__DynamicallyInvokable]
    public override T GetProperty<T>(BindingContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      if (typeof (T) == typeof (ISecurityCapabilities))
      {
        AuthenticationSchemes authenticationSchemes = HttpTransportBindingElement.GetEffectiveAuthenticationSchemes(this.AuthenticationScheme, context.BindingParameters);
        return (T) new SecurityCapabilities(this.GetSupportsClientAuthenticationImpl(authenticationSchemes), authenticationSchemes == AuthenticationSchemes.Negotiate, this.GetSupportsClientWindowsIdentityImpl(authenticationSchemes), ProtectionLevel.None, ProtectionLevel.None);
      }
      else
      {
        if (typeof (T) == typeof (IBindingDeliveryCapabilities))
          return (T) new HttpTransportBindingElement.BindingDeliveryCapabilitiesHelper();
        if (typeof (T) == typeof (TransferMode))
          return (T) (System.Enum) this.TransferMode;
        if (typeof (T) == typeof (ExtendedProtectionPolicy))
          return (T) this.ExtendedProtectionPolicy;
        if (typeof (T) == typeof (IAnonymousUriPrefixMatcher))
        {
          if (this.anonymousUriPrefixMatcher == null)
            this.anonymousUriPrefixMatcher = new HttpAnonymousUriPrefixMatcher();
          return (T) this.anonymousUriPrefixMatcher;
        }
        else
        {
          if (typeof (T) == typeof (ITransportCompressionSupport))
            return (T) new HttpTransportBindingElement.TransportCompressionSupportHelper();
          if (context.BindingParameters.Find<MessageEncodingBindingElement>() == null)
            context.BindingParameters.Add((object) new TextMessageEncodingBindingElement());
          return base.GetProperty<T>(context);
        }
      }
    }

    /// <summary>
    /// Determines whether a channel factory of the specified type can be built.
    /// </summary>
    /// 
    /// <returns>
    /// true if a channel factory can be built; otherwise false.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the channel.</param><typeparam name="TChannel">The type of channel to check.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="context"/> is null.</exception>
    [__DynamicallyInvokable]
    public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
    {
      if (typeof (TChannel) == typeof (IRequestChannel))
        return this.WebSocketSettings.TransportUsage != WebSocketTransportUsage.Always;
      if (typeof (TChannel) == typeof (IDuplexSessionChannel))
        return this.WebSocketSettings.TransportUsage != WebSocketTransportUsage.Never;
      else
        return false;
    }

    /// <summary>
    /// Determines whether a channel listener of the specified type can be built.
    /// </summary>
    /// 
    /// <returns>
    /// true if a channel listener can be built; otherwise false.
    /// </returns>
    /// <param name="context">The <see cref="T:System.ServiceModel.Channels.BindingContext"/> for the channel.</param><typeparam name="TChannel">The type of channel to check.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="context"/> is null.</exception>
    public override bool CanBuildChannelListener<TChannel>(BindingContext context)
    {
      if (typeof (TChannel) == typeof (IReplyChannel))
        return this.WebSocketSettings.TransportUsage != WebSocketTransportUsage.Always;
      if (typeof (TChannel) == typeof (IDuplexSessionChannel))
        return this.WebSocketSettings.TransportUsage != WebSocketTransportUsage.Never;
      else
        return false;
    }

    /// <summary>
    /// Creates a channel factory that can be used to create a channel.
    /// </summary>
    /// 
    /// <returns>
    /// A channel factory of the specified type.
    /// </returns>
    /// <param name="context"><see cref="T:System.ServiceModel.Channels.BindingContext"/> members that describe bindings, behaviors, contracts and other information required to create the channel factory.</param><typeparam name="TChannel">The type of channel factory.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="context"/> cannot be null.</exception><exception cref="T:System.ArgumentException">An invalid argument was passed.</exception>
    [__DynamicallyInvokable]
    public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      if (this.MessageHandlerFactory != null)
        throw System.ServiceModel.FxTrace.Exception.AsError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("HttpPipelineNotSupportedOnClientSide", new object[1]
        {
          (object) "MessageHandlerFactory"
        })));
      else if (!this.CanBuildChannelFactory<TChannel>(context))
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("TChannel", System.ServiceModel.SR.GetString("CouldnTCreateChannelForChannelType2", (object) context.Binding.Name, (object) typeof (TChannel)));
      else if (this.authenticationScheme == AuthenticationSchemes.None)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("value", System.ServiceModel.SR.GetString("HttpAuthSchemeCannotBeNone", new object[1]
        {
          (object) this.authenticationScheme
        }));
      }
      else
      {
        if (AuthenticationSchemesHelper.IsSingleton(this.authenticationScheme))
          return (IChannelFactory<TChannel>) new HttpChannelFactory<TChannel>(this, context);
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("value", System.ServiceModel.SR.GetString("HttpRequiresSingleAuthScheme", new object[1]
        {
          (object) this.authenticationScheme
        }));
      }
    }

    internal static AuthenticationSchemes GetEffectiveAuthenticationSchemes(AuthenticationSchemes currentAuthenticationSchemes, BindingParameterCollection bindingParameters)
    {
      AuthenticationSchemes authenticationSchemes;
      if (bindingParameters == null || !AuthenticationSchemesBindingParameter.TryExtract(bindingParameters, out authenticationSchemes))
        return currentAuthenticationSchemes;
      if (currentAuthenticationSchemes != AuthenticationSchemes.None && (!AspNetEnvironment.Current.IsMetadataListener(bindingParameters) || currentAuthenticationSchemes != AuthenticationSchemes.Anonymous || !AuthenticationSchemesHelper.IsNotSet(authenticationSchemes, AuthenticationSchemes.Anonymous)))
        return currentAuthenticationSchemes & authenticationSchemes;
      if (!AuthenticationSchemesHelper.IsSingleton(authenticationSchemes) && AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Anonymous) && (AspNetEnvironment.Current.AspNetCompatibilityEnabled && AspNetEnvironment.Current.IsSimpleApplicationHost) && AspNetEnvironment.Current.IsWindowsAuthenticationConfigured())
        authenticationSchemes ^= AuthenticationSchemes.Anonymous;
      return authenticationSchemes;
    }

    /// <summary>
    /// Creates a channel listener of the specified type.
    /// </summary>
    /// 
    /// <returns>
    /// A channel listener of the specified type.
    /// </returns>
    /// <param name="context"><see cref="T:System.ServiceModel.Channels.BindingContext"/> members that describe bindings, behaviors, contracts and other information required to create the channel factory.</param><typeparam name="TChannel">The type of channel factory.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="context"/> cannot be null.</exception><exception cref="T:System.ArgumentException">An invalid argument was passed.</exception>
    public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      if (!this.CanBuildChannelListener<TChannel>(context))
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("TChannel", System.ServiceModel.SR.GetString("CouldnTCreateChannelForChannelType2", (object) context.Binding.Name, (object) typeof (TChannel)));
      }
      else
      {
        this.UpdateAuthenticationSchemes(context);
        HttpChannelListener httpChannelListener = (HttpChannelListener) new HttpChannelListener<TChannel>(this, context);
        AspNetEnvironment.Current.ApplyHostedContext((TransportChannelListener) httpChannelListener, context);
        return (IChannelListener<TChannel>) httpChannelListener;
      }
    }

    /// <summary>
    /// Updates the transport authentication schemes that contains the binding context.
    /// </summary>
    /// <param name="context">The binding context.</param>
    protected void UpdateAuthenticationSchemes(BindingContext context)
    {
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      AuthenticationSchemes authenticationSchemes1 = HttpTransportBindingElement.GetEffectiveAuthenticationSchemes(this.AuthenticationScheme, context.BindingParameters);
      if (authenticationSchemes1 == AuthenticationSchemes.None)
      {
        string name = context.Binding.Name;
        if (this.AuthenticationScheme == AuthenticationSchemes.None)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("AuthenticationSchemesCannotBeInheritedFromHost", new object[1]
          {
            (object) name
          })));
        }
        else
        {
          AuthenticationSchemes authenticationSchemes2;
          AuthenticationSchemesBindingParameter.TryExtract(context.BindingParameters, out authenticationSchemes2);
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("AuthenticationSchemes_BindingAndHostConflict", (object) authenticationSchemes2, (object) name, (object) this.AuthenticationScheme)));
        }
      }
      else
        this.AuthenticationScheme = authenticationSchemes1;
    }

    void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext context)
    {
      if (exporter == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("exporter");
      if (context == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("context");
      this.OnExportPolicy(exporter, context);
      bool createdNew;
      MessageEncodingBindingElement encodingBindingElement = this.FindMessageEncodingBindingElement(context.BindingElements, out createdNew);
      if (createdNew && encodingBindingElement is IPolicyExportExtension)
        ((IPolicyExportExtension) encodingBindingElement).ExportPolicy(exporter, context);
      WsdlExporter.WSAddressingHelper.AddWSAddressingAssertion(exporter, context, encodingBindingElement.MessageVersion.Addressing);
    }

    internal virtual void OnExportPolicy(MetadataExporter exporter, PolicyConversionContext policyContext)
    {
      List<string> list = new List<string>();
      AuthenticationSchemes authenticationSchemes = HttpTransportBindingElement.GetEffectiveAuthenticationSchemes(this.AuthenticationScheme, policyContext.BindingParameters);
      if (authenticationSchemes != AuthenticationSchemes.None && !AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Anonymous))
      {
        if (AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Negotiate))
          list.Add("NegotiateAuthentication");
        if (AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Ntlm))
          list.Add("NtlmAuthentication");
        if (AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Digest))
          list.Add("DigestAuthentication");
        if (AuthenticationSchemesHelper.IsSet(authenticationSchemes, AuthenticationSchemes.Basic))
          list.Add("BasicAuthentication");
        if (list != null && list.Count > 0)
        {
          if (list.Count == 1)
          {
            policyContext.GetBindingAssertions().Add(new XmlDocument().CreateElement("http", list[0], "http://schemas.microsoft.com/ws/06/2004/policy/http"));
          }
          else
          {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement element = xmlDocument.CreateElement("wsp", "ExactlyOne", exporter.PolicyVersion.Namespace);
            foreach (string localName in list)
              element.AppendChild((XmlNode) xmlDocument.CreateElement("http", localName, "http://schemas.microsoft.com/ws/06/2004/policy/http"));
            policyContext.GetBindingAssertions().Add(element);
          }
        }
      }
      if (!WebSocketHelper.UseWebSocketTransport(this.WebSocketSettings.TransportUsage, policyContext.Contract.IsDuplex()) || this.TransferMode == TransferMode.Buffered)
        return;
      policyContext.GetBindingAssertions().Add(new XmlDocument().CreateElement("mswsp", ((object) this.TransferMode).ToString(), "http://schemas.microsoft.com/soap/websocket/policy"));
    }

    internal virtual void OnImportPolicy(MetadataImporter importer, PolicyConversionContext policyContext)
    {
    }

    void ITransportPolicyImport.ImportPolicy(MetadataImporter importer, PolicyConversionContext policyContext)
    {
      ICollection<XmlElement> bindingAssertions = (ICollection<XmlElement>) policyContext.GetBindingAssertions();
      List<XmlElement> list = new List<XmlElement>();
      bool flag = false;
      foreach (XmlElement xmlElement in (IEnumerable<XmlElement>) bindingAssertions)
      {
        if (!(xmlElement.NamespaceURI != "http://schemas.microsoft.com/ws/06/2004/policy/http"))
        {
          switch (xmlElement.LocalName)
          {
            case "BasicAuthentication":
              this.AuthenticationScheme = AuthenticationSchemes.Basic;
              break;
            case "DigestAuthentication":
              this.AuthenticationScheme = AuthenticationSchemes.Digest;
              break;
            case "NegotiateAuthentication":
              this.AuthenticationScheme = AuthenticationSchemes.Negotiate;
              break;
            case "NtlmAuthentication":
              this.AuthenticationScheme = AuthenticationSchemes.Ntlm;
              break;
            default:
              continue;
          }
          if (flag)
          {
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("HttpTransportCannotHaveMultipleAuthenticationSchemes", (object) policyContext.Contract.Namespace, (object) policyContext.Contract.Name)));
          }
          else
          {
            flag = true;
            list.Add(xmlElement);
          }
        }
      }
      list.ForEach((Action<XmlElement>) (element => bindingAssertions.Remove(element)));
      if (this.WebSocketSettings.TransportUsage == WebSocketTransportUsage.Always)
      {
        foreach (XmlElement xmlElement in (IEnumerable<XmlElement>) bindingAssertions)
        {
          if (!(xmlElement.NamespaceURI != "http://schemas.microsoft.com/soap/websocket/policy"))
          {
            string localName = xmlElement.LocalName;
            TransferMode result;
            if (!System.Enum.TryParse<TransferMode>(localName, true, out result) || !TransferModeHelper.IsDefined(result) || result == TransferMode.Buffered)
            {
              throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("WebSocketTransportPolicyAssertionInvalid", (object) policyContext.Contract.Namespace, (object) policyContext.Contract.Name, (object) localName, (object) TransferMode.Streamed, (object) TransferMode.StreamedRequest, (object) TransferMode.StreamedResponse)));
            }
            else
            {
              this.TransferMode = result;
              bindingAssertions.Remove(xmlElement);
              break;
            }
          }
        }
      }
      this.OnImportPolicy(importer, policyContext);
    }

    void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
    {
    }

    void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext endpointContext)
    {
      bool createdNew;
      MessageEncodingBindingElement encodingBindingElement = this.FindMessageEncodingBindingElement(endpointContext, out createdNew);
      bool useWebSocketTransport = WebSocketHelper.UseWebSocketTransport(this.WebSocketSettings.TransportUsage, endpointContext.ContractConversionContext.Contract.IsDuplex());
      EndpointAddress address = endpointContext.Endpoint.Address;
      if (useWebSocketTransport)
      {
        address = new EndpointAddress(WebSocketHelper.GetWebSocketUri(endpointContext.Endpoint.Address.Uri), endpointContext.Endpoint.Address);
        SoapAddressBinding soapAddressBinding = SoapHelper.GetSoapAddressBinding(endpointContext.WsdlPort);
        if (soapAddressBinding != null)
          soapAddressBinding.Location = address.Uri.AbsoluteUri;
      }
      TransportBindingElement.ExportWsdlEndpoint(exporter, endpointContext, this.GetWsdlTransportUri(useWebSocketTransport), address, encodingBindingElement.MessageVersion.Addressing);
    }

    internal override bool IsMatch(BindingElement b)
    {
      if (!base.IsMatch(b))
        return false;
      HttpTransportBindingElement transportBindingElement = b as HttpTransportBindingElement;
      return transportBindingElement != null && this.allowCookies == transportBindingElement.allowCookies && (this.authenticationScheme == transportBindingElement.authenticationScheme && this.decompressionEnabled == transportBindingElement.decompressionEnabled) && (this.hostNameComparisonMode == transportBindingElement.hostNameComparisonMode && this.inheritBaseAddressSettings == transportBindingElement.inheritBaseAddressSettings && (this.keepAliveEnabled == transportBindingElement.keepAliveEnabled && this.maxBufferSize == transportBindingElement.maxBufferSize)) && (!(this.method != transportBindingElement.method) && !(this.proxyAddress != transportBindingElement.proxyAddress) && (this.proxyAuthenticationScheme == transportBindingElement.proxyAuthenticationScheme && !(this.realm != transportBindingElement.realm)) && (this.transferMode == transportBindingElement.transferMode && this.unsafeConnectionNtlmAuthentication == transportBindingElement.unsafeConnectionNtlmAuthentication && (this.useDefaultWebProxy == transportBindingElement.useDefaultWebProxy && this.WebSocketSettings.Equals(transportBindingElement.WebSocketSettings)))) && (this.webProxy == transportBindingElement.webProxy && ChannelBindingUtility.AreEqual(this.ExtendedProtectionPolicy, transportBindingElement.ExtendedProtectionPolicy));
    }

    /// <summary>
    /// Returns a valueindicating that it is not possible to XAML serialize the extended protection policy.
    /// </summary>
    /// 
    /// <returns>
    /// false
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeExtendedProtectionPolicy()
    {
      return !ChannelBindingUtility.AreEqual(this.ExtendedProtectionPolicy, ChannelBindingUtility.DefaultPolicy);
    }

    /// <summary>
    /// Determines whether the message handler factory should be serialized.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message handler factory should be serialized; otherwise, false.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeMessageHandlerFactory()
    {
      return false;
    }

    /// <summary>
    /// Determines whether the web socket settings should be serialized.
    /// </summary>
    /// 
    /// <returns>
    /// true if the web socket settings should be serialized; otherwise, false.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeWebSocketSettings()
    {
      return !this.WebSocketSettings.Equals(HttpTransportDefaults.GetDefaultWebSocketTransportSettings());
    }

    private MessageEncodingBindingElement FindMessageEncodingBindingElement(BindingElementCollection bindingElements, out bool createdNew)
    {
      createdNew = false;
      MessageEncodingBindingElement encodingBindingElement = bindingElements.Find<MessageEncodingBindingElement>();
      if (encodingBindingElement == null)
      {
        createdNew = true;
        encodingBindingElement = (MessageEncodingBindingElement) new TextMessageEncodingBindingElement();
      }
      return encodingBindingElement;
    }

    private MessageEncodingBindingElement FindMessageEncodingBindingElement(WsdlEndpointConversionContext endpointContext, out bool createdNew)
    {
      return this.FindMessageEncodingBindingElement(endpointContext.Endpoint.Binding.CreateBindingElements(), out createdNew);
    }

    private class BindingDeliveryCapabilitiesHelper : IBindingDeliveryCapabilities
    {
      bool IBindingDeliveryCapabilities.AssuresOrderedDelivery
      {
        get
        {
          return false;
        }
      }

      bool IBindingDeliveryCapabilities.QueuedDelivery
      {
        get
        {
          return false;
        }
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal BindingDeliveryCapabilitiesHelper()
      {
      }
    }

    private class TransportCompressionSupportHelper : ITransportCompressionSupport
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public TransportCompressionSupportHelper()
      {
      }

      public bool IsCompressionFormatSupported(CompressionFormat compressionFormat)
      {
        return true;
      }
    }
  }
}
