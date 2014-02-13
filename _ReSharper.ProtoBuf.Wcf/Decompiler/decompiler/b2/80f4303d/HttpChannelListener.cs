// Type: System.ServiceModel.Channels.HttpChannelListener`1
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime;
using System.Runtime.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Diagnostics.Application;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace System.ServiceModel.Channels
{
  internal class HttpChannelListener<TChannel> : HttpChannelListener, IChannelListener<TChannel>, IChannelListener, ICommunicationObject where TChannel : class, IChannel
  {
    private InputQueueChannelAcceptor<TChannel> acceptor;
    private bool useWebSocketTransport;
    private CommunicationObjectManager<ServerWebSocketTransportDuplexSessionChannel> webSocketLifetimeManager;
    private TransportIntegrationHandler transportIntegrationHandler;
    private ConnectionBufferPool bufferPool;
    private string currentWebSocketVersion;

    public override bool UseWebSocketTransport
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.useWebSocketTransport;
      }
    }

    public InputQueueChannelAcceptor<TChannel> Acceptor
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.acceptor;
      }
    }

    public override string Method
    {
      get
      {
        if (this.UseWebSocketTransport)
          return "WEBSOCKET";
        else
          return base.Method;
      }
    }

    public HttpChannelListener(HttpTransportBindingElement bindingElement, BindingContext context)
      : base(bindingElement, context)
    {
      this.useWebSocketTransport = bindingElement.WebSocketSettings.TransportUsage == WebSocketTransportUsage.Always || bindingElement.WebSocketSettings.TransportUsage == WebSocketTransportUsage.WhenDuplex && typeof (TChannel) != typeof (IReplyChannel);
      if (this.useWebSocketTransport)
      {
        if (AspNetEnvironment.Enabled)
        {
          AspNetEnvironment current = AspNetEnvironment.Current;
          if (!current.UsingIntegratedPipeline)
            throw System.ServiceModel.FxTrace.Exception.AsError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("WebSocketsNotSupportedInClassicPipeline")));
          if (!current.IsWebSocketModuleLoaded)
            throw System.ServiceModel.FxTrace.Exception.AsError((Exception) new NotSupportedException(System.ServiceModel.SR.GetString("WebSocketModuleNotLoaded")));
        }
        else if (!WebSocketHelper.OSSupportsWebSockets())
          throw System.ServiceModel.FxTrace.Exception.AsError((Exception) new PlatformNotSupportedException(System.ServiceModel.SR.GetString("WebSocketsServerSideNotSupported")));
        this.currentWebSocketVersion = WebSocketHelper.GetCurrentVersion();
        this.acceptor = new InputQueueChannelAcceptor<TChannel>((ChannelManagerBase) this);
        this.bufferPool = new ConnectionBufferPool(WebSocketHelper.ComputeServerBufferSize(bindingElement.MaxReceivedMessageSize));
        this.webSocketLifetimeManager = new CommunicationObjectManager<ServerWebSocketTransportDuplexSessionChannel>(this.ThisLock);
      }
      else
        this.acceptor = (InputQueueChannelAcceptor<TChannel>) new TransportReplyChannelAcceptor((TransportChannelListener) this);
      this.CreatePipeline(bindingElement.MessageHandlerFactory);
    }

    public TChannel AcceptChannel()
    {
      return this.AcceptChannel(this.DefaultReceiveTimeout);
    }

    public IAsyncResult BeginAcceptChannel(AsyncCallback callback, object state)
    {
      return this.BeginAcceptChannel(this.DefaultReceiveTimeout, callback, state);
    }

    public TChannel AcceptChannel(TimeSpan timeout)
    {
      this.ThrowIfNotOpened();
      return this.Acceptor.AcceptChannel(timeout);
    }

    public IAsyncResult BeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.ThrowIfNotOpened();
      return this.Acceptor.BeginAcceptChannel(timeout, callback, state);
    }

    public TChannel EndAcceptChannel(IAsyncResult result)
    {
      this.ThrowPending();
      return this.Acceptor.EndAcceptChannel(result);
    }

    public override bool CreateWebSocketChannelAndEnqueue(HttpRequestContext httpRequestContext, HttpPipeline pipeline, HttpResponseMessage httpResponseMessage, string subProtocol, Action dequeuedCallback)
    {
      if (this.Acceptor.PendingCount >= this.WebSocketSettings.MaxPendingConnections)
      {
        if (System.ServiceModel.Diagnostics.Application.TD.MaxPendingConnectionsExceededIsEnabled())
          System.ServiceModel.Diagnostics.Application.TD.MaxPendingConnectionsExceeded(System.ServiceModel.SR.GetString("WebSocketMaxPendingConnectionsReached", (object) this.WebSocketSettings.MaxPendingConnections, (object) "MaxPendingConnections", (object) "WebSocketTransportSettings"));
        if (DiagnosticUtility.ShouldTraceWarning)
          TraceUtility.TraceEvent(TraceEventType.Warning, 262180, System.ServiceModel.SR.GetString("WebSocketMaxPendingConnectionsReached", (object) this.WebSocketSettings.MaxPendingConnections, (object) "MaxPendingConnections", (object) "WebSocketTransportSettings"), (TraceRecord) new StringTraceRecord("MaxPendingConnections", this.WebSocketSettings.MaxPendingConnections.ToString((IFormatProvider) CultureInfo.InvariantCulture)), (object) this, (Exception) null);
        return false;
      }
      else
      {
        ServerWebSocketTransportDuplexSessionChannel duplexSessionChannel = new ServerWebSocketTransportDuplexSessionChannel((HttpChannelListener) this, new EndpointAddress(this.Uri, new AddressHeader[0]), this.Uri, this.bufferPool, httpRequestContext, pipeline, httpResponseMessage, subProtocol);
        httpRequestContext.WebSocketChannel = duplexSessionChannel;
        this.webSocketLifetimeManager.Add(duplexSessionChannel);
        this.Acceptor.EnqueueAndDispatch((TChannel) duplexSessionChannel, dequeuedCallback, true);
        return true;
      }
    }

    public override byte[] TakeWebSocketInternalBuffer()
    {
      return this.bufferPool.Take();
    }

    public override void ReturnWebSocketInternalBuffer(byte[] buffer)
    {
      this.bufferPool.Return(buffer);
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new ChainedOpenAsyncResult(timeout, callback, state, new ChainedBeginHandler(((TransportChannelListener) this).OnBeginOpen), new ChainedEndHandler(((TransportChannelListener) this).OnEndOpen), new ICommunicationObject[1]
      {
        (ICommunicationObject) this.Acceptor
      });
    }

    protected override void OnOpen(TimeSpan timeout)
    {
      TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
      base.OnOpen(timeoutHelper.RemainingTime());
      this.Acceptor.Open(timeoutHelper.RemainingTime());
    }

    protected override void OnEndOpen(IAsyncResult result)
    {
      ChainedAsyncResult.End(result);
    }

    protected override void OnClose(TimeSpan timeout)
    {
      TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
      this.Acceptor.Close(timeoutHelper.RemainingTime());
      if (this.IsAuthenticationSupported)
        this.CloseUserNameTokenAuthenticator(timeoutHelper.RemainingTime());
      if (this.useWebSocketTransport)
        this.webSocketLifetimeManager.Close(timeoutHelper.RemainingTime());
      base.OnClose(timeoutHelper.RemainingTime());
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
      ICommunicationObject communicationObject = this.UserNameTokenAuthenticator as ICommunicationObject;
      ICommunicationObject[] communicationObjects;
      if (communicationObject == null)
      {
        if (this.IsAuthenticationSupported)
          this.CloseUserNameTokenAuthenticator(timeoutHelper.RemainingTime());
        communicationObjects = new ICommunicationObject[1]
        {
          (ICommunicationObject) this.Acceptor
        };
      }
      else
        communicationObjects = new ICommunicationObject[2]
        {
          (ICommunicationObject) this.Acceptor,
          communicationObject
        };
      if (this.useWebSocketTransport)
        return (IAsyncResult) new HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<ServerWebSocketTransportDuplexSessionChannel>(timeoutHelper.RemainingTime(), callback, state, this.webSocketLifetimeManager, new ChainedBeginHandler(((TransportChannelListener) this).OnBeginClose), new ChainedEndHandler(((TransportChannelListener) this).OnEndClose), communicationObjects);
      else
        return (IAsyncResult) new ChainedCloseAsyncResult(timeoutHelper.RemainingTime(), callback, state, new ChainedBeginHandler(((TransportChannelListener) this).OnBeginClose), new ChainedEndHandler(((TransportChannelListener) this).OnEndClose), communicationObjects);
    }

    protected override void OnEndClose(IAsyncResult result)
    {
      if (this.useWebSocketTransport)
        HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<ServerWebSocketTransportDuplexSessionChannel>.End(result);
      else
        ChainedAsyncResult.End(result);
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      if (this.bufferPool != null)
        this.bufferPool.Close();
      if (this.transportIntegrationHandler == null)
        return;
      this.transportIntegrationHandler.Dispose();
    }

    protected override void OnAbort()
    {
      if (this.IsAuthenticationSupported)
        this.AbortUserNameTokenAuthenticator();
      this.Acceptor.Abort();
      if (this.useWebSocketTransport)
        this.webSocketLifetimeManager.Abort();
      base.OnAbort();
    }

    protected override bool OnWaitForChannel(TimeSpan timeout)
    {
      return this.Acceptor.WaitForChannel(timeout);
    }

    protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return this.Acceptor.BeginWaitForChannel(timeout, callback, state);
    }

    protected override bool OnEndWaitForChannel(IAsyncResult result)
    {
      return this.Acceptor.EndWaitForChannel(result);
    }

    internal override IAsyncResult BeginHttpContextReceived(HttpRequestContext context, Action acceptorCallback, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TChannel>(context, acceptorCallback, this, callback, state);
    }

    internal override bool EndHttpContextReceived(IAsyncResult result)
    {
      return HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TChannel>.End(result);
    }

    private void CreatePipeline(HttpMessageHandlerFactory httpMessageHandlerFactory)
    {
      HttpMessageHandler innerChannel;
      if (this.UseWebSocketTransport)
      {
        innerChannel = (HttpMessageHandler) new DefaultWebSocketConnectionHandler(this.WebSocketSettings.SubProtocol, this.currentWebSocketVersion, this.MessageVersion, this.MessageEncoderFactory, this.TransferMode);
        if (httpMessageHandlerFactory != null)
          innerChannel = httpMessageHandlerFactory.Create(innerChannel);
      }
      else
      {
        if (httpMessageHandlerFactory == null)
          return;
        innerChannel = httpMessageHandlerFactory.Create((HttpMessageHandler) new ChannelModelIntegrationHandler());
      }
      if (innerChannel == null)
        throw System.ServiceModel.FxTrace.Exception.AsError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("HttpMessageHandlerChannelFactoryNullPipeline", (object) httpMessageHandlerFactory.GetType().Name, (object) typeof (HttpRequestContext).Name)));
      else
        this.transportIntegrationHandler = new TransportIntegrationHandler(innerChannel);
    }

    private static void HandleProcessInboundException(Exception ex, HttpRequestContext context)
    {
      if (Fx.IsFatal(ex))
        return;
      if (ex is ProtocolException)
      {
        ProtocolException protocolException = (ProtocolException) ex;
        HttpStatusCode statusCode = HttpStatusCode.BadRequest;
        string statusDescription = string.Empty;
        if (protocolException.Data.Contains((object) "System.ServiceModel.Channels.HttpInput.HttpStatusCode"))
        {
          statusCode = (HttpStatusCode) protocolException.Data[(object) "System.ServiceModel.Channels.HttpInput.HttpStatusCode"];
          protocolException.Data.Remove((object) "System.ServiceModel.Channels.HttpInput.HttpStatusCode");
        }
        if (protocolException.Data.Contains((object) "System.ServiceModel.Channels.HttpInput.HttpStatusDescription"))
        {
          statusDescription = (string) protocolException.Data[(object) "System.ServiceModel.Channels.HttpInput.HttpStatusDescription"];
          protocolException.Data.Remove((object) "System.ServiceModel.Channels.HttpInput.HttpStatusDescription");
        }
        context.SendResponseAndClose(statusCode, statusDescription);
      }
      else
      {
        try
        {
          context.SendResponseAndClose(HttpStatusCode.BadRequest);
        }
        catch (Exception ex1)
        {
          if (Fx.IsFatal(ex1))
            throw;
          else
            DiagnosticUtility.TraceHandledException(ex1, TraceEventType.Error);
        }
      }
    }

    private static bool ContextReceiveExceptionHandled(Exception e)
    {
      if (Fx.IsFatal(e))
        return false;
      if (e is CommunicationException)
        DiagnosticUtility.TraceHandledException(e, TraceEventType.Information);
      else if (e is XmlException)
        DiagnosticUtility.TraceHandledException(e, TraceEventType.Information);
      else if (e is IOException)
        DiagnosticUtility.TraceHandledException(e, TraceEventType.Information);
      else if (e is TimeoutException)
      {
        if (System.ServiceModel.Diagnostics.Application.TD.ReceiveTimeoutIsEnabled())
          System.ServiceModel.Diagnostics.Application.TD.ReceiveTimeout(e.Message);
        DiagnosticUtility.TraceHandledException(e, TraceEventType.Information);
      }
      else if (e is OperationCanceledException)
        DiagnosticUtility.TraceHandledException(e, TraceEventType.Information);
      else if (!System.ServiceModel.Dispatcher.ExceptionHandler.HandleTransportExceptionHelper(e))
        return false;
      return true;
    }

    private class HttpContextReceivedAsyncResult<TListenerChannel> : TraceAsyncResult where TListenerChannel : class, IChannel
    {
      private static AsyncCallback onProcessInboundRequest = Fx.ThunkCallback(new AsyncCallback(HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TListenerChannel>.OnProcessInboundRequest));
      private bool enqueued;
      private HttpRequestContext context;
      private Action acceptorCallback;
      private HttpChannelListener<TListenerChannel> listener;

      static HttpContextReceivedAsyncResult()
      {
      }

      public HttpContextReceivedAsyncResult(HttpRequestContext requestContext, Action acceptorCallback, HttpChannelListener<TListenerChannel> listener, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.context = requestContext;
        this.acceptorCallback = acceptorCallback;
        this.listener = listener;
        if (this.ProcessHttpContextAsync() != AsyncCompletionResult.Completed)
          return;
        this.Complete(true);
      }

      public static bool End(IAsyncResult result)
      {
        return AsyncResult.End<HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TListenerChannel>>(result).enqueued;
      }

      private static void OnProcessInboundRequest(IAsyncResult result)
      {
        if (result.CompletedSynchronously)
          return;
        HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TListenerChannel> receivedAsyncResult = (HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TListenerChannel>) result.AsyncState;
        Exception exception = (Exception) null;
        try
        {
          receivedAsyncResult.HandleProcessInboundRequest(result);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        receivedAsyncResult.Complete(false, exception);
      }

      private AsyncCompletionResult ProcessHttpContextAsync()
      {
        bool flag = false;
        try
        {
          this.context.InitializeHttpPipeline(this.listener.transportIntegrationHandler);
          if (!this.Authenticate())
            return AsyncCompletionResult.Completed;
          if (this.listener.UseWebSocketTransport && !this.context.IsWebSocketRequest)
          {
            this.context.SendResponseAndClose(HttpStatusCode.BadRequest, System.ServiceModel.SR.GetString("WebSocketEndpointOnlySupportWebSocketError"));
            return AsyncCompletionResult.Completed;
          }
          else
          {
            if (!this.listener.UseWebSocketTransport)
            {
              if (this.context.IsWebSocketRequest)
              {
                this.context.SendResponseAndClose(HttpStatusCode.BadRequest, System.ServiceModel.SR.GetString("WebSocketEndpointDoesNotSupportWebSocketError"));
                return AsyncCompletionResult.Completed;
              }
            }
            try
            {
              IAsyncResult result = this.context.BeginProcessInboundRequest(this.listener.Acceptor as ReplyChannelAcceptor, this.acceptorCallback, HttpChannelListener<TChannel>.HttpContextReceivedAsyncResult<TListenerChannel>.onProcessInboundRequest, (object) this);
              if (result.CompletedSynchronously)
              {
                this.EndInboundProcessAndEnqueue(result);
                return AsyncCompletionResult.Completed;
              }
            }
            catch (Exception ex)
            {
              HttpChannelListener<TChannel>.HandleProcessInboundException(ex, this.context);
              throw;
            }
          }
        }
        catch (Exception ex)
        {
          flag = true;
          if (!HttpChannelListener<TChannel>.ContextReceiveExceptionHandled(ex))
            throw;
        }
        finally
        {
          if (flag)
            this.context.Abort();
        }
        return !flag ? AsyncCompletionResult.Queued : AsyncCompletionResult.Completed;
      }

      private bool Authenticate()
      {
        if (this.context.ProcessAuthentication())
          return true;
        if (System.ServiceModel.Diagnostics.Application.TD.HttpAuthFailedIsEnabled())
          System.ServiceModel.Diagnostics.Application.TD.HttpAuthFailed(this.context.EventTraceActivity);
        if (DiagnosticUtility.ShouldTraceInformation)
          TraceUtility.TraceEvent(TraceEventType.Information, 262183, System.ServiceModel.SR.GetString("TraceCodeHttpAuthFailed"), (object) this);
        return false;
      }

      private void HandleProcessInboundRequest(IAsyncResult result)
      {
        bool flag = true;
        try
        {
          try
          {
            this.EndInboundProcessAndEnqueue(result);
            flag = false;
          }
          catch (Exception ex)
          {
            HttpChannelListener<TChannel>.HandleProcessInboundException(ex, this.context);
            throw;
          }
        }
        catch (Exception ex)
        {
          if (HttpChannelListener<TChannel>.ContextReceiveExceptionHandled(ex))
            return;
          throw;
        }
        finally
        {
          if (flag)
            this.context.Abort();
        }
      }

      private void EndInboundProcessAndEnqueue(IAsyncResult result)
      {
        this.context.EndProcessInboundRequest(result);
        this.enqueued = true;
      }
    }

    private class LifetimeWrappedCloseAsyncResult<TCommunicationObject> : AsyncResult where TCommunicationObject : CommunicationObject
    {
      private static AsyncResult.AsyncCompletion handleLifetimeManagerClose = new AsyncResult.AsyncCompletion(HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>.HandleLifetimeManagerClose);
      private static AsyncResult.AsyncCompletion handleChannelClose = new AsyncResult.AsyncCompletion(HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>.HandleChannelClose);
      private TimeoutHelper timeoutHelper;
      private ICommunicationObject[] communicationObjects;
      private CommunicationObjectManager<TCommunicationObject> communicationObjectManager;
      private ChainedBeginHandler begin1;
      private ChainedEndHandler end1;

      static LifetimeWrappedCloseAsyncResult()
      {
      }

      public LifetimeWrappedCloseAsyncResult(TimeSpan timeout, AsyncCallback callback, object state, CommunicationObjectManager<TCommunicationObject> communicationObjectManager, ChainedBeginHandler begin1, ChainedEndHandler end1, ICommunicationObject[] communicationObjects)
        : base(callback, state)
      {
        this.timeoutHelper = new TimeoutHelper(timeout);
        this.begin1 = begin1;
        this.end1 = end1;
        this.communicationObjects = communicationObjects;
        this.communicationObjectManager = communicationObjectManager;
        if (!this.SyncContinue(communicationObjectManager.BeginClose(this.timeoutHelper.RemainingTime(), this.PrepareAsyncCompletion(HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>.handleLifetimeManagerClose), (object) this)))
          return;
        this.Complete(true);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>>(result);
      }

      private static bool HandleLifetimeManagerClose(IAsyncResult result)
      {
        HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject> closeAsyncResult1 = (HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>) result.AsyncState;
        closeAsyncResult1.communicationObjectManager.EndClose(result);
        ChainedCloseAsyncResult closeAsyncResult2 = new ChainedCloseAsyncResult(closeAsyncResult1.timeoutHelper.RemainingTime(), closeAsyncResult1.PrepareAsyncCompletion(HttpChannelListener<TChannel>.LifetimeWrappedCloseAsyncResult<TCommunicationObject>.handleChannelClose), (object) closeAsyncResult1, closeAsyncResult1.begin1, closeAsyncResult1.end1, closeAsyncResult1.communicationObjects);
        return closeAsyncResult1.SyncContinue((IAsyncResult) closeAsyncResult2);
      }

      private static bool HandleChannelClose(IAsyncResult result)
      {
        ChainedAsyncResult.End(result);
        return true;
      }
    }
  }
}
