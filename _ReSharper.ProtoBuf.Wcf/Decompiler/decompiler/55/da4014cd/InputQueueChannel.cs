// Type: System.ServiceModel.Channels.InputQueueChannel`1
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal abstract class InputQueueChannel<TDisposable> : ChannelBase where TDisposable : class, IDisposable
  {
    private InputQueue<TDisposable> inputQueue;

    public int InternalPendingItems
    {
      get
      {
        return this.inputQueue.PendingCount;
      }
    }

    public int PendingItems
    {
      get
      {
        this.ThrowIfDisposedOrNotOpen();
        return this.InternalPendingItems;
      }
    }

    protected InputQueueChannel(ChannelManagerBase channelManager)
      : base(channelManager)
    {
      this.inputQueue = new InputQueue<TDisposable>();
    }

    public void EnqueueAndDispatch(TDisposable item)
    {
      this.EnqueueAndDispatch(item, (ItemDequeuedCallback) null);
    }

    public void EnqueueAndDispatch(TDisposable item, ItemDequeuedCallback dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.OnEnqueueItem(item);
      this.inputQueue.EnqueueAndDispatch(item, dequeuedCallback, canDispatchOnThisThread);
    }

    public void EnqueueAndDispatch(Exception exception, ItemDequeuedCallback dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.inputQueue.EnqueueAndDispatch(exception, dequeuedCallback, canDispatchOnThisThread);
    }

    public void EnqueueAndDispatch(TDisposable item, ItemDequeuedCallback dequeuedCallback)
    {
      this.OnEnqueueItem(item);
      this.inputQueue.EnqueueAndDispatch(item, dequeuedCallback);
    }

    public bool EnqueueWithoutDispatch(Exception exception, ItemDequeuedCallback dequeuedCallback)
    {
      return this.inputQueue.EnqueueWithoutDispatch(exception, dequeuedCallback);
    }

    public bool EnqueueWithoutDispatch(TDisposable item, ItemDequeuedCallback dequeuedCallback)
    {
      this.OnEnqueueItem(item);
      return this.inputQueue.EnqueueWithoutDispatch(item, dequeuedCallback);
    }

    public void Dispatch()
    {
      this.inputQueue.Dispatch();
    }

    public void Shutdown()
    {
      this.inputQueue.Shutdown();
    }

    protected override void OnFaulted()
    {
      base.OnFaulted();
      this.inputQueue.Shutdown((CommunicationObject) this);
    }

    protected virtual void OnEnqueueItem(TDisposable item)
    {
    }

    protected IAsyncResult BeginDequeue(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.ThrowIfNotOpened();
      return this.inputQueue.BeginDequeue(timeout, callback, state);
    }

    protected bool EndDequeue(IAsyncResult result, out TDisposable item)
    {
      bool flag = this.inputQueue.EndDequeue(result, out item);
      if ((object) item == null)
      {
        this.ThrowIfFaulted();
        this.ThrowIfAborted();
      }
      return flag;
    }

    protected bool Dequeue(TimeSpan timeout, out TDisposable item)
    {
      this.ThrowIfNotOpened();
      bool flag = this.inputQueue.Dequeue(timeout, out item);
      if ((object) item == null)
      {
        this.ThrowIfFaulted();
        this.ThrowIfAborted();
      }
      return flag;
    }

    protected bool WaitForItem(TimeSpan timeout)
    {
      this.ThrowIfNotOpened();
      bool flag = this.inputQueue.WaitForItem(timeout);
      this.ThrowIfFaulted();
      this.ThrowIfAborted();
      return flag;
    }

    protected IAsyncResult BeginWaitForItem(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.ThrowIfNotOpened();
      return this.inputQueue.BeginWaitForItem(timeout, callback, state);
    }

    protected bool EndWaitForItem(IAsyncResult result)
    {
      bool flag = this.inputQueue.EndWaitForItem(result);
      this.ThrowIfFaulted();
      this.ThrowIfAborted();
      return flag;
    }

    protected override void OnClosing()
    {
      base.OnClosing();
      this.inputQueue.Shutdown((CommunicationObject) this);
    }

    protected override void OnAbort()
    {
      this.inputQueue.Close();
    }

    protected override void OnClose(TimeSpan timeout)
    {
      this.inputQueue.Close();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.inputQueue.Close();
      return (IAsyncResult) new CompletedAsyncResult(callback, state);
    }

    protected override void OnEndClose(IAsyncResult result)
    {
      CompletedAsyncResult.End(result);
    }
  }
}
