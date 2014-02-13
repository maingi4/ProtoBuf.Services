// Type: System.ServiceModel.Channels.HttpChannelFactory`1
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Diagnostics;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Diagnostics.Application;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading;

namespace System.ServiceModel.Channels
{
  internal class HttpChannelFactory<TChannel> : TransportChannelFactory<TChannel>, IHttpTransportFactorySettings, ITransportFactorySettings, IDefaultCommunicationTimeouts
  {
    private static bool httpWebRequestWebPermissionDenied = false;
    private static RequestCachePolicy requestCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
    private readonly ClientWebSocketFactory clientWebSocketFactory;
    private bool allowCookies;
    private AuthenticationSchemes authenticationScheme;
    private HttpCookieContainerManager httpCookieContainerManager;
    private volatile MruCache<Uri, Uri> credentialCacheUriPrefixCache;
    private bool decompressionEnabled;
    [SecurityCritical]
    private volatile MruCache<string, string> credentialHashCache;
    [SecurityCritical]
    private HashAlgorithm hashAlgorithm;
    private bool keepAliveEnabled;
    private int maxBufferSize;
    private IWebProxy proxy;
    private HttpChannelFactory<TChannel>.WebProxyFactory proxyFactory;
    private SecurityCredentialsManager channelCredentials;
    private SecurityTokenManager securityTokenManager;
    private TransferMode transferMode;
    private ISecurityCapabilities securityCapabilities;
    private WebSocketTransportSettings webSocketSettings;
    private ConnectionBufferPool bufferPool;
    private Lazy<string> webSocketSoapContentType;

