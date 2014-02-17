// Type: System.ServiceModel.Channels.ReplyChannel
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal class ReplyChannel : InputQueueChannel<RequestContext>, IReplyChannel, IChannel, ICommunicationObject
  {
    private EndpointAddress localAddress;

    public EndpointAddress LocalAddress
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.localAddress;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ReplyChannel(ChannelManagerBase channelManager, EndpointAddress localAddress)
      : base(channelManager)
    {
      this.localAddress = localAddress;
    }

    public override T GetProperty<T>()
    {
      if (typeof (T) == typeof (IReplyChannel))
        return (T) this;
      T property = base.GetProperty<T>();
      if ((object) property != null)
        return property;
      else
        return default (T);
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new CompletedAsyncResult(callback, state);
    }

    protected override void OnEndOpen(IAsyncResult result)
    {
      CompletedAsyncResult.End(result);
    }

    protected override void OnOpen(TimeSpan timeout)
    {
    }

    internal static RequestContext HelpReceiveRequest(IReplyChannel channel, TimeSpan timeout)
    {
      RequestContext context;
      if (channel.TryReceiveRequest(timeout, out context))
        return context;
      else
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(ReplyChannel.CreateReceiveRequestTimedOutException(channel, timeout));
    }

    internal static IAsyncResult HelpBeginReceiveRequest(IReplyChannel channel, TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new ReplyChannel.HelpReceiveRequestAsyncResult(channel, timeout, callback, state);
    }

    internal static RequestContext HelpEndReceiveRequest(IAsyncResult result)
    {
      return ReplyChannel.HelpReceiveRequestAsyncResult.End(result);
    }

    public RequestContext ReceiveRequest()
    {
      return this.ReceiveRequest(this.DefaultReceiveTimeout);
    }

    public RequestContext ReceiveRequest(TimeSpan timeout)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return ReplyChannel.HelpReceiveRequest((IReplyChannel) this, timeout);
    }

    public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
    {
      return this.BeginReceiveRequest(this.DefaultReceiveTimeout, callback, state);
    }

    public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return ReplyChannel.HelpBeginReceiveRequest((IReplyChannel) this, timeout, callback, state);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public RequestContext EndReceiveRequest(IAsyncResult result)
    {
      return ReplyChannel.HelpEndReceiveRequest(result);
    }

    public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return this.Dequeue(timeout, out context);
    }

    public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return this.BeginDequeue(timeout, callback, state);
    }

    public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
    {
      return this.EndDequeue(result, out context);
    }

    public bool WaitForRequest(TimeSpan timeout)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return this.WaitForItem(timeout);
    }

    public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowPending();
      return this.BeginWaitForItem(timeout, callback, state);
    }

    public bool EndWaitForRequest(IAsyncResult result)
    {
      return this.EndWaitForItem(result);
    }

    private static Exception CreateReceiveRequestTimedOutException(IReplyChannel channel, TimeSpan timeout)
    {
      if (channel.LocalAddress != (EndpointAddress) null)
        return (Exception) new TimeoutException(System.ServiceModel.SR.GetString("ReceiveRequestTimedOut", (object) channel.LocalAddress.Uri.AbsoluteUri, (object) timeout));
      else
        return (Exception) new TimeoutException(System.ServiceModel.SR.GetString("ReceiveRequestTimedOutNoLocalAddress", new object[1]
        {
          (object) timeout
        }));
    }

    private class HelpReceiveRequestAsyncResult : AsyncResult
    {
      private static AsyncCallback onReceiveRequest = Fx.ThunkCallback(new AsyncCallback(ReplyChannel.HelpReceiveRequestAsyncResult.OnReceiveRequest));
      private IReplyChannel channel;
      private TimeSpan timeout;
      private RequestContext requestContext;

      static HelpReceiveRequestAsyncResult()
      {
      }

      public HelpReceiveRequestAsyncResult(IReplyChannel channel, TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.channel = channel;
        this.timeout = timeout;
        IAsyncResult result = channel.BeginTryReceiveRequest(timeout, ReplyChannel.HelpReceiveRequestAsyncResult.onReceiveRequest, (object) this);
        if (!result.CompletedSynchronously)
          return;
        this.HandleReceiveRequestComplete(result);
        this.Complete(true);
      }

      public static RequestContext End(IAsyncResult result)
      {
        return AsyncResult.End<ReplyChannel.HelpReceiveRequestAsyncResult>(result).requestContext;
      }

      private void HandleReceiveRequestComplete(IAsyncResult result)
      {
        if (!this.channel.EndTryReceiveRequest(result, out this.requestContext))
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(ReplyChannel.CreateReceiveRequestTimedOutException(this.channel, this.timeout));
      }

      private static void OnReceiveRequest(IAsyncResult result)
      {
        if (result.CompletedSynchronously)
          return;
        ReplyChannel.HelpReceiveRequestAsyncResult requestAsyncResult = (ReplyChannel.HelpReceiveRequestAsyncResult) result.AsyncState;
        Exception exception = (Exception) null;
        try
        {
          requestAsyncResult.HandleReceiveRequestComplete(result);
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
}
