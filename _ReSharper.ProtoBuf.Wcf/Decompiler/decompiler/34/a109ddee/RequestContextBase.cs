// Type: System.ServiceModel.Channels.RequestContextBase
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Diagnostics;
using System.Runtime;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;

namespace System.ServiceModel.Channels
{
  internal abstract class RequestContextBase : RequestContext
  {
    private CommunicationState state = CommunicationState.Opened;
    private object thisLock = new object();
    private TimeSpan defaultSendTimeout;
    private TimeSpan defaultCloseTimeout;
    private Message requestMessage;
    private Exception requestMessageException;
    private bool replySent;
    private bool replyInitiated;
    private bool aborted;

    public override Message RequestMessage
    {
      get
      {
        if (this.requestMessageException != null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.requestMessageException);
        else
          return this.requestMessage;
      }
    }

    protected bool ReplyInitiated
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.replyInitiated;
      }
    }

    protected object ThisLock
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.thisLock;
      }
    }

    public bool Aborted
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.aborted;
      }
    }

    public TimeSpan DefaultCloseTimeout
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.defaultCloseTimeout;
      }
    }

    public TimeSpan DefaultSendTimeout
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.defaultSendTimeout;
      }
    }

    protected RequestContextBase(Message requestMessage, TimeSpan defaultCloseTimeout, TimeSpan defaultSendTimeout)
    {
      this.defaultSendTimeout = defaultSendTimeout;
      this.defaultCloseTimeout = defaultCloseTimeout;
      this.requestMessage = requestMessage;
    }

    public void ReInitialize(Message requestMessage)
    {
      this.state = CommunicationState.Opened;
      this.requestMessageException = (Exception) null;
      this.replySent = false;
      this.replyInitiated = false;
      this.aborted = false;
      this.requestMessage = requestMessage;
    }

    protected void SetRequestMessage(Message requestMessage)
    {
      this.requestMessage = requestMessage;
    }

    protected void SetRequestMessage(Exception requestMessageException)
    {
      this.requestMessageException = requestMessageException;
    }

    public override void Abort()
    {
      lock (this.ThisLock)
      {
        if (this.state == CommunicationState.Closed)
          return;
        this.state = CommunicationState.Closing;
        this.aborted = true;
      }
      if (DiagnosticUtility.ShouldTraceWarning)
        TraceUtility.TraceEvent(TraceEventType.Warning, 262174, System.ServiceModel.SR.GetString("TraceCodeRequestContextAbort"), (object) this);
      try
      {
        this.OnAbort();
      }
      finally
      {
        this.state = CommunicationState.Closed;
      }
    }

    public override void Close()
    {
      this.Close(this.defaultCloseTimeout);
    }

    public override void Close(TimeSpan timeout)
    {
      if (timeout < TimeSpan.Zero)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("timeout", (object) timeout, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")));
      bool flag1 = false;
      lock (this.ThisLock)
      {
        if (this.state != CommunicationState.Opened)
          return;
        if (this.TryInitiateReply())
          flag1 = true;
        this.state = CommunicationState.Closing;
      }
      TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
      bool flag2 = true;
      try
      {
        if (flag1)
          this.OnReply((Message) null, timeoutHelper.RemainingTime());
        this.OnClose(timeoutHelper.RemainingTime());
        this.state = CommunicationState.Closed;
        flag2 = false;
      }
      finally
      {
        if (flag2)
          this.Abort();
      }
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      if (!disposing)
        return;
      if (this.replySent)
        this.Close();
      else
        this.Abort();
    }

    protected abstract void OnAbort();

    protected abstract void OnClose(TimeSpan timeout);

    protected abstract void OnReply(Message message, TimeSpan timeout);

    protected abstract IAsyncResult OnBeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state);

    protected abstract void OnEndReply(IAsyncResult result);

    protected void ThrowIfInvalidReply()
    {
      if (this.state == CommunicationState.Closed || this.state == CommunicationState.Closing)
      {
        if (this.aborted)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new CommunicationObjectAbortedException(System.ServiceModel.SR.GetString("RequestContextAborted")));
        else
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ObjectDisposedException(this.GetType().FullName));
      }
      else if (this.replyInitiated)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("ReplyAlreadySent")));
    }

    protected bool TryInitiateReply()
    {
      lock (this.thisLock)
      {
        if (this.state != CommunicationState.Opened || this.replyInitiated)
          return false;
        this.replyInitiated = true;
        return true;
      }
    }

    public override IAsyncResult BeginReply(Message message, AsyncCallback callback, object state)
    {
      return this.BeginReply(message, this.defaultSendTimeout, callback, state);
    }

    public override IAsyncResult BeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state)
    {
      lock (this.thisLock)
      {
        this.ThrowIfInvalidReply();
        this.replyInitiated = true;
      }
      return this.OnBeginReply(message, timeout, callback, state);
    }

    public override void EndReply(IAsyncResult result)
    {
      this.OnEndReply(result);
      this.replySent = true;
    }

    public override void Reply(Message message)
    {
      this.Reply(message, this.defaultSendTimeout);
    }

    public override void Reply(Message message, TimeSpan timeout)
    {
      lock (this.thisLock)
      {
        this.ThrowIfInvalidReply();
        this.replyInitiated = true;
      }
      this.OnReply(message, timeout);
      this.replySent = true;
    }

    protected void SetReplySent()
    {
      lock (this.thisLock)
      {
        this.ThrowIfInvalidReply();
        this.replyInitiated = true;
      }
      this.replySent = true;
    }
  }
}