    public bool AllowCookies
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.allowCookies;
      }
    }

    public AuthenticationSchemes AuthenticationScheme
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.authenticationScheme;
      }
    }

    public bool DecompressionEnabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.decompressionEnabled;
      }
    }

    public virtual bool IsChannelBindingSupportEnabled
    {
      get
      {
        return false;
      }
    }

    public bool KeepAliveEnabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.keepAliveEnabled;
      }
    }

    public SecurityTokenManager SecurityTokenManager
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.securityTokenManager;
      }
    }

    public int MaxBufferSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxBufferSize;
      }
    }

    public IWebProxy Proxy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.proxy;
      }
    }

    public TransferMode TransferMode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.transferMode;
      }
    }

    public override string Scheme
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return Uri.UriSchemeHttp;
      }
    }

    public WebSocketTransportSettings WebSocketSettings
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.webSocketSettings;
      }
    }

    internal string WebSocketSoapContentType
    {
      get
      {
        return this.webSocketSoapContentType.Value;
      }
    }

    protected ConnectionBufferPool WebSocketBufferPool
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.bufferPool;
      }
    }

    HashAlgorithm HashAlgorithm
    {
      [SecurityCritical] private get
      {
        if (this.hashAlgorithm == null)
          this.hashAlgorithm = CryptoHelper.CreateHashAlgorithm("http://www.w3.org/2000/09/xmldsig#sha1");
        else
          this.hashAlgorithm.Initialize();
        return this.hashAlgorithm;
      }
    }

    int IHttpTransportFactorySettings.MaxBufferSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.MaxBufferSize;
      }
    }

    TransferMode IHttpTransportFactorySettings.TransferMode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.TransferMode;
      }
    }

    protected ClientWebSocketFactory ClientWebSocketFactory
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.clientWebSocketFactory;
      }
    }

    static HttpChannelFactory()
    {
    }

    internal HttpChannelFactory(HttpTransportBindingElement bindingElement, BindingContext context)
      : base((TransportBindingElement) bindingElement, context, HttpTransportDefaults.GetDefaultMessageEncoderFactory())
    {
      if (bindingElement.TransferMode == TransferMode.Buffered)
      {
        if (bindingElement.MaxReceivedMessageSize > (long) int.MaxValue)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("bindingElement.MaxReceivedMessageSize", System.ServiceModel.SR.GetString("MaxReceivedMessageSizeMustBeInIntegerRange")));
        if ((long) bindingElement.MaxBufferSize != bindingElement.MaxReceivedMessageSize)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("bindingElement", System.ServiceModel.SR.GetString("MaxBufferSizeMustMatchMaxReceivedMessageSize"));
      }
      else if ((long) bindingElement.MaxBufferSize > bindingElement.MaxReceivedMessageSize)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("bindingElement", System.ServiceModel.SR.GetString("MaxBufferSizeMustNotExceedMaxReceivedMessageSize"));
      if (TransferModeHelper.IsRequestStreamed(bindingElement.TransferMode) && bindingElement.AuthenticationScheme != AuthenticationSchemes.Anonymous)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("bindingElement", System.ServiceModel.SR.GetString("HttpAuthDoesNotSupportRequestStreaming"));
      this.allowCookies = bindingElement.AllowCookies;
      if (!this.allowCookies)
      {
        Collection<HttpCookieContainerBindingElement> all = context.BindingParameters.FindAll<HttpCookieContainerBindingElement>();
        if (all.Count > 1)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MultipleCCbesInParameters", new object[1]
          {
            (object) typeof (HttpCookieContainerBindingElement)
          })));
        else if (all.Count == 1)
        {
          this.allowCookies = true;
          context.BindingParameters.Remove<HttpCookieContainerBindingElement>();
        }
      }
      if (this.allowCookies)
        this.httpCookieContainerManager = new HttpCookieContainerManager();
      if (!AuthenticationSchemesHelper.IsSingleton(bindingElement.AuthenticationScheme))
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("value", System.ServiceModel.SR.GetString("HttpRequiresSingleAuthScheme", new object[1]
        {
          (object) bindingElement.AuthenticationScheme
        }));
      }
      else
      {
        this.authenticationScheme = bindingElement.AuthenticationScheme;
        this.decompressionEnabled = bindingElement.DecompressionEnabled;
        this.keepAliveEnabled = bindingElement.KeepAliveEnabled;
        this.maxBufferSize = bindingElement.MaxBufferSize;
        this.transferMode = bindingElement.TransferMode;
        if (bindingElement.Proxy != null)
          this.proxy = bindingElement.Proxy;
        else if (bindingElement.ProxyAddress != (Uri) null)
        {
          if (bindingElement.UseDefaultWebProxy)
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("UseDefaultWebProxyCantBeUsedWithExplicitProxyAddress")));
          if (bindingElement.ProxyAuthenticationScheme == AuthenticationSchemes.Anonymous)
          {
            this.proxy = (IWebProxy) new WebProxy(bindingElement.ProxyAddress, bindingElement.BypassProxyOnLocal);
          }
          else
          {
            this.proxy = (IWebProxy) null;
            this.proxyFactory = new HttpChannelFactory<TChannel>.WebProxyFactory(bindingElement.ProxyAddress, bindingElement.BypassProxyOnLocal, bindingElement.ProxyAuthenticationScheme);
          }
        }
        else if (!bindingElement.UseDefaultWebProxy)
          this.proxy = (IWebProxy) new WebProxy();
        this.channelCredentials = context.BindingParameters.Find<SecurityCredentialsManager>();
        this.securityCapabilities = bindingElement.GetProperty<ISecurityCapabilities>(context);
        this.webSocketSettings = WebSocketHelper.GetRuntimeWebSocketSettings(bindingElement.WebSocketSettings);
        this.bufferPool = new ConnectionBufferPool(WebSocketHelper.ComputeClientBufferSize(this.MaxReceivedMessageSize));
        Collection<ClientWebSocketFactory> all = context.BindingParameters.FindAll<ClientWebSocketFactory>();
        if (all.Count > 1)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("context", System.ServiceModel.SR.GetString("MultipleClientWebSocketFactoriesSpecified", (object) typeof (BindingContext).Name, (object) typeof (ClientWebSocketFactory).Name));
        }
        else
        {
          this.clientWebSocketFactory = all.Count == 0 ? (ClientWebSocketFactory) null : all[0];
          this.webSocketSoapContentType = new Lazy<string>((Func<string>) (() => this.MessageEncoderFactory.CreateSessionEncoder().ContentType), LazyThreadSafetyMode.ExecutionAndPublication);
        }
      }
    }

    public override T GetProperty<T>()
    {
      if (typeof (T) == typeof (ISecurityCapabilities))
        return (T) this.securityCapabilities;
      if (typeof (T) == typeof (IHttpCookieContainerManager))
        return (T) this.GetHttpCookieContainerManager();
      else
        return base.GetProperty<T>();
    }

    [SecuritySafeCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
    private HttpCookieContainerManager GetHttpCookieContainerManager()
    {
      return this.httpCookieContainerManager;
    }

    internal virtual SecurityMessageProperty CreateReplySecurityProperty(HttpWebRequest request, HttpWebResponse response)
    {
      if (!response.IsMutuallyAuthenticated)
        return (SecurityMessageProperty) null;
      else
        return this.CreateMutuallyAuthenticatedReplySecurityProperty(response);
    }

    internal Exception CreateToMustEqualViaException(Uri to, Uri via)
    {
      return (Exception) new ArgumentException(System.ServiceModel.SR.GetString("HttpToMustEqualVia", (object) to, (object) via));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private SecurityMessageProperty CreateMutuallyAuthenticatedReplySecurityProperty(HttpWebResponse response)
    {
      string principalName = AuthenticationManager.CustomTargetNameDictionary[response.ResponseUri.AbsoluteUri];
      if (principalName == null)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageSecurityException(System.ServiceModel.SR.GetString("HttpSpnNotFound", new object[1]
        {
          (object) response.ResponseUri
        })));
      }
      else
      {
        ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies = System.ServiceModel.Security.SecurityUtils.CreatePrincipalNameAuthorizationPolicies(principalName);
        return new SecurityMessageProperty()
        {
          TransportToken = new SecurityTokenSpecification((SecurityToken) null, authorizationPolicies),
          ServiceSecurityContext = new ServiceSecurityContext(authorizationPolicies)
        };
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal override int GetMaxBufferSize()
    {
      return this.MaxBufferSize;
    }

    private SecurityTokenProviderContainer CreateAndOpenTokenProvider(TimeSpan timeout, AuthenticationSchemes authenticationScheme, EndpointAddress target, Uri via, ChannelParameterCollection channelParameters)
    {
      SecurityTokenProvider tokenProvider = (SecurityTokenProvider) null;
      switch (authenticationScheme)
      {
        case AuthenticationSchemes.Digest:
          tokenProvider = TransportSecurityHelpers.GetDigestTokenProvider(this.SecurityTokenManager, target, via, this.Scheme, authenticationScheme, channelParameters);
          goto case AuthenticationSchemes.Anonymous;
        case AuthenticationSchemes.Negotiate:
        case AuthenticationSchemes.Ntlm:
          tokenProvider = (SecurityTokenProvider) TransportSecurityHelpers.GetSspiTokenProvider(this.SecurityTokenManager, target, via, this.Scheme, authenticationScheme, channelParameters);
          goto case AuthenticationSchemes.Anonymous;
        case AuthenticationSchemes.Basic:
          tokenProvider = TransportSecurityHelpers.GetUserNameTokenProvider(this.SecurityTokenManager, target, via, this.Scheme, authenticationScheme, channelParameters);
          goto case AuthenticationSchemes.Anonymous;
        case AuthenticationSchemes.Anonymous:
          SecurityTokenProviderContainer providerContainer;
          if (tokenProvider != null)
          {
            providerContainer = new SecurityTokenProviderContainer(tokenProvider);
            providerContainer.Open(timeout);
          }
          else
            providerContainer = (SecurityTokenProviderContainer) null;
          return providerContainer;
        default:
          throw Fx.AssertAndThrow("CreateAndOpenTokenProvider: Invalid authentication scheme");
      }
    }

    protected virtual void ValidateCreateChannelParameters(EndpointAddress remoteAddress, Uri via)
    {
      this.ValidateScheme(via);
      if (this.MessageVersion.Addressing == AddressingVersion.None && remoteAddress.Uri != via)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateToMustEqualViaException(remoteAddress.Uri, via));
    }

    protected override TChannel OnCreateChannel(EndpointAddress remoteAddress, Uri via)
    {
      return this.OnCreateChannelCore(!(remoteAddress != (EndpointAddress) null) || !WebSocketHelper.IsWebSocketUri(remoteAddress.Uri) ? remoteAddress : new EndpointAddress(WebSocketHelper.NormalizeWsSchemeWithHttpScheme(remoteAddress.Uri), remoteAddress), WebSocketHelper.IsWebSocketUri(via) ? WebSocketHelper.NormalizeWsSchemeWithHttpScheme(via) : via);
    }

    protected virtual TChannel OnCreateChannelCore(EndpointAddress remoteAddress, Uri via)
    {
      this.ValidateCreateChannelParameters(remoteAddress, via);
      this.ValidateWebSocketTransportUsage();
      if (typeof (TChannel) == typeof (IRequestChannel))
        return (TChannel) new HttpChannelFactory<TChannel>.HttpRequestChannel((HttpChannelFactory<IRequestChannel>) this, remoteAddress, via, this.ManualAddressing);
      else
        return (TChannel) new ClientWebSocketTransportDuplexSessionChannel((HttpChannelFactory<IDuplexSessionChannel>) this, this.clientWebSocketFactory, remoteAddress, via, this.WebSocketBufferPool);
    }

    protected void ValidateWebSocketTransportUsage()
    {
      System.Type type = typeof (TChannel);
      if (type == typeof (IRequestChannel) && this.WebSocketSettings.TransportUsage == WebSocketTransportUsage.Always)
      {
        throw FxTrace.Exception.AsError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("WebSocketCannotCreateRequestClientChannelWithCertainWebSocketTransportUsage", (object) typeof (TChannel), (object) "TransportUsage", (object) typeof (WebSocketTransportSettings).Name, (object) this.WebSocketSettings.TransportUsage)));
      }
      else
      {
        if (!(type == typeof (IDuplexSessionChannel)))
          return;
        if (this.WebSocketSettings.TransportUsage == WebSocketTransportUsage.Never)
        {
          throw FxTrace.Exception.AsError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("WebSocketCannotCreateRequestClientChannelWithCertainWebSocketTransportUsage", (object) typeof (TChannel), (object) "TransportUsage", (object) typeof (WebSocketTransportSettings).Name, (object) this.WebSocketSettings.TransportUsage)));
        }
        else
        {
          if (WebSocketHelper.OSSupportsWebSockets() || this.ClientWebSocketFactory != null)
            return;
          throw FxTrace.Exception.AsError((Exception) new PlatformNotSupportedException(System.ServiceModel.SR.GetString("WebSocketsClientSideNotSupported", new object[1]
          {
            (object) typeof (ClientWebSocketFactory).FullName
          })));
        }
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void InitializeSecurityTokenManager()
    {
      if (this.channelCredentials == null)
        this.channelCredentials = (SecurityCredentialsManager) ClientCredentials.CreateDefaultCredentials();
      this.securityTokenManager = this.channelCredentials.CreateSecurityTokenManager();
    }

    protected virtual bool IsSecurityTokenManagerRequired()
    {
      return this.AuthenticationScheme != AuthenticationSchemes.Anonymous || this.proxyFactory != null && this.proxyFactory.AuthenticationScheme != AuthenticationSchemes.Anonymous;
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.OnOpen(timeout);
      return (IAsyncResult) new CompletedAsyncResult(callback, state);
    }

    protected override void OnEndOpen(IAsyncResult result)
    {
      CompletedAsyncResult.End(result);
    }

    protected override void OnOpen(TimeSpan timeout)
    {
      if (this.IsSecurityTokenManagerRequired())
        this.InitializeSecurityTokenManager();
      if (this.AllowCookies && !this.httpCookieContainerManager.IsInitialized)
        this.httpCookieContainerManager.CookieContainer = new CookieContainer();
      if (HttpChannelFactory<TChannel>.httpWebRequestWebPermissionDenied || HttpWebRequest.DefaultMaximumErrorResponseLength == -1)
        return;
      int num;
      if (this.MaxBufferSize >= 2147482623)
      {
        num = -1;
      }
      else
      {
        num = this.MaxBufferSize / 1024;
        if (num * 1024 < this.MaxBufferSize)
          ++num;
      }
      if (num != -1)
      {
        if (num <= HttpWebRequest.DefaultMaximumErrorResponseLength)
          return;
      }
      try
      {
        HttpWebRequest.DefaultMaximumErrorResponseLength = num;
      }
      catch (SecurityException ex)
      {
        HttpChannelFactory<TChannel>.httpWebRequestWebPermissionDenied = true;
        DiagnosticUtility.TraceHandledException((Exception) ex, TraceEventType.Warning);
      }
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      if (this.bufferPool == null)
        return;
      this.bufferPool.Close();
    }

    internal static void TraceResponseReceived(HttpWebResponse response, Message message, object receiver)
    {
      if (!DiagnosticUtility.ShouldTraceVerbose)
        return;
      if (response != null && response.ResponseUri != (Uri) null)
        TraceUtility.TraceEvent(TraceEventType.Verbose, 262153, System.ServiceModel.SR.GetString("TraceCodeHttpResponseReceived"), (TraceRecord) new StringTraceRecord("ResponseUri", response.ResponseUri.ToString()), receiver, (Exception) null, message);
      else
        TraceUtility.TraceEvent(TraceEventType.Verbose, 262153, System.ServiceModel.SR.GetString("TraceCodeHttpResponseReceived"), receiver, message);
    }

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private string AppendWindowsAuthenticationInfo(string inputString, NetworkCredential credential, AuthenticationLevel authenticationLevel, TokenImpersonationLevel impersonationLevel)
    {
      return System.ServiceModel.Security.SecurityUtils.AppendWindowsAuthenticationInfo(inputString, credential, authenticationLevel, impersonationLevel);
    }

    protected virtual string OnGetConnectionGroupPrefix(HttpWebRequest httpWebRequest, SecurityTokenContainer clientCertificateToken)
    {
      return string.Empty;
    }

    internal static bool IsWindowsAuth(AuthenticationSchemes authScheme)
    {
      if (authScheme != AuthenticationSchemes.Negotiate)
        return authScheme == AuthenticationSchemes.Ntlm;
      else
        return true;
    }

    [SecuritySafeCritical]
    private string GetConnectionGroupName(HttpWebRequest httpWebRequest, NetworkCredential credential, AuthenticationLevel authenticationLevel, TokenImpersonationLevel impersonationLevel, SecurityTokenContainer clientCertificateToken)
    {
      if (this.credentialHashCache == null)
      {
        lock (this.ThisLock)
        {
          if (this.credentialHashCache == null)
            this.credentialHashCache = new MruCache<string, string>(5);
        }
      }
      string inputString = TransferModeHelper.IsRequestStreamed(this.TransferMode) ? "streamed" : string.Empty;
      if (HttpChannelFactory<TChannel>.IsWindowsAuth(this.AuthenticationScheme))
      {
        if (!HttpChannelFactory<TChannel>.httpWebRequestWebPermissionDenied)
        {
          try
          {
            httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
          }
          catch (SecurityException ex)
          {
            DiagnosticUtility.TraceHandledException((Exception) ex, TraceEventType.Information);
            HttpChannelFactory<TChannel>.httpWebRequestWebPermissionDenied = true;
          }
        }
        inputString = this.AppendWindowsAuthenticationInfo(inputString, credential, authenticationLevel, impersonationLevel);
      }
      string str1 = this.OnGetConnectionGroupPrefix(httpWebRequest, clientCertificateToken) + inputString;
      string str2 = (string) null;
      if (!string.IsNullOrEmpty(str1))
      {
        lock (this.credentialHashCache)
        {
          if (!this.credentialHashCache.TryGetValue(str1, out str2))
          {
            str2 = Convert.ToBase64String(this.HashAlgorithm.ComputeHash(new UTF8Encoding().GetBytes(str1)));
            this.credentialHashCache.Add(str1, str2);
          }
        }
      }
      return str2;
    }

    private Uri GetCredentialCacheUriPrefix(Uri via)
    {
      if (this.credentialCacheUriPrefixCache == null)
      {
        lock (this.ThisLock)
        {
          if (this.credentialCacheUriPrefixCache == null)
            this.credentialCacheUriPrefixCache = new MruCache<Uri, Uri>(10);
        }
      }
      Uri uri;
      lock (this.credentialCacheUriPrefixCache)
      {
        if (!this.credentialCacheUriPrefixCache.TryGetValue(via, out uri))
        {
          uri = new UriBuilder(via.Scheme, via.Host, via.Port).Uri;
          this.credentialCacheUriPrefixCache.Add(via, uri);
        }
      }
      return uri;
    }

    private HttpWebRequest GetWebRequest(EndpointAddress to, Uri via, NetworkCredential credential, TokenImpersonationLevel impersonationLevel, AuthenticationLevel authenticationLevel, SecurityTokenProviderContainer proxyTokenProvider, SecurityTokenContainer clientCertificateToken, TimeSpan timeout, bool isWebSocketRequest)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(isWebSocketRequest ? WebSocketHelper.GetWebSocketUri(via) : via);
      if (!isWebSocketRequest)
      {
        httpWebRequest.Method = "POST";
        if (TransferModeHelper.IsRequestStreamed(this.TransferMode))
        {
          httpWebRequest.SendChunked = true;
          httpWebRequest.AllowWriteStreamBuffering = false;
        }
        else
          httpWebRequest.AllowWriteStreamBuffering = true;
      }
      httpWebRequest.CachePolicy = HttpChannelFactory<TChannel>.requestCachePolicy;
      httpWebRequest.KeepAlive = this.keepAliveEnabled;
      httpWebRequest.AutomaticDecompression = !this.decompressionEnabled ? DecompressionMethods.None : DecompressionMethods.GZip | DecompressionMethods.Deflate;
      if (credential != null)
        httpWebRequest.Credentials = (ICredentials) new CredentialCache()
        {
          {
            this.GetCredentialCacheUriPrefix(via),
            AuthenticationSchemesHelper.ToString(this.authenticationScheme),
            credential
          }
        };
      httpWebRequest.AuthenticationLevel = authenticationLevel;
      httpWebRequest.ImpersonationLevel = impersonationLevel;
      string str = this.GetConnectionGroupName(httpWebRequest, credential, authenticationLevel, impersonationLevel, clientCertificateToken);
      X509CertificateEndpointIdentity endpointIdentity = to.Identity as X509CertificateEndpointIdentity;
      if (endpointIdentity != null)
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}[{1}]", new object[2]
        {
          (object) str,
          (object) endpointIdentity.Certificates[0].Thumbprint
        });
      if (!string.IsNullOrEmpty(str))
        httpWebRequest.ConnectionGroupName = str;
      if (this.AuthenticationScheme == AuthenticationSchemes.Basic)
        httpWebRequest.PreAuthenticate = true;
      if (this.proxy != null)
        httpWebRequest.Proxy = this.proxy;
      else if (this.proxyFactory != null)
        httpWebRequest.Proxy = this.proxyFactory.CreateWebProxy(httpWebRequest, proxyTokenProvider, timeout);
      if (this.AllowCookies)
        httpWebRequest.CookieContainer = this.httpCookieContainerManager.CookieContainer;
      httpWebRequest.ServicePoint.UseNagleAlgorithm = false;
      return httpWebRequest;
    }

    private void ApplyManualAddressing(ref EndpointAddress to, ref Uri via, Message message)
    {
      if (this.ManualAddressing)
      {
        Uri to1 = message.Headers.To;
        if (to1 == (Uri) null)
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("ManualAddressingRequiresAddressedMessages")), message);
        to = new EndpointAddress(to1, new AddressHeader[0]);
        if (this.MessageVersion.Addressing == AddressingVersion.None)
          via = to1;
      }
      object obj;
      if (!message.Properties.TryGetValue(HttpRequestMessageProperty.Name, out obj))
        return;
      HttpRequestMessageProperty requestMessageProperty = (HttpRequestMessageProperty) obj;
      if (string.IsNullOrEmpty(requestMessageProperty.QueryString))
        return;
      via = new UriBuilder(via)
      {
        Query = (!requestMessageProperty.QueryString.StartsWith("?", StringComparison.Ordinal) ? requestMessageProperty.QueryString : requestMessageProperty.QueryString.Substring(1))
      }.Uri;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void CreateAndOpenTokenProvidersCore(EndpointAddress to, Uri via, ChannelParameterCollection channelParameters, TimeSpan timeout, out SecurityTokenProviderContainer tokenProvider, out SecurityTokenProviderContainer proxyTokenProvider)
    {
      System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
      tokenProvider = this.CreateAndOpenTokenProvider(timeoutHelper.RemainingTime(), this.AuthenticationScheme, to, via, channelParameters);
      if (this.proxyFactory != null)
        proxyTokenProvider = this.CreateAndOpenTokenProvider(timeoutHelper.RemainingTime(), this.proxyFactory.AuthenticationScheme, to, via, channelParameters);
      else
        proxyTokenProvider = (SecurityTokenProviderContainer) null;
    }

    internal void CreateAndOpenTokenProviders(EndpointAddress to, Uri via, ChannelParameterCollection channelParameters, TimeSpan timeout, out SecurityTokenProviderContainer tokenProvider, out SecurityTokenProviderContainer proxyTokenProvider)
    {
      if (!this.IsSecurityTokenManagerRequired())
      {
        tokenProvider = (SecurityTokenProviderContainer) null;
        proxyTokenProvider = (SecurityTokenProviderContainer) null;
      }
      else
        this.CreateAndOpenTokenProvidersCore(to, via, channelParameters, timeout, out tokenProvider, out proxyTokenProvider);
    }

    internal HttpWebRequest GetWebRequest(EndpointAddress to, Uri via, SecurityTokenProviderContainer tokenProvider, SecurityTokenProviderContainer proxyTokenProvider, SecurityTokenContainer clientCertificateToken, TimeSpan timeout, bool isWebSocketRequest)
    {
      System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
      TokenImpersonationLevel impersonationLevel;
      AuthenticationLevel authenticationLevel;
      NetworkCredential credential = HttpChannelUtilities.GetCredential(this.authenticationScheme, tokenProvider, timeoutHelper.RemainingTime(), out impersonationLevel, out authenticationLevel);
      return this.GetWebRequest(to, via, credential, impersonationLevel, authenticationLevel, proxyTokenProvider, clientCertificateToken, timeoutHelper.RemainingTime(), isWebSocketRequest);
    }

    internal static bool MapIdentity(EndpointAddress target, AuthenticationSchemes authenticationScheme)
    {
      if (target.Identity == null || target.Identity is X509CertificateEndpointIdentity)
        return false;
      else
        return HttpChannelFactory<TChannel>.IsWindowsAuth(authenticationScheme);
    }

    private bool MapIdentity(EndpointAddress target)
    {
      return HttpChannelFactory<TChannel>.MapIdentity(target, this.AuthenticationScheme);
    }

    protected class HttpRequestChannel : RequestChannel
    {
      private volatile bool cleanupIdentity;
      private HttpChannelFactory<IRequestChannel> factory;
      private SecurityTokenProviderContainer tokenProvider;
      private SecurityTokenProviderContainer proxyTokenProvider;
      private ServiceModelActivity activity;
      private ChannelParameterCollection channelParameters;

      public HttpChannelFactory<IRequestChannel> Factory
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.factory;
        }
      }

      internal ServiceModelActivity Activity
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.activity;
        }
      }

      protected ChannelParameterCollection ChannelParameters
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.channelParameters;
        }
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public HttpRequestChannel(HttpChannelFactory<IRequestChannel> factory, EndpointAddress to, Uri via, bool manualAddressing)
        : base((ChannelManagerBase) factory, to, via, manualAddressing)
      {
        this.factory = factory;
      }

      public override T GetProperty<T>()
      {
        if (!(typeof (T) == typeof (ChannelParameterCollection)))
          return base.GetProperty<T>();
        if (this.State == CommunicationState.Created)
        {
          lock (this.ThisLock)
          {
            if (this.channelParameters == null)
              this.channelParameters = new ChannelParameterCollection();
          }
        }
        return (T) this.channelParameters;
      }

      private void PrepareOpen()
      {
        if (!this.Factory.MapIdentity(this.RemoteAddress))
          return;
        lock (this.ThisLock)
          this.cleanupIdentity = HttpTransportSecurityHelpers.AddIdentityMapping(this.Via, this.RemoteAddress);
      }

      private void CreateAndOpenTokenProviders(TimeSpan timeout)
      {
        System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
        if (this.ManualAddressing)
          return;
        this.Factory.CreateAndOpenTokenProviders(this.RemoteAddress, this.Via, this.channelParameters, timeoutHelper.RemainingTime(), out this.tokenProvider, out this.proxyTokenProvider);
      }

      private void CloseTokenProviders(TimeSpan timeout)
      {
        System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
        if (this.tokenProvider != null)
          this.tokenProvider.Close(timeoutHelper.RemainingTime());
        if (this.proxyTokenProvider == null)
          return;
        this.proxyTokenProvider.Close(timeoutHelper.RemainingTime());
      }

      private void AbortTokenProviders()
      {
        if (this.tokenProvider != null)
          this.tokenProvider.Abort();
        if (this.proxyTokenProvider == null)
          return;
        this.proxyTokenProvider.Abort();
      }

      protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
      {
        this.PrepareOpen();
        this.CreateAndOpenTokenProviders(new System.Runtime.TimeoutHelper(timeout).RemainingTime());
        return (IAsyncResult) new CompletedAsyncResult(callback, state);
      }

      protected override void OnOpen(TimeSpan timeout)
      {
        this.PrepareOpen();
        this.CreateAndOpenTokenProviders(timeout);
      }

      protected override void OnEndOpen(IAsyncResult result)
      {
        CompletedAsyncResult.End(result);
      }

      private void PrepareClose(bool aborting)
      {
        if (!this.cleanupIdentity)
          return;
        lock (this.ThisLock)
        {
          if (!this.cleanupIdentity)
            return;
          this.cleanupIdentity = false;
          HttpTransportSecurityHelpers.RemoveIdentityMapping(this.Via, this.RemoteAddress, !aborting);
        }
      }

      protected override void OnAbort()
      {
        this.PrepareClose(true);
        this.AbortTokenProviders();
        base.OnAbort();
      }

      protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
      {
        IAsyncResult asyncResult = (IAsyncResult) null;
        using (ServiceModelActivity.BoundOperation(this.activity))
        {
          this.PrepareClose(false);
          System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
          this.CloseTokenProviders(timeoutHelper.RemainingTime());
          asyncResult = this.BeginWaitForPendingRequests(timeoutHelper.RemainingTime(), callback, state);
        }
        ServiceModelActivity.Stop(this.activity);
        return asyncResult;
      }

      protected override void OnEndClose(IAsyncResult result)
      {
        using (ServiceModelActivity.BoundOperation(this.activity))
          this.EndWaitForPendingRequests(result);
        ServiceModelActivity.Stop(this.activity);
      }

      protected override void OnClose(TimeSpan timeout)
      {
        using (ServiceModelActivity.BoundOperation(this.activity))
        {
          this.PrepareClose(false);
          System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
          this.CloseTokenProviders(timeoutHelper.RemainingTime());
          this.WaitForPendingRequests(timeoutHelper.RemainingTime());
        }
        ServiceModelActivity.Stop(this.activity);
      }

      protected override IAsyncRequest CreateAsyncRequest(Message message, AsyncCallback callback, object state)
      {
        if (DiagnosticUtility.ShouldUseActivity && this.activity == null)
        {
          this.activity = ServiceModelActivity.CreateActivity();
          if (FxTrace.Trace != null)
            FxTrace.Trace.TraceTransfer(this.activity.Id);
          ServiceModelActivity.Start(this.activity, System.ServiceModel.SR.GetString("ActivityReceiveBytes", new object[1]
          {
            (object) this.RemoteAddress.Uri.ToString()
          }), ActivityType.ReceiveBytes);
        }
        return (IAsyncRequest) new HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest(this, callback, state);
      }

      protected override IRequest CreateRequest(Message message)
      {
        return (IRequest) new HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelRequest(this, this.Factory);
      }

      public virtual HttpWebRequest GetWebRequest(EndpointAddress to, Uri via, ref System.Runtime.TimeoutHelper timeoutHelper)
      {
        return this.GetWebRequest(to, via, (SecurityTokenContainer) null, ref timeoutHelper);
      }

      protected HttpWebRequest GetWebRequest(EndpointAddress to, Uri via, SecurityTokenContainer clientCertificateToken, ref System.Runtime.TimeoutHelper timeoutHelper)
      {
        SecurityTokenProviderContainer tokenProvider;
        SecurityTokenProviderContainer proxyTokenProvider;
        if (this.ManualAddressing)
        {
          this.Factory.CreateAndOpenTokenProviders(to, via, this.channelParameters, timeoutHelper.RemainingTime(), out tokenProvider, out proxyTokenProvider);
        }
        else
        {
          tokenProvider = this.tokenProvider;
          proxyTokenProvider = this.proxyTokenProvider;
        }
        try
        {
          return this.Factory.GetWebRequest(to, via, tokenProvider, proxyTokenProvider, clientCertificateToken, timeoutHelper.RemainingTime(), false);
        }
        finally
        {
          if (this.ManualAddressing)
          {
            if (tokenProvider != null)
              tokenProvider.Abort();
            if (proxyTokenProvider != null)
              proxyTokenProvider.Abort();
          }
        }
      }

      protected IAsyncResult BeginGetWebRequest(EndpointAddress to, Uri via, SecurityTokenContainer clientCertificateToken, ref System.Runtime.TimeoutHelper timeoutHelper, AsyncCallback callback, object state)
      {
        return (IAsyncResult) new HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult(this, to, via, clientCertificateToken, ref timeoutHelper, callback, state);
      }

      public virtual IAsyncResult BeginGetWebRequest(EndpointAddress to, Uri via, ref System.Runtime.TimeoutHelper timeoutHelper, AsyncCallback callback, object state)
      {
        return this.BeginGetWebRequest(to, via, (SecurityTokenContainer) null, ref timeoutHelper, callback, state);
      }

      public virtual HttpWebRequest EndGetWebRequest(IAsyncResult result)
      {
        return HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.End(result);
      }

      public virtual bool WillGetWebRequestCompleteSynchronously()
      {
        if (this.tokenProvider == null)
          return !this.Factory.ManualAddressing;
        else
          return false;
      }

      internal virtual void OnWebRequestCompleted(HttpWebRequest request)
      {
      }

      private class HttpChannelRequest : IRequest, IRequestBase
      {
        private HttpChannelFactory<TChannel>.HttpRequestChannel channel;
        private HttpChannelFactory<IRequestChannel> factory;
        private EndpointAddress to;
        private Uri via;
        private HttpWebRequest webRequest;
        private HttpAbortReason abortReason;
        private ChannelBinding channelBinding;
        private int webRequestCompleted;
        private EventTraceActivity eventTraceActivity;

        public HttpChannelRequest(HttpChannelFactory<TChannel>.HttpRequestChannel channel, HttpChannelFactory<IRequestChannel> factory)
        {
          this.channel = channel;
          this.to = channel.RemoteAddress;
          this.via = channel.Via;
          this.factory = factory;
        }

        public void SendRequest(Message message, TimeSpan timeout)
        {
          System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
          this.factory.ApplyManualAddressing(ref this.to, ref this.via, message);
          this.webRequest = this.channel.GetWebRequest(this.to, this.via, ref timeoutHelper);
          Message message1 = message;
          try
          {
            if (this.channel.State != CommunicationState.Opened)
            {
              this.Cleanup();
              this.channel.ThrowIfDisposedOrNotOpen();
            }
            HttpChannelUtilities.SetRequestTimeout(this.webRequest, timeoutHelper.RemainingTime());
            HttpOutput httpOutput = HttpOutput.CreateHttpOutput(this.webRequest, (IHttpTransportFactorySettings) this.factory, message1, this.factory.IsChannelBindingSupportEnabled);
            bool flag = false;
            try
            {
              httpOutput.Send(timeoutHelper.RemainingTime());
              this.channelBinding = httpOutput.TakeChannelBinding();
              httpOutput.Close();
              flag = true;
              if (!FxTrace.Trace.IsEnd2EndActivityTracingEnabled)
                return;
              this.eventTraceActivity = EventTraceActivityHelper.TryExtractActivity(message);
              if (!TD.MessageSentByTransportIsEnabled())
                return;
              TD.MessageSentByTransport(this.eventTraceActivity, this.to.Uri.AbsoluteUri);
            }
            finally
            {
              if (!flag)
                httpOutput.Abort(HttpAbortReason.Aborted);
            }
          }
          finally
          {
            if (!object.ReferenceEquals((object) message1, (object) message))
              message1.Close();
          }
        }

        private void Cleanup()
        {
          if (this.webRequest != null)
          {
            HttpChannelUtilities.AbortRequest(this.webRequest);
            this.TryCompleteWebRequest(this.webRequest);
          }
          ChannelBindingUtility.Dispose(ref this.channelBinding);
        }

        public void Abort(RequestChannel channel)
        {
          this.Cleanup();
          this.abortReason = HttpAbortReason.Aborted;
        }

        public void Fault(RequestChannel channel)
        {
          this.Cleanup();
        }

        public Message WaitForReply(TimeSpan timeout)
        {
          if (TD.HttpResponseReceiveStartIsEnabled())
            TD.HttpResponseReceiveStart(this.eventTraceActivity);
          WebException responseException = (WebException) null;
          HttpWebResponse httpWebResponse;
          try
          {
            try
            {
              httpWebResponse = (HttpWebResponse) this.webRequest.GetResponse();
            }
            catch (NullReferenceException ex)
            {
              if (TransferModeHelper.IsRequestStreamed(this.factory.transferMode))
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateNullReferenceResponseException(ex));
              throw;
            }
            if (TD.MessageReceivedByTransportIsEnabled())
              TD.MessageReceivedByTransport(this.eventTraceActivity ?? EventTraceActivity.Empty, httpWebResponse.ResponseUri != (Uri) null ? httpWebResponse.ResponseUri.AbsoluteUri : string.Empty, EventTraceActivity.GetActivityIdFromThread());
            if (DiagnosticUtility.ShouldTraceVerbose)
              HttpChannelFactory<TChannel>.TraceResponseReceived(httpWebResponse, (Message) null, (object) this);
          }
          catch (WebException ex)
          {
            responseException = ex;
            httpWebResponse = HttpChannelUtilities.ProcessGetResponseWebException(ex, this.webRequest, this.abortReason);
          }
          HttpInput httpInput = HttpChannelUtilities.ValidateRequestReplyResponse(this.webRequest, httpWebResponse, this.factory, responseException, this.channelBinding);
          this.channelBinding = (ChannelBinding) null;
          Message message = (Message) null;
          if (httpInput != null)
          {
            Exception requestException = (Exception) null;
            message = httpInput.ParseIncomingMessage(out requestException);
            if (message != null)
            {
              HttpChannelUtilities.AddReplySecurityProperty(this.factory, this.webRequest, httpWebResponse, message);
              if (FxTrace.Trace.IsEnd2EndActivityTracingEnabled && this.eventTraceActivity != null)
                EventTraceActivityHelper.TryAttachActivity(message, this.eventTraceActivity);
            }
          }
          this.TryCompleteWebRequest(this.webRequest);
          return message;
        }

        public void OnReleaseRequest()
        {
          this.TryCompleteWebRequest(this.webRequest);
        }

        private void TryCompleteWebRequest(HttpWebRequest request)
        {
          if (request == null || Interlocked.CompareExchange(ref this.webRequestCompleted, 1, 0) != 0)
            return;
          this.channel.OnWebRequestCompleted(request);
        }
      }

      private class HttpChannelAsyncRequest : TraceAsyncResult, IAsyncRequest, IAsyncResult, IRequestBase
      {
        private static AsyncCallback onProcessIncomingMessage = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.OnParseIncomingMessage));
        private static AsyncCallback onGetResponse = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.OnGetResponse));
        private static AsyncCallback onSend = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.OnSend));
        private object sendLock = new object();
        private static AsyncCallback onGetWebRequestCompleted;
        private static Action<object> onSendTimeout;
        private ChannelBinding channelBinding;
        private HttpChannelFactory<IRequestChannel> factory;
        private HttpChannelFactory<TChannel>.HttpRequestChannel channel;
        private HttpOutput httpOutput;
        private HttpInput httpInput;
        private Message message;
        private Message requestMessage;
        private Message replyMessage;
        private HttpWebResponse response;
        private HttpWebRequest request;
        private IOThreadTimer sendTimer;
        private System.Runtime.TimeoutHelper timeoutHelper;
        private EndpointAddress to;
        private Uri via;
        private HttpAbortReason abortReason;
        private int webRequestCompleted;
        private EventTraceActivity eventTraceActivity;

        IOThreadTimer SendTimer
        {
          private get
          {
            if (this.sendTimer == null)
            {
              if (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onSendTimeout == null)
                HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onSendTimeout = new Action<object>(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.OnSendTimeout);
              this.sendTimer = new IOThreadTimer(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onSendTimeout, (object) this, false);
            }
            return this.sendTimer;
          }
        }

        static HttpChannelAsyncRequest()
        {
        }

        public HttpChannelAsyncRequest(HttpChannelFactory<TChannel>.HttpRequestChannel channel, AsyncCallback callback, object state)
          : base(callback, state)
        {
          this.channel = channel;
          this.to = channel.RemoteAddress;
          this.via = channel.Via;
          this.factory = channel.Factory;
        }

        public static void End(IAsyncResult result)
        {
          AsyncResult.End<HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest>(result);
        }

        public void BeginSendRequest(Message message, TimeSpan timeout)
        {
          this.message = this.requestMessage = message;
          this.timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
          if (FxTrace.Trace.IsEnd2EndActivityTracingEnabled)
            this.eventTraceActivity = EventTraceActivityHelper.TryExtractActivity(message);
          this.factory.ApplyManualAddressing(ref this.to, ref this.via, this.requestMessage);
          if (this.channel.WillGetWebRequestCompleteSynchronously())
          {
            this.SetWebRequest(this.channel.GetWebRequest(this.to, this.via, ref this.timeoutHelper));
            if (!this.SendWebRequest())
              return;
            this.Complete(true);
          }
          else
          {
            if (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onGetWebRequestCompleted == null)
              HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onGetWebRequestCompleted = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.OnGetWebRequestCompletedCallback));
            IAsyncResult webRequest = this.channel.BeginGetWebRequest(this.to, this.via, ref this.timeoutHelper, HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onGetWebRequestCompleted, (object) this);
            if (!webRequest.CompletedSynchronously)
              return;
            if (TD.MessageSentByTransportIsEnabled())
              TD.MessageSentByTransport(this.eventTraceActivity, this.to.Uri.AbsoluteUri);
            if (!this.OnGetWebRequestCompleted(webRequest))
              return;
            this.Complete(true);
          }
        }

        private static void OnGetWebRequestCompletedCallback(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest channelAsyncRequest = (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest) result.AsyncState;
          Exception exception = (Exception) null;
          bool flag;
          try
          {
            flag = channelAsyncRequest.OnGetWebRequestCompleted(result);
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
            {
              throw;
            }
            else
            {
              flag = true;
              exception = ex;
            }
          }
          if (!flag)
            return;
          channelAsyncRequest.Complete(false, exception);
        }

        private void AbortSend()
        {
          this.CancelSendTimer();
          if (this.request == null)
            return;
          this.TryCompleteWebRequest(this.request);
          this.abortReason = HttpAbortReason.TimedOut;
          this.httpOutput.Abort(this.abortReason);
        }

        private void CancelSendTimer()
        {
          lock (this.sendLock)
          {
            if (this.sendTimer == null)
              return;
            this.sendTimer.Cancel();
            this.sendTimer = (IOThreadTimer) null;
          }
        }

        private bool OnGetWebRequestCompleted(IAsyncResult result)
        {
          this.SetWebRequest(this.channel.EndGetWebRequest(result));
          return this.SendWebRequest();
        }

        private bool SendWebRequest()
        {
          this.httpOutput = HttpOutput.CreateHttpOutput(this.request, (IHttpTransportFactorySettings) this.factory, this.requestMessage, this.factory.IsChannelBindingSupportEnabled);
          bool flag1 = false;
          try
          {
            bool flag2 = false;
            this.SetSendTimeout(this.timeoutHelper.RemainingTime());
            IAsyncResult result = this.httpOutput.BeginSend(this.timeoutHelper.RemainingTime(), HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onSend, (object) this);
            flag1 = true;
            if (result.CompletedSynchronously)
              flag2 = this.CompleteSend(result);
            return flag2;
          }
          finally
          {
            if (!flag1)
            {
              this.httpOutput.Abort(HttpAbortReason.Aborted);
              if (!object.ReferenceEquals((object) this.message, (object) this.requestMessage))
                this.requestMessage.Close();
            }
          }
        }

        private bool CompleteSend(IAsyncResult result)
        {
          bool flag = false;
          try
          {
            this.httpOutput.EndSend(result);
            this.channelBinding = this.httpOutput.TakeChannelBinding();
            this.httpOutput.Close();
            flag = true;
            if (TD.MessageSentByTransportIsEnabled())
              TD.MessageSentByTransport(this.eventTraceActivity, this.to.Uri.AbsoluteUri);
          }
          finally
          {
            if (!flag)
              this.httpOutput.Abort(HttpAbortReason.Aborted);
            if (!object.ReferenceEquals((object) this.message, (object) this.requestMessage))
              this.requestMessage.Close();
          }
          try
          {
            IAsyncResult response;
            try
            {
              response = this.request.BeginGetResponse(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onGetResponse, (object) this);
            }
            catch (NullReferenceException ex)
            {
              if (TransferModeHelper.IsRequestStreamed(this.factory.transferMode))
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateNullReferenceResponseException(ex));
              throw;
            }
            if (response.CompletedSynchronously)
              return this.CompleteGetResponse(response);
            else
              return false;
          }
          catch (IOException ex)
          {
            throw TraceUtility.ThrowHelperError((Exception) new CommunicationException(ex.Message, (Exception) ex), this.requestMessage);
          }
          catch (WebException ex)
          {
            throw TraceUtility.ThrowHelperError((Exception) new CommunicationException(ex.Message, (Exception) ex), this.requestMessage);
          }
          catch (ObjectDisposedException ex)
          {
            if (this.abortReason == HttpAbortReason.Aborted)
              throw TraceUtility.ThrowHelperError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.SR.GetString("HttpRequestAborted", new object[1]
              {
                (object) this.to.Uri
              }), (Exception) ex), this.requestMessage);
            else
              throw TraceUtility.ThrowHelperError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("HttpRequestTimedOut", (object) this.to.Uri, (object) this.timeoutHelper.OriginalTimeout), (Exception) ex), this.requestMessage);
          }
        }

        private bool CompleteGetResponse(IAsyncResult result)
        {
          using (ServiceModelActivity.BoundOperation(this.channel.Activity))
          {
            WebException responseException = (WebException) null;
            HttpWebResponse response;
            try
            {
              try
              {
                this.CancelSendTimer();
                response = (HttpWebResponse) this.request.EndGetResponse(result);
              }
              catch (NullReferenceException ex)
              {
                if (TransferModeHelper.IsRequestStreamed(this.factory.transferMode))
                  throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateNullReferenceResponseException(ex));
                throw;
              }
              if (TD.MessageReceivedByTransportIsEnabled())
                TD.MessageReceivedByTransport(this.eventTraceActivity ?? EventTraceActivity.Empty, this.to.Uri.AbsoluteUri, EventTraceActivity.GetActivityIdFromThread());
              if (DiagnosticUtility.ShouldTraceVerbose)
                HttpChannelFactory<TChannel>.TraceResponseReceived(response, this.message, (object) this);
            }
            catch (WebException ex)
            {
              responseException = ex;
              response = HttpChannelUtilities.ProcessGetResponseWebException(ex, this.request, this.abortReason);
            }
            return this.ProcessResponse(response, responseException);
          }
        }

        private void Cleanup()
        {
          if (this.request != null)
          {
            HttpChannelUtilities.AbortRequest(this.request);
            this.TryCompleteWebRequest(this.request);
          }
          ChannelBindingUtility.Dispose(ref this.channelBinding);
        }

        private void SetSendTimeout(TimeSpan timeout)
        {
          HttpChannelUtilities.SetRequestTimeout(this.request, timeout);
          if (timeout == TimeSpan.MaxValue)
            this.CancelSendTimer();
          else
            this.SendTimer.Set(timeout);
        }

        public void Abort(RequestChannel channel)
        {
          this.Cleanup();
          this.abortReason = HttpAbortReason.Aborted;
        }

        public void Fault(RequestChannel channel)
        {
          this.Cleanup();
        }

        private void SetWebRequest(HttpWebRequest webRequest)
        {
          this.request = webRequest;
          if (this.channel.State == CommunicationState.Opened)
            return;
          this.Cleanup();
          this.channel.ThrowIfDisposedOrNotOpen();
        }

        public Message End()
        {
          HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.End((IAsyncResult) this);
          return this.replyMessage;
        }

        private bool ProcessResponse(HttpWebResponse response, WebException responseException)
        {
          this.httpInput = HttpChannelUtilities.ValidateRequestReplyResponse(this.request, response, this.factory, responseException, this.channelBinding);
          this.channelBinding = (ChannelBinding) null;
          if (this.httpInput != null)
          {
            this.response = response;
            IAsyncResult result = this.httpInput.BeginParseIncomingMessage(HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest.onProcessIncomingMessage, (object) this);
            if (!result.CompletedSynchronously)
              return false;
            this.CompleteParseIncomingMessage(result);
          }
          else
            this.replyMessage = (Message) null;
          this.TryCompleteWebRequest(this.request);
          return true;
        }

        private void CompleteParseIncomingMessage(IAsyncResult result)
        {
          Exception requestException = (Exception) null;
          this.replyMessage = this.httpInput.EndParseIncomingMessage(result, out requestException);
          if (this.replyMessage == null)
            return;
          HttpChannelUtilities.AddReplySecurityProperty(this.factory, this.request, this.response, this.replyMessage);
        }

        private static void OnParseIncomingMessage(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest channelAsyncRequest = (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest) result.AsyncState;
          Exception exception = (Exception) null;
          try
          {
            channelAsyncRequest.CompleteParseIncomingMessage(result);
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
              throw;
            else
              exception = ex;
          }
          channelAsyncRequest.Complete(false, exception);
        }

        private static void OnSend(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest channelAsyncRequest = (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest) result.AsyncState;
          Exception exception = (Exception) null;
          bool flag;
          try
          {
            flag = channelAsyncRequest.CompleteSend(result);
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
            {
              throw;
            }
            else
            {
              flag = true;
              exception = ex;
            }
          }
          if (!flag)
            return;
          channelAsyncRequest.Complete(false, exception);
        }

        private static void OnSendTimeout(object state)
        {
          ((HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest) state).AbortSend();
        }

        private static void OnGetResponse(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest channelAsyncRequest = (HttpChannelFactory<TChannel>.HttpRequestChannel.HttpChannelAsyncRequest) result.AsyncState;
          Exception exception = (Exception) null;
          bool flag;
          try
          {
            flag = channelAsyncRequest.CompleteGetResponse(result);
          }
          catch (WebException ex)
          {
            flag = true;
            exception = (Exception) new CommunicationException(ex.Message, (Exception) ex);
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
            {
              throw;
            }
            else
            {
              flag = true;
              exception = ex;
            }
          }
          if (!flag)
            return;
          channelAsyncRequest.Complete(false, exception);
        }

        public void OnReleaseRequest()
        {
          this.TryCompleteWebRequest(this.request);
        }

        private void TryCompleteWebRequest(HttpWebRequest request)
        {
          if (request == null || Interlocked.CompareExchange(ref this.webRequestCompleted, 1, 0) != 0)
            return;
          this.channel.OnWebRequestCompleted(request);
        }
      }

      private class GetWebRequestAsyncResult : AsyncResult
      {
        private static AsyncCallback onGetSspiCredential;
        private static AsyncCallback onGetUserNameCredential;
        private SecurityTokenContainer clientCertificateToken;
        private HttpChannelFactory<IRequestChannel> factory;
        private SecurityTokenProviderContainer proxyTokenProvider;
        private HttpWebRequest request;
        private EndpointAddress to;
        private System.Runtime.TimeoutHelper timeoutHelper;
        private SecurityTokenProviderContainer tokenProvider;
        private Uri via;

        public GetWebRequestAsyncResult(HttpChannelFactory<TChannel>.HttpRequestChannel channel, EndpointAddress to, Uri via, SecurityTokenContainer clientCertificateToken, ref System.Runtime.TimeoutHelper timeoutHelper, AsyncCallback callback, object state)
          : base(callback, state)
        {
          this.to = to;
          this.via = via;
          this.clientCertificateToken = clientCertificateToken;
          this.timeoutHelper = timeoutHelper;
          this.factory = channel.Factory;
          this.tokenProvider = channel.tokenProvider;
          this.proxyTokenProvider = channel.proxyTokenProvider;
          if (this.factory.ManualAddressing)
            this.factory.CreateAndOpenTokenProviders(to, via, channel.channelParameters, timeoutHelper.RemainingTime(), out this.tokenProvider, out this.proxyTokenProvider);
          bool flag = false;
          if (this.factory.AuthenticationScheme == AuthenticationSchemes.Anonymous)
          {
            this.SetupWebRequest(AuthenticationLevel.None, TokenImpersonationLevel.None, (NetworkCredential) null);
            flag = true;
          }
          else if (this.factory.AuthenticationScheme == AuthenticationSchemes.Basic)
          {
            if (HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetUserNameCredential == null)
              HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetUserNameCredential = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.OnGetUserNameCredential));
            IAsyncResult userNameCredential = TransportSecurityHelpers.BeginGetUserNameCredential(this.tokenProvider, timeoutHelper.RemainingTime(), HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetUserNameCredential, (object) this);
            if (userNameCredential.CompletedSynchronously)
            {
              this.CompleteGetUserNameCredential(userNameCredential);
              flag = true;
            }
          }
          else
          {
            if (HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetSspiCredential == null)
              HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetSspiCredential = Fx.ThunkCallback(new AsyncCallback(HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.OnGetSspiCredential));
            IAsyncResult sspiCredential = TransportSecurityHelpers.BeginGetSspiCredential(this.tokenProvider, timeoutHelper.RemainingTime(), HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult.onGetSspiCredential, (object) this);
            if (sspiCredential.CompletedSynchronously)
            {
              this.CompleteGetSspiCredential(sspiCredential);
              flag = true;
            }
          }
          if (!flag)
            return;
          this.CloseTokenProvidersIfRequired();
          this.Complete(true);
        }

        public static HttpWebRequest End(IAsyncResult result)
        {
          return AsyncResult.End<HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult>(result).request;
        }

        private void CompleteGetUserNameCredential(IAsyncResult result)
        {
          this.SetupWebRequest(AuthenticationLevel.None, TokenImpersonationLevel.None, TransportSecurityHelpers.EndGetUserNameCredential(result));
        }

        private void CompleteGetSspiCredential(IAsyncResult result)
        {
          TokenImpersonationLevel impersonationLevel;
          AuthenticationLevel authenticationLevel;
          NetworkCredential sspiCredential = TransportSecurityHelpers.EndGetSspiCredential(result, out impersonationLevel, out authenticationLevel);
          if (this.factory.AuthenticationScheme == AuthenticationSchemes.Digest)
            HttpChannelUtilities.ValidateDigestCredential(ref sspiCredential, impersonationLevel);
          else if (this.factory.AuthenticationScheme == AuthenticationSchemes.Ntlm && authenticationLevel == AuthenticationLevel.MutualAuthRequired)
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("CredentialDisallowsNtlm")));
          this.SetupWebRequest(authenticationLevel, impersonationLevel, sspiCredential);
        }

        private void SetupWebRequest(AuthenticationLevel authenticationLevel, TokenImpersonationLevel impersonationLevel, NetworkCredential credential)
        {
          this.request = this.factory.GetWebRequest(this.to, this.via, credential, impersonationLevel, authenticationLevel, this.proxyTokenProvider, this.clientCertificateToken, this.timeoutHelper.RemainingTime(), false);
        }

        private void CloseTokenProvidersIfRequired()
        {
          if (!this.factory.ManualAddressing)
            return;
          if (this.tokenProvider != null)
            this.tokenProvider.Abort();
          if (this.proxyTokenProvider == null)
            return;
          this.proxyTokenProvider.Abort();
        }

        private static void OnGetSspiCredential(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult requestAsyncResult = (HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult) result.AsyncState;
          Exception exception = (Exception) null;
          try
          {
            requestAsyncResult.CompleteGetSspiCredential(result);
            requestAsyncResult.CloseTokenProvidersIfRequired();
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
              throw;
            else
              exception = ex;
          }
          requestAsyncResult.Complete(false, exception);
        }

        private static void OnGetUserNameCredential(IAsyncResult result)
        {
          if (result.CompletedSynchronously)
            return;
          HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult requestAsyncResult = (HttpChannelFactory<TChannel>.HttpRequestChannel.GetWebRequestAsyncResult) result.AsyncState;
          Exception exception = (Exception) null;
          try
          {
            requestAsyncResult.CompleteGetUserNameCredential(result);
            requestAsyncResult.CloseTokenProvidersIfRequired();
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
              throw;
            else
              exception = ex;
          }
          requestAsyncResult.Complete(false, exception);
        }
      }
    }

    private class WebProxyFactory
    {
      private Uri address;
      private bool bypassOnLocal;
      private AuthenticationSchemes authenticationScheme;

      internal AuthenticationSchemes AuthenticationScheme
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.authenticationScheme;
        }
      }

      public WebProxyFactory(Uri address, bool bypassOnLocal, AuthenticationSchemes authenticationScheme)
      {
        this.address = address;
        this.bypassOnLocal = bypassOnLocal;
        if (!AuthenticationSchemesHelper.IsSingleton(authenticationScheme))
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("value", System.ServiceModel.SR.GetString("HttpRequiresSingleAuthScheme", new object[1]
          {
            (object) authenticationScheme
          }));
        else
          this.authenticationScheme = authenticationScheme;
      }

      public IWebProxy CreateWebProxy(HttpWebRequest request, SecurityTokenProviderContainer tokenProvider, TimeSpan timeout)
      {
        WebProxy webProxy = new WebProxy(this.address, this.bypassOnLocal);
        if (this.authenticationScheme != AuthenticationSchemes.Anonymous)
        {
          TokenImpersonationLevel impersonationLevel;
          AuthenticationLevel authenticationLevel;
          NetworkCredential credential = HttpChannelUtilities.GetCredential(this.authenticationScheme, tokenProvider, timeout, out impersonationLevel, out authenticationLevel);
          if (!TokenImpersonationLevelHelper.IsGreaterOrEqual(impersonationLevel, request.ImpersonationLevel))
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("ProxyImpersonationLevelMismatch", (object) impersonationLevel, (object) request.ImpersonationLevel)));
          else if (authenticationLevel == AuthenticationLevel.MutualAuthRequired && request.AuthenticationLevel != AuthenticationLevel.MutualAuthRequired)
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("ProxyAuthenticationLevelMismatch", (object) authenticationLevel, (object) request.AuthenticationLevel)));
          else
            webProxy.Credentials = (ICredentials) new CredentialCache()
            {
              {
                this.address,
                AuthenticationSchemesHelper.ToString(this.authenticationScheme),
                credential
              }
            };
        }
        return (IWebProxy) webProxy;
      }
    }
  }
}
