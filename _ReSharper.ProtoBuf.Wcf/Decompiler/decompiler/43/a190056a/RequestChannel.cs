// Type: System.ServiceModel.Channels.RequestChannel
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;
using System.Threading;

namespace System.ServiceModel.Channels
{
  internal abstract class RequestChannel : ChannelBase, IRequestChannel, IChannel, ICommunicationObject
  {
    private List<IRequestBase> outstandingRequests = new List<IRequestBase>();
    private bool manualAddressing;
    private EndpointAddress to;
    private Uri via;
    private ManualResetEvent closedEvent;
    private bool closed;

    protected bool ManualAddressing
    {
      get
      {
        return this.manualAddressing;
      }
    }

    public EndpointAddress RemoteAddress
    {
      get
      {
        return this.to;
      }
    }

    public Uri Via
    {
      get
      {
        return this.via;
      }
    }

    protected RequestChannel(ChannelManagerBase channelFactory, EndpointAddress to, Uri via, bool manualAddressing)
      : base(channelFactory)
    {
      if (!manualAddressing && to == (EndpointAddress) null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("to");
      this.manualAddressing = manualAddressing;
      this.to = to;
      this.via = via;
    }

    protected void AbortPendingRequests()
    {
      IRequestBase[] requestBaseArray = this.CopyPendingRequests(false);
      if (requestBaseArray == null)
        return;
      foreach (IRequestBase requestBase in requestBaseArray)
        requestBase.Abort(this);
    }

    protected IAsyncResult BeginWaitForPendingRequests(TimeSpan timeout, AsyncCallback callback, object state)
    {
      IRequestBase[] pendingRequests = this.SetupWaitForPendingRequests();
      return (IAsyncResult) new RequestChannel.WaitForPendingRequestsAsyncResult(timeout, this, pendingRequests, callback, state);
    }

    protected void EndWaitForPendingRequests(IAsyncResult result)
    {
      RequestChannel.WaitForPendingRequestsAsyncResult.End(result);
    }

    private void FinishClose()
    {
      lock (this.outstandingRequests)
      {
        if (this.closed)
          return;
        this.closed = true;
        if (this.closedEvent == null)
          return;
        this.closedEvent.Close();
      }
    }

    private IRequestBase[] SetupWaitForPendingRequests()
    {
      return this.CopyPendingRequests(true);
    }

    protected void WaitForPendingRequests(TimeSpan timeout)
    {
      IRequestBase[] requestBaseArray = this.SetupWaitForPendingRequests();
      if (requestBaseArray != null && !this.closedEvent.WaitOne(timeout, false))
      {
        foreach (IRequestBase requestBase in requestBaseArray)
          requestBase.Abort(this);
      }
      this.FinishClose();
    }

    private IRequestBase[] CopyPendingRequests(bool createEventIfNecessary)
    {
      IRequestBase[] array = (IRequestBase[]) null;
      lock (this.outstandingRequests)
      {
        if (this.outstandingRequests.Count > 0)
        {
          array = new IRequestBase[this.outstandingRequests.Count];
          this.outstandingRequests.CopyTo(array);
          this.outstandingRequests.Clear();
          if (createEventIfNecessary)
          {
            if (this.closedEvent == null)
              this.closedEvent = new ManualResetEvent(false);
          }
        }
      }
      return array;
    }

    protected void FaultPendingRequests()
    {
      IRequestBase[] requestBaseArray = this.CopyPendingRequests(false);
      if (requestBaseArray == null)
        return;
      foreach (IRequestBase requestBase in requestBaseArray)
        requestBase.Fault(this);
    }

    public override T GetProperty<T>()
    {
      if (typeof (T) == typeof (IRequestChannel))
        return (T) this;
      T property = base.GetProperty<T>();
      if ((object) property != null)
        return property;
      else
        return default (T);
    }

    protected override void OnAbort()
    {
      this.AbortPendingRequests();
    }

    private void ReleaseRequest(IRequestBase request)
    {
      lock (this.outstandingRequests)
      {
        this.outstandingRequests.Remove(request);
        if (this.outstandingRequests.Count != 0 || this.closed || this.closedEvent == null)
          return;
        this.closedEvent.Set();
      }
    }

    private void TrackRequest(IRequestBase request)
    {
      lock (this.outstandingRequests)
      {
        this.ThrowIfDisposedOrNotOpen();
        this.outstandingRequests.Add(request);
      }
    }

    public IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state)
    {
      return this.BeginRequest(message, this.DefaultSendTimeout, callback, state);
    }

