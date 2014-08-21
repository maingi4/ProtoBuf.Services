// Type: System.ServiceModel.Activation.HostedHttpContext
// Assembly: System.ServiceModel.Activation, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.Activation.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Runtime;
using System.Security;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace System.ServiceModel.Activation
{
  internal class HostedHttpContext : HttpRequestContext
  {
    private HostedHttpContext.HostedRequestContainer requestContainer;
    private HostedHttpRequestAsyncResult result;
    private TaskCompletionSource<object> webSocketWaitingTask;
    private RemoteEndpointMessageProperty remoteEndpointMessageProperty;
    private TaskCompletionSource<WebSocketContext> webSocketContextTaskSource;
    private int impersonationReleased;

    public override string HttpMethod
    {
      get
      {
        return this.result.GetHttpMethod();
      }
    }

    public override bool IsWebSocketRequest
    {
      get
      {
        return this.result.IsWebSocketRequest;
      }
    }

    public HostedHttpContext(HttpChannelListener listener, HostedHttpRequestAsyncResult result)
      : base(listener, (Message) null, result.EventTraceActivity)
    {
      AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
      this.result = result;
      result.AddRefForImpersonation();
    }

    internal void CompleteWithException(Exception ex)
    {
      this.result.CompleteOperation(ex);
    }

    protected override SecurityMessageProperty OnProcessAuthentication()
    {
      return this.Listener.ProcessAuthentication((HttpChannelListener.IHttpAuthenticationContext) this.result);
    }

    protected override HttpStatusCode ValidateAuthentication()
    {
      return this.Listener.ValidateAuthentication((HttpChannelListener.IHttpAuthenticationContext) this.result);
    }

    protected override Task<WebSocketContext> AcceptWebSocketCore(HttpResponseMessage response, string protocol)
    {
      this.BeforeAcceptWebSocket(response);
      this.webSocketContextTaskSource = new TaskCompletionSource<WebSocketContext>();
      this.result.Application.Context.AcceptWebSocketRequest(new Func<AspNetWebSocketContext, Task>(this.PostAcceptWebSocket), new AspNetWebSocketOptions()
      {
        SubProtocol = protocol
      });
      this.result.OnReplySent();
      return this.webSocketContextTaskSource.Task;
    }

    protected override void OnAcceptWebSocketSuccess(WebSocketContext context, HttpRequestMessage requestMessage)
    {
      base.OnAcceptWebSocketSuccess(context, this.remoteEndpointMessageProperty, (byte[]) null, false, requestMessage);
    }

    protected override void OnReply(Message message, TimeSpan timeout)
    {
      this.CloseHostedRequestContainer();
      base.OnReply(message, timeout);
    }

    protected override IAsyncResult OnBeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.CloseHostedRequestContainer();
      return base.OnBeginReply(message, timeout, callback, state);
    }

    protected override void OnAbort()
    {
      base.OnAbort();
      this.result.Abort();
    }

    protected override void Cleanup()
    {
      base.Cleanup();
      if (Interlocked.Increment(ref this.impersonationReleased) != 1)
        return;
      this.result.ReleaseImpersonation();
    }

    protected override HttpInput GetHttpInput()
    {
      return (HttpInput) new HostedHttpContext.HostedHttpInput(this);
    }

    public override HttpOutput GetHttpOutput(Message message)
    {
      HttpInput httpInput = base.GetHttpInput(false);
      if (httpInput != null && httpInput.ContentLength == -1L && !OSEnvironmentHelper.IsVistaOrGreater || !this.KeepAliveEnabled)
        this.result.SetConnectionClose();
      ICompressedMessageEncoder compressedMessageEncoder = this.Listener.MessageEncoderFactory.Encoder as ICompressedMessageEncoder;
      if (compressedMessageEncoder != null && compressedMessageEncoder.CompressionEnabled)
      {
        string acceptEncoding = this.result.GetAcceptEncoding();
        compressedMessageEncoder.AddCompressedMessageProperties(message, acceptEncoding);
      }
      return (HttpOutput) new HostedHttpContext.HostedRequestHttpOutput(this.result, (IHttpTransportFactorySettings) this.Listener, message, this);
    }

    protected override void OnClose(TimeSpan timeout)
    {
      base.OnClose(timeout);
      this.result.OnReplySent();
    }

    private void CloseHostedRequestContainer()
    {
      if (this.requestContainer == null)
        return;
      this.requestContainer.Close();
      this.requestContainer = (HostedHttpContext.HostedRequestContainer) null;
    }

    private void BeforeAcceptWebSocket(HttpResponseMessage response)
    {
      this.SetRequestContainer(new HostedHttpContext.HostedRequestContainer(this.result));
      string address = string.Empty;
      int port = 0;
      if (this.requestContainer.TryGetAddressAndPort(out address, out port))
        this.remoteEndpointMessageProperty = new RemoteEndpointMessageProperty(address, port);
      this.CloseHostedRequestContainer();
      HostedHttpContext.AppendHeaderFromHttpResponseMessageToResponse(response, this.result);
    }

    private Task PostAcceptWebSocket(AspNetWebSocketContext context)
    {
      this.webSocketWaitingTask = new TaskCompletionSource<object>();
      this.WebSocketChannel.Closed += new EventHandler(this.FinishWebSocketWaitingTask);
      this.webSocketContextTaskSource.SetResult((WebSocketContext) context);
      return (Task) this.webSocketWaitingTask.Task;
    }

    private static void AppendHeaderFromHttpResponseMessageToResponse(HttpResponseMessage response, HostedHttpRequestAsyncResult result)
    {
      HostedHttpContext.AppendHeaderToResponse((HttpHeaders) response.Headers, result);
      if (response.Content == null)
        return;
      HostedHttpContext.AppendHeaderToResponse((HttpHeaders) response.Content.Headers, result);
    }

    private static void AppendHeaderToResponse(HttpHeaders headers, HostedHttpRequestAsyncResult result)
    {
      foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in headers)
      {
        foreach (string str in keyValuePair.Value)
          result.AppendHeader(keyValuePair.Key, str);
      }
    }

    private void FinishWebSocketWaitingTask(object sender, EventArgs args)
    {
      this.webSocketWaitingTask.TrySetResult((object) null);
    }

    private void SetRequestContainer(HostedHttpContext.HostedRequestContainer requestContainer)
    {
      this.requestContainer = requestContainer;
    }

    private class HostedRequestContainer : RemoteEndpointMessageProperty.IRemoteEndpointProvider, HttpRequestMessageProperty.IHttpHeaderProvider
    {
      private volatile bool isClosed;
      private HostedHttpRequestAsyncResult result;
      private object thisLock;

      object ThisLock
      {
        private get
        {
          return this.thisLock;
        }
      }

      public HostedRequestContainer(HostedHttpRequestAsyncResult result)
      {
        AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
        this.result = result;
        this.thisLock = new object();
      }

      public void Close()
      {
        lock (this.ThisLock)
          this.isClosed = true;
      }

      [SecuritySafeCritical]
      void HttpRequestMessageProperty.IHttpHeaderProvider.CopyHeaders(WebHeaderCollection headers)
      {
        if (this.isClosed)
          return;
        lock (this.ThisLock)
        {
          if (this.isClosed)
            return;
          HttpChannelUtilities.CopyHeadersToNameValueCollection(this.result.Application.Request.Headers, (NameValueCollection) headers);
        }
      }

      [SecuritySafeCritical]
      string RemoteEndpointMessageProperty.IRemoteEndpointProvider.GetAddress()
      {
        if (!this.isClosed)
        {
          lock (this.ThisLock)
          {
            if (!this.isClosed)
              return this.result.Application.Request.UserHostAddress;
          }
        }
        return string.Empty;
      }

      [SecuritySafeCritical]
      int RemoteEndpointMessageProperty.IRemoteEndpointProvider.GetPort()
      {
        int result = 0;
        if (!this.isClosed)
        {
          lock (this.ThisLock)
          {
            if (!this.isClosed)
            {
              string local_1 = this.result.Application.Request.ServerVariables["REMOTE_PORT"];
              if (!string.IsNullOrEmpty(local_1))
              {
                if (int.TryParse(local_1, out result))
                  goto label_9;
              }
              result = 0;
            }
          }
        }
label_9:
        return result;
      }

      [SecuritySafeCritical]
      public bool TryGetAddressAndPort(out string address, out int port)
      {
        address = string.Empty;
        port = 0;
        if (!this.isClosed)
        {
          lock (this.ThisLock)
          {
            if (!this.isClosed)
            {
              address = this.result.Application.Request.UserHostAddress;
              IServiceProvider local_0 = (IServiceProvider) this.result.Application.Context;
              port = HostedHttpContext.HostedRequestContainer.GetRemotePort(local_0);
              return true;
            }
          }
        }
        return false;
      }

      [SecuritySafeCritical]
      [SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
      private static int GetRemotePort(IServiceProvider provider)
      {
        return ((HttpWorkerRequest) provider.GetService(typeof (HttpWorkerRequest))).GetRemotePort();
      }
    }

    private class HostedHttpInput : HttpInput
    {
      private int contentLength;
      private string contentType;
      private HostedHttpContext hostedHttpContext;
      private byte[] preReadBuffer;

      public override long ContentLength
      {
        get
        {
          return (long) this.contentLength;
        }
      }

      protected override string ContentTypeCore
      {
        get
        {
          return this.contentType;
        }
      }

      protected override bool HasContent
      {
        get
        {
          if (this.preReadBuffer == null)
            return this.ContentLength > 0L;
          else
            return true;
        }
      }

      protected override string SoapActionHeader
      {
        get
        {
          return this.hostedHttpContext.result.GetSoapAction();
        }
      }

      protected override ChannelBinding ChannelBinding
      {
        get
        {
          return ChannelBindingUtility.DuplicateToken(this.hostedHttpContext.result.GetChannelBinding());
        }
      }

      public HostedHttpInput(HostedHttpContext hostedHttpContext)
        : base((IHttpTransportFactorySettings) hostedHttpContext.Listener, true, hostedHttpContext.Listener.IsChannelBindingSupportEnabled)
      {
        AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
        this.hostedHttpContext = hostedHttpContext;
        this.contentType = hostedHttpContext.Listener.MessageEncoderFactory.Encoder.MessageVersion.Envelope != EnvelopeVersion.Soap11 ? hostedHttpContext.result.GetContentTypeFast() : hostedHttpContext.result.GetContentType();
        this.contentLength = hostedHttpContext.result.GetContentLength();
        if (this.contentLength != 0)
          return;
        this.preReadBuffer = hostedHttpContext.result.GetPrereadBuffer(ref this.contentLength);
      }

      protected override void AddProperties(Message message)
      {
        HostedHttpContext.HostedRequestContainer requestContainer = new HostedHttpContext.HostedRequestContainer(this.hostedHttpContext.result);
        HttpRequestMessageProperty requestMessageProperty = new HttpRequestMessageProperty((HttpRequestMessageProperty.IHttpHeaderProvider) requestContainer);
        requestMessageProperty.Method = this.hostedHttpContext.HttpMethod;
        if (this.hostedHttpContext.result.RequestUri.Query.Length > 1)
          requestMessageProperty.QueryString = this.hostedHttpContext.result.RequestUri.Query.Substring(1);
        message.Properties.Add(HttpRequestMessageProperty.Name, (object) requestMessageProperty);
        message.Properties.Add(HostingMessageProperty.Name, (object) HostedHttpContext.HostedHttpInput.CreateMessagePropertyFromHostedResult(this.hostedHttpContext.result));
        message.Properties.Via = this.hostedHttpContext.result.RequestUri;
        RemoteEndpointMessageProperty endpointMessageProperty = new RemoteEndpointMessageProperty((RemoteEndpointMessageProperty.IRemoteEndpointProvider) requestContainer);
        message.Properties.Add(RemoteEndpointMessageProperty.Name, (object) endpointMessageProperty);
        this.hostedHttpContext.SetRequestContainer(requestContainer);
      }

      public override void ConfigureHttpRequestMessage(HttpRequestMessage message)
      {
        message.Method = new HttpMethod(this.hostedHttpContext.result.GetHttpMethod());
        message.RequestUri = this.hostedHttpContext.result.RequestUri;
        foreach (string header in this.hostedHttpContext.result.Application.Context.Request.Headers.Keys)
          HttpRequestMessageExtensionMethods.AddHeader(message, header, this.hostedHttpContext.result.Application.Context.Request.Headers[header]);
        RemoteEndpointMessageProperty endpointMessageProperty = new RemoteEndpointMessageProperty((RemoteEndpointMessageProperty.IRemoteEndpointProvider) new HostedHttpContext.HostedRequestContainer(this.hostedHttpContext.result));
        message.Properties.Add(RemoteEndpointMessageProperty.Name, (object) endpointMessageProperty);
      }

      [SecuritySafeCritical]
      private static HostingMessageProperty CreateMessagePropertyFromHostedResult(HostedHttpRequestAsyncResult result)
      {
        return new HostingMessageProperty(result);
      }

      protected override Stream GetInputStream()
      {
        if (this.preReadBuffer != null)
          return (Stream) new HostedHttpContext.HostedHttpInput.HostedInputStream(this.hostedHttpContext, this.preReadBuffer);
        else
          return (Stream) new HostedHttpContext.HostedHttpInput.HostedInputStream(this.hostedHttpContext);
      }

      private class HostedInputStream : HttpDelayedAcceptStream
      {
        private HostedHttpRequestAsyncResult result;

        public HostedInputStream(HostedHttpContext hostedContext)
          : base(hostedContext.result.GetInputStream())
        {
          AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
          this.result = hostedContext.result;
        }

        public HostedInputStream(HostedHttpContext hostedContext, byte[] preReadBuffer)
          : base((Stream) new PreReadStream(hostedContext.result.GetInputStream(), preReadBuffer))
        {
          AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
          this.result = hostedContext.result;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
          if (!this.result.TryStartStreamedRead())
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.Activation.SR.RequestContextAborted));
          bool flag = true;
          try
          {
            IAsyncResult asyncResult = base.BeginRead(buffer, offset, count, callback, state);
            flag = false;
            return asyncResult;
          }
          catch (HttpException ex)
          {
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError(HostedHttpContext.HostedHttpInput.HostedInputStream.CreateCommunicationException(ex));
          }
          finally
          {
            if (flag)
              this.result.SetStreamedReadFinished();
          }
        }

        public override int EndRead(IAsyncResult result)
        {
          try
          {
            return base.EndRead(result);
          }
          catch (HttpException ex)
          {
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError(HostedHttpContext.HostedHttpInput.HostedInputStream.CreateCommunicationException(ex));
          }
          finally
          {
            this.result.SetStreamedReadFinished();
          }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
          if (!this.result.TryStartStreamedRead())
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.Activation.SR.RequestContextAborted));
          try
          {
            return base.Read(buffer, offset, count);
          }
          catch (HttpException ex)
          {
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError(HostedHttpContext.HostedHttpInput.HostedInputStream.CreateCommunicationException(ex));
          }
          finally
          {
            this.result.SetStreamedReadFinished();
          }
        }

        private static Exception CreateCommunicationException(HttpException hostedException)
        {
          if (hostedException.WebEventCode == 3004)
            return (Exception) HttpInput.CreateHttpProtocolException(System.ServiceModel.Activation.SR.Hosting_MaxRequestLengthExceeded, HttpStatusCode.RequestEntityTooLarge, (string) null, (Exception) hostedException);
          else
            return (Exception) new CommunicationException(hostedException.Message, (Exception) hostedException);
        }
      }
    }

    private class HostedRequestHttpOutput : HttpOutput
    {
      private HostedHttpRequestAsyncResult result;
      private HostedHttpContext context;
      private string mimeVersion;
      private string contentType;
      private int statusCode;
      private bool isSettingMimeHeader;
      private bool isSettingContentType;

      public HostedRequestHttpOutput(HostedHttpRequestAsyncResult result, IHttpTransportFactorySettings settings, Message message, HostedHttpContext context)
        : base(settings, message, false, false)
      {
        AspNetPartialTrustHelpers.FailIfInPartialTrustOutsideAspNet();
        this.result = result;
        this.context = context;
        if (TransferModeHelper.IsResponseStreamed(settings.TransferMode))
          result.SetTransferModeToStreaming();
        if (message.IsFault)
          this.statusCode = 500;
        else
          this.statusCode = 200;
      }

      protected override Stream GetOutputStream()
      {
        return (Stream) new HostedHttpContext.HostedRequestHttpOutput.HostedResponseOutputStream(this.result, this.context);
      }

      protected override void AddHeader(string name, string value)
      {
        this.result.AppendHeader(name, value);
      }

      protected override void AddMimeVersion(string version)
      {
        if (!this.isSettingMimeHeader)
          this.mimeVersion = version;
        else
          this.result.AppendHeader("MIME-Version", this.mimeVersion);
      }

      protected override void SetContentType(string contentType)
      {
        if (!this.isSettingContentType)
          this.contentType = contentType;
        else
          this.result.SetContentType(contentType);
      }

      protected override void SetContentEncoding(string contentEncoding)
      {
        this.result.AppendHeader("Content-Encoding", contentEncoding);
      }

      protected override void SetContentLength(int contentLength)
      {
        this.result.AppendHeader("content-length", contentLength.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }

      protected override void SetStatusCode(HttpStatusCode statusCode)
      {
        this.result.SetStatusCode((int) statusCode);
      }

      protected override void SetStatusDescription(string statusDescription)
      {
        this.result.SetStatusDescription(statusDescription);
      }

      protected override bool PrepareHttpSend(Message message)
      {
        bool flag1 = base.PrepareHttpSend(message);
        bool flag2 = string.Compare(this.context.HttpMethod, "HEAD", StringComparison.OrdinalIgnoreCase) == 0;
        if (flag2)
          flag1 = true;
        object obj;
        if (message.Properties.TryGetValue(HttpResponseMessageProperty.Name, out obj))
        {
          HttpResponseMessageProperty responseMessageProperty = (HttpResponseMessageProperty) obj;
          if (responseMessageProperty.SuppressPreamble)
          {
            if (!flag1)
              return responseMessageProperty.SuppressEntityBody;
            else
              return true;
          }
          else
          {
            this.SetStatusCode(responseMessageProperty.StatusCode);
            if (responseMessageProperty.StatusDescription != null)
              this.SetStatusDescription(responseMessageProperty.StatusDescription);
            WebHeaderCollection headers = responseMessageProperty.Headers;
            for (int index = 0; index < headers.Count; ++index)
            {
              string str = headers.Keys[index];
              string s = ((NameValueCollection) headers)[index];
              if (string.Compare(str, "content-type", StringComparison.OrdinalIgnoreCase) == 0)
                this.contentType = s;
              else if (string.Compare(str, "MIME-Version", StringComparison.OrdinalIgnoreCase) == 0)
                this.mimeVersion = s;
              else if (string.Compare(str, "content-length", StringComparison.OrdinalIgnoreCase) == 0)
              {
                int result = -1;
                if (flag2 && int.TryParse(s, out result))
                  this.SetContentLength(result);
              }
              else
                this.AddHeader(str, s);
            }
            if (responseMessageProperty.SuppressEntityBody)
            {
              this.contentType = (string) null;
              flag1 = true;
            }
          }
        }
        else
          this.SetStatusCode((HttpStatusCode) this.statusCode);
        if (this.contentType != null && this.contentType.Length != 0)
        {
          string contentEncoding;
          if (this.CanSendCompressedResponses && HttpChannelUtilities.GetHttpResponseTypeAndEncodingForCompression(ref this.contentType, out contentEncoding))
            this.result.SetContentEncoding(contentEncoding);
          this.isSettingContentType = true;
          this.SetContentType(this.contentType);
        }
        if (this.mimeVersion != null)
        {
          this.isSettingMimeHeader = true;
          this.AddMimeVersion(this.mimeVersion);
        }
        return flag1;
      }

      protected override void PrepareHttpSendCore(HttpResponseMessage message)
      {
        this.result.SetStatusCode((int) message.StatusCode);
        if (message.ReasonPhrase != null)
          this.result.SetStatusDescription(message.ReasonPhrase);
        HostedHttpContext.AppendHeaderFromHttpResponseMessageToResponse(message, this.result);
      }

      private class HostedResponseOutputStream : BytesReadPositionStream
      {
        private HostedHttpContext context;
        private HostedHttpRequestAsyncResult result;

        public HostedResponseOutputStream(HostedHttpRequestAsyncResult result, HostedHttpContext context)
          : base(result.GetOutputStream())
        {
          this.context = context;
          this.result = result;
        }

        public override void Close()
        {
          try
          {
            base.Close();
          }
          catch (Exception ex)
          {
            this.CheckWrapThrow(ex);
            throw;
          }
          finally
          {
            this.result.OnReplySent();
          }
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
          try
          {
            return base.BeginWrite(buffer, offset, count, callback, state);
          }
          catch (Exception ex)
          {
            this.CheckWrapThrow(ex);
            throw;
          }
        }

        public override void EndWrite(IAsyncResult result)
        {
          try
          {
            base.EndWrite(result);
          }
          catch (Exception ex)
          {
            this.CheckWrapThrow(ex);
            throw;
          }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
          try
          {
            base.Write(buffer, offset, count);
          }
          catch (Exception ex)
          {
            this.CheckWrapThrow(ex);
            throw;
          }
        }

        private void CheckWrapThrow(Exception e)
        {
          if (Fx.IsFatal(e))
            return;
          if (e is HttpException)
          {
            if (this.context.Aborted)
              throw System.ServiceModel.Activation.FxTrace.Exception.AsError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.Activation.SR.RequestContextAborted, e));
            else
              throw System.ServiceModel.Activation.FxTrace.Exception.AsError((Exception) new CommunicationException(e.Message, e));
          }
          else
          {
            if (!this.context.Aborted)
              return;
            if (DiagnosticUtility.ShouldTraceError)
              TraceUtility.TraceEvent(TraceEventType.Error, 262174, System.ServiceModel.Activation.SR.TraceCodeRequestContextAbort, (object) this, e);
            throw System.ServiceModel.Activation.FxTrace.Exception.AsError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.Activation.SR.RequestContextAborted));
          }
        }
      }
    }
  }
}
