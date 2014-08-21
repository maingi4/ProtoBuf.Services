// Type: System.ServiceModel.Channels.HttpRequestContext
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime;
using System.Runtime.Diagnostics;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Diagnostics.Application;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace System.ServiceModel.Channels
{
  internal abstract class HttpRequestContext : System.ServiceModel.Channels.RequestContextBase
  {
    private HttpOutput httpOutput;
    private bool errorGettingHttpInput;
    private HttpChannelListener listener;
    private SecurityMessageProperty securityProperty;
    private EventTraceActivity eventTraceActivity;
    private HttpPipeline httpPipeline;
    private ServerWebSocketTransportDuplexSessionChannel webSocketChannel;

    public bool KeepAliveEnabled
    {
      get
      {
        return this.listener.KeepAliveEnabled;
      }
    }

    public bool HttpMessagesSupported
    {
      get
      {
        return this.listener.HttpMessageSettings.HttpMessagesSupported;
      }
    }

    public abstract string HttpMethod { get; }

    public abstract bool IsWebSocketRequest { get; }

    internal ServerWebSocketTransportDuplexSessionChannel WebSocketChannel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.webSocketChannel;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.webSocketChannel = value;
      }
    }

    internal HttpChannelListener Listener
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.listener;
      }
    }

    internal EventTraceActivity EventTraceActivity
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.eventTraceActivity;
      }
    }

    protected HttpRequestContext(HttpChannelListener listener, Message requestMessage, EventTraceActivity eventTraceActivity)
      : base(requestMessage, listener.InternalCloseTimeout, listener.InternalSendTimeout)
    {
      this.listener = listener;
      this.eventTraceActivity = eventTraceActivity;
    }

    public HttpInput GetHttpInput(bool throwOnError)
    {
      HttpPipeline httpPipeline = this.httpPipeline;
      if (httpPipeline != null && httpPipeline.IsHttpInputInitialized)
        return httpPipeline.HttpInput;
      HttpInput httpInput = (HttpInput) null;
      if (!throwOnError)
      {
        if (this.errorGettingHttpInput)
          goto label_8;
      }
      try
      {
        httpInput = this.GetHttpInput();
        this.errorGettingHttpInput = false;
      }
      catch (Exception ex)
      {
        this.errorGettingHttpInput = true;
        if (throwOnError || Fx.IsFatal(ex))
          throw;
        else
          DiagnosticUtility.TraceHandledException(ex, TraceEventType.Warning);
      }
label_8:
      return httpInput;
    }

    internal static HttpRequestContext CreateContext(HttpChannelListener listener, HttpListenerContext listenerContext, EventTraceActivity eventTraceActivity)
    {
      return (HttpRequestContext) new HttpRequestContext.ListenerHttpContext(listener, listenerContext, eventTraceActivity);
    }

    protected abstract SecurityMessageProperty OnProcessAuthentication();

    public abstract HttpOutput GetHttpOutput(Message message);

    protected abstract HttpInput GetHttpInput();

    public HttpOutput GetHttpOutputCore(Message message)
    {
      if (this.httpOutput != null)
        return this.httpOutput;
      else
        return this.GetHttpOutput(message);
    }

    protected override void OnAbort()
    {
      if (this.httpOutput != null)
        this.httpOutput.Abort(HttpAbortReason.Aborted);
      this.Cleanup();
    }

    protected override void OnClose(TimeSpan timeout)
    {
      try
      {
        if (this.httpOutput == null)
          return;
        this.httpOutput.Close();
      }
      finally
      {
        this.Cleanup();
      }
    }

    protected virtual void Cleanup()
    {
      if (this.httpPipeline == null)
        return;
      this.httpPipeline.Close();
    }

    public void InitializeHttpPipeline(TransportIntegrationHandler transportIntegrationHandler)
    {
      this.httpPipeline = HttpPipeline.CreateHttpPipeline(this, transportIntegrationHandler, this.IsWebSocketRequest);
    }

    internal void SetMessage(Message message, Exception requestException)
    {
      if (message == null && requestException == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ProtocolException(System.ServiceModel.SR.GetString("MessageXmlProtocolError"), (Exception) new XmlException(System.ServiceModel.SR.GetString("MessageIsEmpty"))));
      this.TraceHttpMessageReceived(message);
      if (requestException != null)
      {
        this.SetRequestMessage(requestException);
        message.Close();
      }
      else
      {
        message.Properties.Security = this.securityProperty != null ? (SecurityMessageProperty) this.securityProperty.CreateCopy() : (SecurityMessageProperty) null;
        this.SetRequestMessage(message);
      }
    }

    protected abstract HttpStatusCode ValidateAuthentication();

    protected override void OnReply(Message message, TimeSpan timeout)
    {
      System.Runtime.TimeoutHelper timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
      Message message1 = message;
      try
      {
        bool flag = this.PrepareReply(ref message1);
        this.httpPipeline.SendReply(message1, timeoutHelper.RemainingTime());
        if (flag)
          this.httpOutput.Close();
        if (!TD.MessageSentByTransportIsEnabled())
          return;
        TD.MessageSentByTransport(this.eventTraceActivity, this.Listener.Uri.AbsoluteUri);
      }
      finally
      {
        if (message != null && !object.ReferenceEquals((object) message, (object) message1))
          message1.Close();
      }
    }

    protected override IAsyncResult OnBeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new HttpRequestContext.ReplyAsyncResult(this, message, timeout, callback, state);
    }

    protected override void OnEndReply(IAsyncResult result)
    {
      HttpRequestContext.ReplyAsyncResult.End(result);
    }

    public bool ProcessAuthentication()
    {
      if (TD.HttpContextBeforeProcessAuthenticationIsEnabled())
        TD.HttpContextBeforeProcessAuthentication(this.eventTraceActivity);
      HttpStatusCode statusCode1 = this.ValidateAuthentication();
      if (statusCode1 == HttpStatusCode.OK)
      {
        bool flag = false;
        HttpStatusCode statusCode2 = HttpStatusCode.Forbidden;
        try
        {
          this.securityProperty = this.OnProcessAuthentication();
          flag = true;
          return true;
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
          {
            throw;
          }
          else
          {
            if (ex.Data.Contains((object) "HttpStatusCode") && ex.Data[(object) "HttpStatusCode"] is HttpStatusCode)
              statusCode2 = (HttpStatusCode) ex.Data[(object) "HttpStatusCode"];
            throw;
          }
        }
        finally
        {
          if (!flag)
            this.SendResponseAndClose(statusCode2);
        }
      }
      else
      {
        this.SendResponseAndClose(statusCode1);
        return false;
      }
    }

    internal void SendResponseAndClose(HttpStatusCode statusCode)
    {
      this.SendResponseAndClose(statusCode, string.Empty);
    }

    internal void SendResponseAndClose(HttpStatusCode statusCode, string statusDescription)
    {
      if (this.ReplyInitiated)
      {
        this.Close();
      }
      else
      {
        using (Message ackMessage = this.CreateAckMessage(statusCode, statusDescription))
          this.Reply(ackMessage);
        this.Close();
      }
    }

    internal void SendResponseAndClose(HttpResponseMessage httpResponseMessage)
    {
      if (this.TryInitiateReply())
      {
        try
        {
          if (this.httpOutput == null)
            this.httpOutput = this.GetHttpOutputCore((Message) new NullMessage());
          this.httpOutput.Send(httpResponseMessage, this.DefaultSendTimeout);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            DiagnosticUtility.TraceHandledException(ex, TraceEventType.Information);
        }
      }
      try
      {
        this.Close();
      }
      catch (Exception ex)
      {
        if (Fx.IsFatal(ex))
          throw;
        else
          DiagnosticUtility.TraceHandledException(ex, TraceEventType.Information);
      }
    }

    public void AcceptWebSocket(HttpResponseMessage response, string protocol, TimeSpan timeout)
    {
      bool flag = false;
      Task<WebSocketContext> task;
      try
      {
        task = this.AcceptWebSocketCore(response, protocol);
        try
        {
          if (!task.Wait(System.Runtime.TimeoutHelper.ToMilliseconds(timeout)))
            throw FxTrace.Exception.AsError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("AcceptWebSocketTimedOutError")));
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            WebSocketHelper.ThrowCorrectException(ex);
        }
        flag = true;
      }
      finally
      {
        if (!flag)
          this.OnAcceptWebSocketError();
      }
      this.SetReplySent();
      this.OnAcceptWebSocketSuccess(task.Result, response.RequestMessage);
    }

    protected abstract Task<WebSocketContext> AcceptWebSocketCore(HttpResponseMessage response, string protocol);

    protected virtual void OnAcceptWebSocketError()
    {
    }

    protected abstract void OnAcceptWebSocketSuccess(WebSocketContext context, HttpRequestMessage requestMessage);

    protected void OnAcceptWebSocketSuccess(WebSocketContext context, RemoteEndpointMessageProperty remoteEndpointMessageProperty, byte[] webSocketInternalBuffer, bool shouldDisposeWebSocketAfterClose, HttpRequestMessage requestMessage)
    {
      this.webSocketChannel.SetWebSocketInfo(context, remoteEndpointMessageProperty, this.securityProperty, webSocketInternalBuffer, shouldDisposeWebSocketAfterClose, requestMessage);
    }

    public IAsyncResult BeginAcceptWebSocket(HttpResponseMessage response, string protocol, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new HttpRequestContext.AcceptWebSocketAsyncResult(this, response, protocol, callback, state);
    }

    public void EndAcceptWebSocket(IAsyncResult result)
    {
      HttpRequestContext.AcceptWebSocketAsyncResult.End(result);
    }

    internal IAsyncResult BeginProcessInboundRequest(ReplyChannelAcceptor replyChannelAcceptor, System.Action acceptorCallback, AsyncCallback callback, object state)
    {
      return this.httpPipeline.BeginProcessInboundRequest(replyChannelAcceptor, acceptorCallback, callback, state);
    }

    internal void EndProcessInboundRequest(IAsyncResult result)
    {
      this.httpPipeline.EndProcessInboundRequest(result);
    }

    private void TraceHttpMessageReceived(Message message)
    {
      if (!FxTrace.Trace.IsEnd2EndActivityTracingEnabled)
        return;
      bool flag = false;
      Guid relatedActivityId = this.eventTraceActivity != null ? this.eventTraceActivity.ActivityId : Guid.Empty;
      if (message.Headers.MessageId == (UniqueId) null)
      {
        HttpRequestMessageProperty property;
        if (message.Properties.TryGetValue<HttpRequestMessageProperty>(HttpRequestMessageProperty.Name, out property))
        {
          try
          {
            string s = ((NameValueCollection) property.Headers)[EventTraceActivity.Name];
            if (!string.IsNullOrEmpty(s))
            {
              byte[] b = Convert.FromBase64String(s);
              if (b != null)
              {
                if (b.Length == 16)
                {
                  this.eventTraceActivity = new EventTraceActivity(new Guid(b), true);
                  message.Properties[EventTraceActivity.Name] = (object) this.eventTraceActivity;
                  flag = true;
                }
              }
            }
          }
          catch (Exception ex)
          {
            if (Fx.IsFatal(ex))
              throw;
          }
        }
      }
      if (!flag)
        this.eventTraceActivity = EventTraceActivityHelper.TryExtractActivity(message, true);
      if (!TD.MessageReceivedByTransportIsEnabled())
        return;
      TD.MessageReceivedByTransport(this.eventTraceActivity, this.listener == null || !(this.listener.Uri != (Uri) null) ? string.Empty : this.listener.Uri.AbsoluteUri, relatedActivityId);
    }

    private bool PrepareReply(ref Message message)
    {
      bool closeHttpOutput = false;
      if (message == null)
      {
        closeHttpOutput = true;
        message = this.CreateAckMessage(HttpStatusCode.Accepted, string.Empty);
      }
      if (!this.listener.ManualAddressing)
      {
        if (message.Version.Addressing == AddressingVersion.WSAddressingAugust2004)
        {
          if (message.Headers.To == (Uri) null || this.listener.AnonymousUriPrefixMatcher == null || !this.listener.AnonymousUriPrefixMatcher.IsAnonymousUri(message.Headers.To))
            message.Headers.To = message.Version.Addressing.AnonymousUri;
        }
        else if (message.Version.Addressing == AddressingVersion.WSAddressing10 || message.Version.Addressing == AddressingVersion.None)
        {
          if (message.Headers.To != (Uri) null && (this.listener.AnonymousUriPrefixMatcher == null || !this.listener.AnonymousUriPrefixMatcher.IsAnonymousUri(message.Headers.To)))
            message.Headers.To = (Uri) null;
        }
        else
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ProtocolException(System.ServiceModel.SR.GetString("AddressingVersionNotSupported", new object[1]
          {
            (object) message.Version.Addressing
          })));
      }
      message.Properties.AllowOutputBatching = false;
      this.httpOutput = this.GetHttpOutputCore(message);
      HttpInput httpInput = this.httpPipeline.HttpInput;
      if (httpInput != null)
      {
        HttpDelayedAcceptStream delayedAcceptStream = httpInput.GetInputStream(false) as HttpDelayedAcceptStream;
        if (delayedAcceptStream != null && TransferModeHelper.IsRequestStreamed(this.listener.TransferMode) && delayedAcceptStream.EnableDelayedAccept(this.httpOutput, closeHttpOutput))
          return false;
      }
      return true;
    }

    private Message CreateAckMessage(HttpStatusCode statusCode, string statusDescription)
    {
      Message message = (Message) new NullMessage();
      HttpResponseMessageProperty responseMessageProperty = new HttpResponseMessageProperty();
      responseMessageProperty.StatusCode = statusCode;
      responseMessageProperty.SuppressEntityBody = true;
      if (statusDescription.Length > 0)
        responseMessageProperty.StatusDescription = statusDescription;
      message.Properties.Add(HttpResponseMessageProperty.Name, (object) responseMessageProperty);
      return message;
    }

    private class ReplyAsyncResult : AsyncResult
    {
      private static AsyncCallback onSendCompleted;
      private static System.Action<object, HttpResponseMessage> onHttpPipelineSend;
      private bool closeOutputAfterReply;
      private HttpRequestContext context;
      private Message message;
      private Message responseMessage;
      private System.Runtime.TimeoutHelper timeoutHelper;

      public ReplyAsyncResult(HttpRequestContext context, Message message, TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.context = context;
        this.message = message;
        this.responseMessage = (Message) null;
        this.timeoutHelper = new System.Runtime.TimeoutHelper(timeout);
        ThreadTrace.Trace("Begin sending http reply");
        this.responseMessage = this.message;
        if (!this.SendResponse())
          return;
        this.Complete(true);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<HttpRequestContext.ReplyAsyncResult>(result);
      }

      private void OnSendResponseCompleted(IAsyncResult result)
      {
        try
        {
          this.context.httpOutput.EndSend(result);
          ThreadTrace.Trace("End sending http reply");
          if (!this.closeOutputAfterReply)
            return;
          this.context.httpOutput.Close();
        }
        finally
        {
          if (this.message != null && !object.ReferenceEquals((object) this.message, (object) this.responseMessage))
            this.responseMessage.Close();
        }
      }

      private static void OnSendResponseCompletedCallback(IAsyncResult result)
      {
        if (result.CompletedSynchronously)
          return;
        HttpRequestContext.ReplyAsyncResult replyAsyncResult = (HttpRequestContext.ReplyAsyncResult) result.AsyncState;
        Exception exception = (Exception) null;
        try
        {
          replyAsyncResult.OnSendResponseCompleted(result);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        replyAsyncResult.Complete(false, exception);
      }

      private static void OnHttpPipelineSendCallback(object target, HttpResponseMessage httpResponseMessage)
      {
        HttpRequestContext.ReplyAsyncResult replyAsyncResult = (HttpRequestContext.ReplyAsyncResult) target;
        Exception exception = (Exception) null;
        bool flag;
        try
        {
          flag = replyAsyncResult.SendResponse(httpResponseMessage);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
          {
            throw;
          }
          else
          {
            exception = ex;
            flag = true;
          }
        }
        if (!flag)
          return;
        replyAsyncResult.Complete(false, exception);
      }

      public bool SendResponse(HttpResponseMessage httpResponseMessage)
      {
        if (HttpRequestContext.ReplyAsyncResult.onSendCompleted == null)
          HttpRequestContext.ReplyAsyncResult.onSendCompleted = Fx.ThunkCallback(new AsyncCallback(HttpRequestContext.ReplyAsyncResult.OnSendResponseCompletedCallback));
        bool success = false;
        try
        {
          return this.SendResponseCore(httpResponseMessage, out success);
        }
        finally
        {
          if (!success && this.message != null && !object.ReferenceEquals((object) this.message, (object) this.responseMessage))
            this.responseMessage.Close();
        }
      }

      public bool SendResponse()
      {
        if (HttpRequestContext.ReplyAsyncResult.onSendCompleted == null)
          HttpRequestContext.ReplyAsyncResult.onSendCompleted = Fx.ThunkCallback(new AsyncCallback(HttpRequestContext.ReplyAsyncResult.OnSendResponseCompletedCallback));
        bool success = false;
        try
        {
          this.closeOutputAfterReply = this.context.PrepareReply(ref this.responseMessage);
          if (HttpRequestContext.ReplyAsyncResult.onHttpPipelineSend == null)
            HttpRequestContext.ReplyAsyncResult.onHttpPipelineSend = new System.Action<object, HttpResponseMessage>(HttpRequestContext.ReplyAsyncResult.OnHttpPipelineSendCallback);
          if (this.context.httpPipeline.SendAsyncReply(this.responseMessage, HttpRequestContext.ReplyAsyncResult.onHttpPipelineSend, (object) this) == AsyncCompletionResult.Queued)
          {
            success = true;
            return false;
          }
          else
          {
            HttpResponseMessage httpResponseMessage = (HttpResponseMessage) null;
            if (this.context.HttpMessagesSupported)
              httpResponseMessage = HttpResponseMessageProperty.GetHttpResponseMessageFromMessage(this.responseMessage);
            return this.SendResponseCore(httpResponseMessage, out success);
          }
        }
        finally
        {
          if (!success && this.message != null && !object.ReferenceEquals((object) this.message, (object) this.responseMessage))
            this.responseMessage.Close();
        }
      }

      private bool SendResponseCore(HttpResponseMessage httpResponseMessage, out bool success)
      {
        success = false;
        IAsyncResult result = httpResponseMessage != null ? this.context.httpOutput.BeginSend(httpResponseMessage, this.timeoutHelper.RemainingTime(), HttpRequestContext.ReplyAsyncResult.onSendCompleted, (object) this) : this.context.httpOutput.BeginSend(this.timeoutHelper.RemainingTime(), HttpRequestContext.ReplyAsyncResult.onSendCompleted, (object) this);
        success = true;
        if (!result.CompletedSynchronously)
          return false;
        this.OnSendResponseCompleted(result);
        return true;
      }
    }

    private class ListenerHttpContext : HttpRequestContext, HttpRequestMessageProperty.IHttpHeaderProvider
    {
      private HttpListenerContext listenerContext;
      private byte[] webSocketInternalBuffer;

      public override string HttpMethod
      {
        get
        {
          return this.listenerContext.Request.HttpMethod;
        }
      }

      public override bool IsWebSocketRequest
      {
        get
        {
          return this.listenerContext.Request.IsWebSocketRequest;
        }
      }

      public ListenerHttpContext(HttpChannelListener listener, HttpListenerContext listenerContext, EventTraceActivity eventTraceActivity)
        : base(listener, (Message) null, eventTraceActivity)
      {
        this.listenerContext = listenerContext;
      }

      protected override HttpInput GetHttpInput()
      {
        return (HttpInput) new HttpRequestContext.ListenerHttpContext.ListenerContextHttpInput(this);
      }

      protected override Task<WebSocketContext> AcceptWebSocketCore(HttpResponseMessage response, string protocol)
      {
        HttpChannelUtilities.CopyHeadersToNameValueCollection(response, (NameValueCollection) this.listenerContext.Response.Headers);
        this.webSocketInternalBuffer = this.Listener.TakeWebSocketInternalBuffer();
        return System.Runtime.TaskExtensions.Upcast<HttpListenerWebSocketContext, WebSocketContext>(this.listenerContext.AcceptWebSocketAsync(protocol, WebSocketHelper.GetReceiveBufferSize(this.listener.MaxReceivedMessageSize), this.Listener.WebSocketSettings.GetEffectiveKeepAliveInterval(), new ArraySegment<byte>(this.webSocketInternalBuffer)));
      }

      protected override void OnAcceptWebSocketError()
      {
        byte[] buffer = Interlocked.CompareExchange<byte[]>(ref this.webSocketInternalBuffer, (byte[]) null, this.webSocketInternalBuffer);
        if (buffer == null)
          return;
        this.Listener.ReturnWebSocketInternalBuffer(buffer);
      }

      protected override void OnAcceptWebSocketSuccess(WebSocketContext context, HttpRequestMessage requestMessage)
      {
        RemoteEndpointMessageProperty remoteEndpointMessageProperty = (RemoteEndpointMessageProperty) null;
        if (this.listenerContext.Request.RemoteEndPoint != null)
          remoteEndpointMessageProperty = new RemoteEndpointMessageProperty(this.listenerContext.Request.RemoteEndPoint);
        base.OnAcceptWebSocketSuccess(context, remoteEndpointMessageProperty, this.webSocketInternalBuffer, true, requestMessage);
      }

      public override HttpOutput GetHttpOutput(Message message)
      {
        this.listenerContext.Response.KeepAlive = (this.listenerContext.Request.ContentLength64 != -1L || OSEnvironmentHelper.IsVistaOrGreater) && this.listener.KeepAliveEnabled;
        ICompressedMessageEncoder compressedMessageEncoder = this.listener.MessageEncoderFactory.Encoder as ICompressedMessageEncoder;
        if (compressedMessageEncoder != null && compressedMessageEncoder.CompressionEnabled)
        {
          string supportedCompressionTypes = this.listenerContext.Request.Headers["Accept-Encoding"];
          compressedMessageEncoder.AddCompressedMessageProperties(message, supportedCompressionTypes);
        }
        return HttpOutput.CreateHttpOutput(this.listenerContext.Response, (IHttpTransportFactorySettings) this.Listener, message, this.HttpMethod);
      }

      protected override SecurityMessageProperty OnProcessAuthentication()
      {
        return this.Listener.ProcessAuthentication(this.listenerContext);
      }

      protected override HttpStatusCode ValidateAuthentication()
      {
        return this.Listener.ValidateAuthentication(this.listenerContext);
      }

      protected override void OnAbort()
      {
        this.listenerContext.Response.Abort();
        this.Cleanup();
      }

      protected override void OnClose(TimeSpan timeout)
      {
        base.OnClose(new System.Runtime.TimeoutHelper(timeout).RemainingTime());
        try
        {
          this.listenerContext.Response.Close();
        }
        catch (HttpListenerException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateCommunicationException(ex));
        }
      }

      void HttpRequestMessageProperty.IHttpHeaderProvider.CopyHeaders(WebHeaderCollection headers)
      {
        HttpListenerRequest request = this.listenerContext.Request;
        ((NameValueCollection) headers).Add(request.Headers);
        if (request.UserAgent == null || headers[HttpRequestHeader.UserAgent] != null)
          return;
        headers.Add(HttpRequestHeader.UserAgent, request.UserAgent);
      }

      private class ListenerContextHttpInput : HttpInput
      {
        private HttpRequestContext.ListenerHttpContext listenerHttpContext;
        private string cachedContentType;
        private byte[] preReadBuffer;

        public override long ContentLength
        {
          get
          {
            return this.listenerHttpContext.listenerContext.Request.ContentLength64;
          }
        }

        protected override string ContentTypeCore
        {
          get
          {
            if (this.cachedContentType == null)
              this.cachedContentType = this.listenerHttpContext.listenerContext.Request.ContentType;
            return this.cachedContentType;
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
            return this.listenerHttpContext.listenerContext.Request.Headers["SOAPAction"];
          }
        }

        protected override ChannelBinding ChannelBinding
        {
          get
          {
            return ChannelBindingUtility.GetToken(this.listenerHttpContext.listenerContext.Request.TransportContext);
          }
        }

        public ListenerContextHttpInput(HttpRequestContext.ListenerHttpContext listenerHttpContext)
          : base((IHttpTransportFactorySettings) listenerHttpContext.Listener, true, listenerHttpContext.listener.IsChannelBindingSupportEnabled)
        {
          this.listenerHttpContext = listenerHttpContext;
          if (this.listenerHttpContext.listenerContext.Request.ContentLength64 != -1L)
            return;
          this.preReadBuffer = new byte[1];
          if (this.listenerHttpContext.listenerContext.Request.InputStream.Read(this.preReadBuffer, 0, 1) != 0)
            return;
          this.preReadBuffer = (byte[]) null;
        }

        protected override void AddProperties(Message message)
        {
          HttpRequestMessageProperty requestMessageProperty = new HttpRequestMessageProperty((HttpRequestMessageProperty.IHttpHeaderProvider) this.listenerHttpContext);
          requestMessageProperty.Method = this.listenerHttpContext.listenerContext.Request.HttpMethod;
          if (this.listenerHttpContext.listenerContext.Request.Url.Query.Length > 1)
            requestMessageProperty.QueryString = this.listenerHttpContext.listenerContext.Request.Url.Query.Substring(1);
          message.Properties.Add(HttpRequestMessageProperty.Name, (object) requestMessageProperty);
          message.Properties.Via = this.listenerHttpContext.listenerContext.Request.Url;
          RemoteEndpointMessageProperty endpointMessageProperty = new RemoteEndpointMessageProperty(this.listenerHttpContext.listenerContext.Request.RemoteEndPoint);
          message.Properties.Add(RemoteEndpointMessageProperty.Name, (object) endpointMessageProperty);
        }

        public override void ConfigureHttpRequestMessage(HttpRequestMessage message)
        {
          message.Method = new HttpMethod(this.listenerHttpContext.listenerContext.Request.HttpMethod);
          message.RequestUri = this.listenerHttpContext.listenerContext.Request.Url;
          foreach (string header in this.listenerHttpContext.listenerContext.Request.Headers.Keys)
            HttpRequestMessageExtensionMethods.AddHeader(message, header, this.listenerHttpContext.listenerContext.Request.Headers[header]);
          message.Properties.Add(RemoteEndpointMessageProperty.Name, (object) new RemoteEndpointMessageProperty(this.listenerHttpContext.listenerContext.Request.RemoteEndPoint));
        }

        protected override Stream GetInputStream()
        {
          if (this.preReadBuffer != null)
            return (Stream) new HttpRequestContext.ListenerHttpContext.ListenerContextHttpInput.ListenerContextInputStream(this.listenerHttpContext, this.preReadBuffer);
          else
            return (Stream) new HttpRequestContext.ListenerHttpContext.ListenerContextHttpInput.ListenerContextInputStream(this.listenerHttpContext);
        }

        private class ListenerContextInputStream : HttpDelayedAcceptStream
        {
          public ListenerContextInputStream(HttpRequestContext.ListenerHttpContext listenerHttpContext)
            : base(listenerHttpContext.listenerContext.Request.InputStream)
          {
          }

          public ListenerContextInputStream(HttpRequestContext.ListenerHttpContext listenerHttpContext, byte[] preReadBuffer)
            : base((Stream) new PreReadStream(listenerHttpContext.listenerContext.Request.InputStream, preReadBuffer))
          {
          }

          public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
          {
            try
            {
              return base.BeginRead(buffer, offset, count, callback, state);
            }
            catch (HttpListenerException ex)
            {
              throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateCommunicationException(ex));
            }
          }

          public override int EndRead(IAsyncResult result)
          {
            try
            {
              return base.EndRead(result);
            }
            catch (HttpListenerException ex)
            {
              throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateCommunicationException(ex));
            }
          }

          public override int Read(byte[] buffer, int offset, int count)
          {
            try
            {
              return base.Read(buffer, offset, count);
            }
            catch (HttpListenerException ex)
            {
              throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateCommunicationException(ex));
            }
          }

          public override int ReadByte()
          {
            try
            {
              return base.ReadByte();
            }
            catch (HttpListenerException ex)
            {
              throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(HttpChannelUtilities.CreateCommunicationException(ex));
            }
          }
        }
      }
    }

    private class AcceptWebSocketAsyncResult : AsyncResult
    {
      private static AsyncCallback onHandleAcceptWebSocketResult = Fx.ThunkCallback(new AsyncCallback(HttpRequestContext.AcceptWebSocketAsyncResult.HandleAcceptWebSocketResult));
      private SignalGate gate = new SignalGate();
      private HttpRequestContext context;
      private HttpResponseMessage response;

      static AcceptWebSocketAsyncResult()
      {
      }

      public AcceptWebSocketAsyncResult(HttpRequestContext context, HttpResponseMessage response, string protocol, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.context = context;
        this.response = response;
        IAsyncResult result = System.Runtime.TaskExtensions.AsAsyncResult<WebSocketContext>(this.context.AcceptWebSocketCore(response, protocol), HttpRequestContext.AcceptWebSocketAsyncResult.onHandleAcceptWebSocketResult, (object) this);
        if (!this.gate.Unlock())
          return;
        this.CompleteAcceptWebSocket(result);
        this.Complete(true);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<HttpRequestContext.AcceptWebSocketAsyncResult>(result);
      }

      private static void HandleAcceptWebSocketResult(IAsyncResult result)
      {
        HttpRequestContext.AcceptWebSocketAsyncResult socketAsyncResult = (HttpRequestContext.AcceptWebSocketAsyncResult) result.AsyncState;
        if (!socketAsyncResult.gate.Signal())
          return;
        Exception exception = (Exception) null;
        try
        {
          socketAsyncResult.CompleteAcceptWebSocket(result);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        socketAsyncResult.Complete(false, exception);
      }

      private void CompleteAcceptWebSocket(IAsyncResult result)
      {
        Task<WebSocketContext> task = result as Task<WebSocketContext>;
        if (task.IsFaulted)
        {
          this.context.OnAcceptWebSocketError();
          throw FxTrace.Exception.AsError<WebSocketException>(task.Exception);
        }
        else if (task.IsCanceled)
        {
          this.context.OnAcceptWebSocketError();
          throw FxTrace.Exception.AsError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("AcceptWebSocketTimedOutError")));
        }
        else
        {
          this.context.SetReplySent();
          this.context.OnAcceptWebSocketSuccess(task.Result, this.response.RequestMessage);
        }
      }
    }
  }
}