    public IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state)
    {
      if (message == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("message");
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowIfDisposedOrNotOpen();
      this.AddHeadersTo(message);
      IAsyncRequest asyncRequest = this.CreateAsyncRequest(message, callback, state);
      this.TrackRequest((IRequestBase) asyncRequest);
      bool flag = true;
      try
      {
        asyncRequest.BeginSendRequest(message, timeout);
        flag = false;
      }
      finally
      {
        if (flag)
          this.ReleaseRequest((IRequestBase) asyncRequest);
      }
      return (IAsyncResult) asyncRequest;
    }

    protected abstract IRequest CreateRequest(Message message);

    protected abstract IAsyncRequest CreateAsyncRequest(Message message, AsyncCallback callback, object state);

    public Message EndRequest(IAsyncResult result)
    {
      if (result == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("result");
      IAsyncRequest asyncRequest = result as IAsyncRequest;
      if (asyncRequest == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("result", System.ServiceModel.SR.GetString("InvalidAsyncResult"));
      try
      {
        Message message = asyncRequest.End();
        if (DiagnosticUtility.ShouldTraceInformation)
          TraceUtility.TraceEvent(TraceEventType.Information, TraceCode.RequestChannelReplyReceived, message);
        return message;
      }
      finally
      {
        this.ReleaseRequest((IRequestBase) asyncRequest);
      }
    }

    public Message Request(Message message)
    {
      return this.Request(message, this.DefaultSendTimeout);
    }

    public Message Request(Message message, TimeSpan timeout)
    {
      if (message == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("message");
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("SFxTimeoutOutOfRange0")));
      this.ThrowIfDisposedOrNotOpen();
      this.AddHeadersTo(message);
      IRequest request = this.CreateRequest(message);
      this.TrackRequest((IRequestBase) request);
      try
      {
        TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
        TimeSpan timeout1 = timeoutHelper.RemainingTime();
        try
        {
          request.SendRequest(message, timeout1);
        }
        catch (TimeoutException ex)
        {
          throw TraceUtility.ThrowHelperError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("RequestChannelSendTimedOut", new object[1]
          {
            (object) timeout1
          }), (Exception) ex), message);
        }
        TimeSpan timeout2 = timeoutHelper.RemainingTime();
        Message message1;
        try
        {
          message1 = request.WaitForReply(timeout2);
        }
        catch (TimeoutException ex)
        {
          throw TraceUtility.ThrowHelperError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("RequestChannelWaitForReplyTimedOut", new object[1]
          {
            (object) timeout2
          }), (Exception) ex), message);
        }
        if (DiagnosticUtility.ShouldTraceInformation)
          TraceUtility.TraceEvent(TraceEventType.Information, TraceCode.RequestChannelReplyReceived, message1);
        return message1;
      }
      finally
      {
        this.ReleaseRequest((IRequestBase) request);
      }
    }

    protected virtual void AddHeadersTo(Message message)
    {
      if (this.manualAddressing || !(this.to != (EndpointAddress) null))
        return;
      this.to.ApplyTo(message);
    }

    private class WaitForPendingRequestsAsyncResult : AsyncResult
    {
      private static WaitOrTimerCallback completeWaitCallBack = new WaitOrTimerCallback(RequestChannel.WaitForPendingRequestsAsyncResult.OnCompleteWaitCallBack);
      private IRequestBase[] pendingRequests;
      private RequestChannel requestChannel;
      private TimeSpan timeout;
      private RegisteredWaitHandle waitHandle;

      static WaitForPendingRequestsAsyncResult()
      {
      }

      public WaitForPendingRequestsAsyncResult(TimeSpan timeout, RequestChannel requestChannel, IRequestBase[] pendingRequests, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.requestChannel = requestChannel;
        this.pendingRequests = pendingRequests;
        this.timeout = timeout;
        if (this.timeout == TimeSpan.Zero || this.pendingRequests == null)
        {
          this.AbortRequests();
          this.CleanupEvents();
          this.Complete(true);
        }
        else
          this.waitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject((WaitHandle) this.requestChannel.closedEvent, RequestChannel.WaitForPendingRequestsAsyncResult.completeWaitCallBack, (object) this, TimeoutHelper.ToMilliseconds(timeout), true);
      }

      private void AbortRequests()
      {
        if (this.pendingRequests == null)
          return;
        foreach (IRequestBase requestBase in this.pendingRequests)
          requestBase.Abort(this.requestChannel);
      }

      private void CleanupEvents()
      {
        if (this.requestChannel.closedEvent == null)
          return;
        if (this.waitHandle != null)
          this.waitHandle.Unregister((WaitHandle) this.requestChannel.closedEvent);
        this.requestChannel.FinishClose();
      }

      private static void OnCompleteWaitCallBack(object state, bool timedOut)
      {
        RequestChannel.WaitForPendingRequestsAsyncResult requestsAsyncResult = (RequestChannel.WaitForPendingRequestsAsyncResult) state;
        Exception exception = (Exception) null;
        try
        {
          if (timedOut)
            requestsAsyncResult.AbortRequests();
          requestsAsyncResult.CleanupEvents();
        }
        catch (Exception ex)
        {
          if (DiagnosticUtility.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        requestsAsyncResult.Complete(false, exception);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<RequestChannel.WaitForPendingRequestsAsyncResult>(result);
      }
    }
  }
}
