// Type: System.Runtime.InputQueue`1
// Assembly: System.ServiceModel.Internals, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.ServiceModel.Internals\v4.0_4.0.0.0__31bf3856ad364e35\System.ServiceModel.Internals.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Runtime
{
  internal sealed class InputQueue<T> : IDisposable where T : class
  {
    private static Action<object> completeOutstandingReadersCallback;
    private static Action<object> completeWaitersFalseCallback;
    private static Action<object> completeWaitersTrueCallback;
    private static Action<object> onDispatchCallback;
    private static Action<object> onInvokeDequeuedCallback;
    private InputQueue<T>.QueueState queueState;
    private InputQueue<T>.ItemQueue itemQueue;
    private Queue<InputQueue<T>.IQueueReader> readerQueue;
    private List<InputQueue<T>.IQueueWaiter> waiterList;

    public int PendingCount
    {
      get
      {
        lock (this.ThisLock)
          return this.itemQueue.ItemCount;
      }
    }

    public Action<T> DisposeItemCallback { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

    private Func<Action<AsyncCallback, IAsyncResult>> AsyncCallbackGenerator { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

    object ThisLock
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] private get
      {
        return (object) this.itemQueue;
      }
    }

    public InputQueue()
    {
      this.itemQueue = new InputQueue<T>.ItemQueue();
      this.readerQueue = new Queue<InputQueue<T>.IQueueReader>();
      this.waiterList = new List<InputQueue<T>.IQueueWaiter>();
      this.queueState = (InputQueue<T>.QueueState) 0;
    }

    public InputQueue(Func<Action<AsyncCallback, IAsyncResult>> asyncCallbackGenerator)
      : this()
    {
      this.AsyncCallbackGenerator = asyncCallbackGenerator;
    }

    public IAsyncResult BeginDequeue(TimeSpan timeout, AsyncCallback callback, object state)
    {
      // ISSUE: unable to decompile the method.
    }

    public IAsyncResult BeginWaitForItem(TimeSpan timeout, AsyncCallback callback, object state)
    {
      // ISSUE: unable to decompile the method.
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Close()
    {
      this.Dispose();
    }

    public T Dequeue(TimeSpan timeout)
    {
      T obj;
      if (!this.Dequeue(timeout, out obj))
        throw Fx.Exception.AsError((Exception) new TimeoutException(InternalSR.TimeoutInputQueueDequeue((object) timeout)));
      else
        return obj;
    }

    public bool Dequeue(TimeSpan timeout, out T value)
    {
      // ISSUE: unable to decompile the method.
    }

    public void Dispatch()
    {
      InputQueue<T>.IQueueReader queueReader = (InputQueue<T>.IQueueReader) null;
      InputQueue<T>.Item obj = new InputQueue<T>.Item();
      InputQueue<T>.IQueueReader[] array = (InputQueue<T>.IQueueReader[]) null;
      InputQueue<T>.IQueueWaiter[] waiters = (InputQueue<T>.IQueueWaiter[]) null;
      bool itemAvailable = true;
      lock (this.ThisLock)
      {
        itemAvailable = this.queueState != 2 && this.queueState != 1;
        this.GetWaiters(out waiters);
        if (this.queueState != 2)
        {
          this.itemQueue.MakePendingItemAvailable();
          if (this.readerQueue.Count > 0)
          {
            obj = this.itemQueue.DequeueAvailableItem();
            queueReader = this.readerQueue.Dequeue();
            if (this.queueState == 1)
            {
              if (this.readerQueue.Count > 0)
              {
                if (this.itemQueue.ItemCount == 0)
                {
                  array = new InputQueue<T>.IQueueReader[this.readerQueue.Count];
                  this.readerQueue.CopyTo(array, 0);
                  this.readerQueue.Clear();
                  itemAvailable = false;
                }
              }
            }
          }
        }
      }
      if (array != null)
      {
        if (InputQueue<T>.completeOutstandingReadersCallback == null)
          InputQueue<T>.completeOutstandingReadersCallback = new Action<object>(InputQueue<T>.CompleteOutstandingReadersCallback);
        ActionItem.Schedule(InputQueue<T>.completeOutstandingReadersCallback, (object) array);
      }
      if (waiters != null)
        InputQueue<T>.CompleteWaitersLater(itemAvailable, waiters);
      if (queueReader == null)
        return;
      InputQueue<T>.InvokeDequeuedCallback(obj.DequeuedCallback);
      queueReader.Set(obj);
    }

    public bool EndDequeue(IAsyncResult result, out T value)
    {
      if (!(result is CompletedAsyncResult<T>))
        return InputQueue<T>.AsyncQueueReader.End(result, out value);
      value = CompletedAsyncResult<T>.End(result);
      return true;
    }

    public T EndDequeue(IAsyncResult result)
    {
      T obj;
      if (!this.EndDequeue(result, out obj))
        throw Fx.Exception.AsError((Exception) new TimeoutException());
      else
        return obj;
    }

    public bool EndWaitForItem(IAsyncResult result)
    {
      if (result is CompletedAsyncResult<bool>)
        return CompletedAsyncResult<bool>.End(result);
      else
        return InputQueue<T>.AsyncQueueWaiter.End(result);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void EnqueueAndDispatch(T item)
    {
      this.EnqueueAndDispatch(item, (Action) null);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void EnqueueAndDispatch(T item, Action dequeuedCallback)
    {
      this.EnqueueAndDispatch(item, dequeuedCallback, true);
    }

    public void EnqueueAndDispatch(Exception exception, Action dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.EnqueueAndDispatch(new InputQueue<T>.Item(exception, dequeuedCallback), canDispatchOnThisThread);
    }

    public void EnqueueAndDispatch(T item, Action dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.EnqueueAndDispatch(new InputQueue<T>.Item(item, dequeuedCallback), canDispatchOnThisThread);
    }

    public bool EnqueueWithoutDispatch(T item, Action dequeuedCallback)
    {
      return this.EnqueueWithoutDispatch(new InputQueue<T>.Item(item, dequeuedCallback));
    }

    public bool EnqueueWithoutDispatch(Exception exception, Action dequeuedCallback)
    {
      return this.EnqueueWithoutDispatch(new InputQueue<T>.Item(exception, dequeuedCallback));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Shutdown()
    {
      this.Shutdown((Func<Exception>) null);
    }

    public void Shutdown(Func<Exception> pendingExceptionGenerator)
    {
      InputQueue<T>.IQueueReader[] array = (InputQueue<T>.IQueueReader[]) null;
      lock (this.ThisLock)
      {
        if (this.queueState == 1 || this.queueState == 2)
          return;
        this.queueState = (InputQueue<T>.QueueState) 1;
        if (this.readerQueue.Count > 0)
        {
          if (this.itemQueue.ItemCount == 0)
          {
            array = new InputQueue<T>.IQueueReader[this.readerQueue.Count];
            this.readerQueue.CopyTo(array, 0);
            this.readerQueue.Clear();
          }
        }
      }
      if (array == null)
        return;
      for (int index = 0; index < array.Length; ++index)
      {
        Exception exception = pendingExceptionGenerator != null ? pendingExceptionGenerator() : (Exception) null;
        array[index].Set(new InputQueue<T>.Item(exception, (Action) null));
      }
    }

    public bool WaitForItem(TimeSpan timeout)
    {
      // ISSUE: unable to decompile the method.
    }

    public void Dispose()
    {
      bool flag = false;
      lock (this.ThisLock)
      {
        if (this.queueState != 2)
        {
          this.queueState = (InputQueue<T>.QueueState) 2;
          flag = true;
        }
      }
      if (!flag)
        return;
      while (this.readerQueue.Count > 0)
        this.readerQueue.Dequeue().Set(new InputQueue<T>.Item());
      while (this.itemQueue.HasAnyItem)
      {
        InputQueue<T>.Item obj = this.itemQueue.DequeueAnyItem();
        this.DisposeItem(obj);
        InputQueue<T>.InvokeDequeuedCallback(obj.DequeuedCallback);
      }
    }

    private void DisposeItem(InputQueue<T>.Item item)
    {
      T obj = item.Value;
      if ((object) obj == null)
        return;
      if ((object) obj is IDisposable)
      {
        ((IDisposable) (object) obj).Dispose();
      }
      else
      {
        Action<T> disposeItemCallback = this.DisposeItemCallback;
        if (disposeItemCallback == null)
          return;
        disposeItemCallback(obj);
      }
    }

    private static void CompleteOutstandingReadersCallback(object state)
    {
      foreach (InputQueue<T>.IQueueReader queueReader in (InputQueue<T>.IQueueReader[]) state)
        queueReader.Set(new InputQueue<T>.Item());
    }

    private static void CompleteWaiters(bool itemAvailable, InputQueue<T>.IQueueWaiter[] waiters)
    {
      for (int index = 0; index < waiters.Length; ++index)
        waiters[index].Set(itemAvailable);
    }

    private static void CompleteWaitersFalseCallback(object state)
    {
      InputQueue<T>.CompleteWaiters(false, (InputQueue<T>.IQueueWaiter[]) state);
    }

    private static void CompleteWaitersLater(bool itemAvailable, InputQueue<T>.IQueueWaiter[] waiters)
    {
      if (itemAvailable)
      {
        if (InputQueue<T>.completeWaitersTrueCallback == null)
          InputQueue<T>.completeWaitersTrueCallback = new Action<object>(InputQueue<T>.CompleteWaitersTrueCallback);
        ActionItem.Schedule(InputQueue<T>.completeWaitersTrueCallback, (object) waiters);
      }
      else
      {
        if (InputQueue<T>.completeWaitersFalseCallback == null)
          InputQueue<T>.completeWaitersFalseCallback = new Action<object>(InputQueue<T>.CompleteWaitersFalseCallback);
        ActionItem.Schedule(InputQueue<T>.completeWaitersFalseCallback, (object) waiters);
      }
    }

    private static void CompleteWaitersTrueCallback(object state)
    {
      InputQueue<T>.CompleteWaiters(true, (InputQueue<T>.IQueueWaiter[]) state);
    }

    private static void InvokeDequeuedCallback(Action dequeuedCallback)
    {
      if (dequeuedCallback == null)
        return;
      dequeuedCallback();
    }

    private static void InvokeDequeuedCallbackLater(Action dequeuedCallback)
    {
      if (dequeuedCallback == null)
        return;
      if (InputQueue<T>.onInvokeDequeuedCallback == null)
        InputQueue<T>.onInvokeDequeuedCallback = new Action<object>(InputQueue<T>.OnInvokeDequeuedCallback);
      ActionItem.Schedule(InputQueue<T>.onInvokeDequeuedCallback, (object) dequeuedCallback);
    }

    private static void OnDispatchCallback(object state)
    {
      ((InputQueue<T>) state).Dispatch();
    }

    private static void OnInvokeDequeuedCallback(object state)
    {
      ((Action) state)();
    }

    private void EnqueueAndDispatch(InputQueue<T>.Item item, bool canDispatchOnThisThread)
    {
      // ISSUE: unable to decompile the method.
    }

    private bool EnqueueWithoutDispatch(InputQueue<T>.Item item)
    {
      lock (this.ThisLock)
      {
        if (this.queueState != 2)
        {
          if (this.queueState != 1)
          {
            if (this.readerQueue.Count == 0 && this.waiterList.Count == 0)
            {
              this.itemQueue.EnqueueAvailableItem(item);
              return false;
            }
            else
            {
              this.itemQueue.EnqueuePendingItem(item);
              return true;
            }
          }
        }
      }
      this.DisposeItem(item);
      InputQueue<T>.InvokeDequeuedCallbackLater(item.DequeuedCallback);
      return false;
    }

    private void GetWaiters(out InputQueue<T>.IQueueWaiter[] waiters)
    {
      if (this.waiterList.Count > 0)
      {
        waiters = this.waiterList.ToArray();
        this.waiterList.Clear();
      }
      else
        waiters = (InputQueue<T>.IQueueWaiter[]) null;
    }

    private bool RemoveReader(InputQueue<T>.IQueueReader reader)
    {
      // ISSUE: unable to decompile the method.
    }

    private enum QueueState
    {
      Open,
      Shutdown,
      Closed,
    }

    private interface IQueueReader
    {
      void Set(InputQueue<T>.Item item);
    }

    private interface IQueueWaiter
    {
      void Set(bool itemAvailable);
    }

    private struct Item
    {
      private Action dequeuedCallback;
      private Exception exception;
      private T value;

      public Action DequeuedCallback
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.dequeuedCallback;
        }
      }

      public Exception Exception
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.exception;
        }
      }

      public T Value
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.value;
        }
      }

      public Item(T value, Action dequeuedCallback)
      {
        this = new InputQueue<T>.Item(value, (Exception) null, dequeuedCallback);
      }

      public Item(Exception exception, Action dequeuedCallback)
      {
        this = new InputQueue<T>.Item(default (T), exception, dequeuedCallback);
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      private Item(T value, Exception exception, Action dequeuedCallback)
      {
        this.value = value;
        this.exception = exception;
        this.dequeuedCallback = dequeuedCallback;
      }

      public T GetValue()
      {
        if (this.exception != null)
          throw Fx.Exception.AsError(this.exception);
        else
          return this.value;
      }
    }

    private class AsyncQueueReader : AsyncResult, InputQueue<T>.IQueueReader
    {
      private static Action<object> timerCallback = new Action<object>(InputQueue<T>.AsyncQueueReader.TimerCallback);
      private bool expired;
      private InputQueue<T> inputQueue;
      private T item;
      private IOThreadTimer timer;

      static AsyncQueueReader()
      {
      }

      public AsyncQueueReader(InputQueue<T> inputQueue, TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        if (inputQueue.AsyncCallbackGenerator != null)
          this.VirtualCallback = inputQueue.AsyncCallbackGenerator();
        this.inputQueue = inputQueue;
        if (!(timeout != TimeSpan.MaxValue))
          return;
        this.timer = new IOThreadTimer(InputQueue<T>.AsyncQueueReader.timerCallback, (object) this, false);
        this.timer.Set(timeout);
      }

      public static bool End(IAsyncResult result, out T value)
      {
        InputQueue<T>.AsyncQueueReader asyncQueueReader = AsyncResult.End<InputQueue<T>.AsyncQueueReader>(result);
        if (asyncQueueReader.expired)
        {
          value = default (T);
          return false;
        }
        else
        {
          value = asyncQueueReader.item;
          return true;
        }
      }

      public void Set(InputQueue<T>.Item item)
      {
        this.item = item.Value;
        if (this.timer != null)
          this.timer.Cancel();
        this.Complete(false, item.Exception);
      }

      private static void TimerCallback(object state)
      {
        InputQueue<T>.AsyncQueueReader asyncQueueReader = (InputQueue<T>.AsyncQueueReader) state;
        if (!asyncQueueReader.inputQueue.RemoveReader((InputQueue<T>.IQueueReader) asyncQueueReader))
          return;
        asyncQueueReader.expired = true;
        asyncQueueReader.Complete(false);
      }
    }

    private class AsyncQueueWaiter : AsyncResult, InputQueue<T>.IQueueWaiter
    {
      private static Action<object> timerCallback = new Action<object>(InputQueue<T>.AsyncQueueWaiter.TimerCallback);
      private object thisLock = new object();
      private bool itemAvailable;
      private IOThreadTimer timer;

      object ThisLock
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] private get
        {
          return this.thisLock;
        }
      }

      static AsyncQueueWaiter()
      {
      }

      public AsyncQueueWaiter(TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        if (!(timeout != TimeSpan.MaxValue))
          return;
        this.timer = new IOThreadTimer(InputQueue<T>.AsyncQueueWaiter.timerCallback, (object) this, false);
        this.timer.Set(timeout);
      }

      public static bool End(IAsyncResult result)
      {
        return AsyncResult.End<InputQueue<T>.AsyncQueueWaiter>(result).itemAvailable;
      }

      public void Set(bool itemAvailable)
      {
        bool flag;
        lock (this.ThisLock)
        {
          flag = this.timer == null || this.timer.Cancel();
          this.itemAvailable = itemAvailable;
        }
        if (!flag)
          return;
        this.Complete(false);
      }

      private static void TimerCallback(object state)
      {
        ((AsyncResult) state).Complete(false);
      }
    }

    private class ItemQueue
    {
      private int head;
      private InputQueue<T>.Item[] items;
      private int pendingCount;
      private int totalCount;

      public bool HasAnyItem
      {
        get
        {
          return this.totalCount > 0;
        }
      }

      public bool HasAvailableItem
      {
        get
        {
          return this.totalCount > this.pendingCount;
        }
      }

      public int ItemCount
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.totalCount;
        }
      }

      public ItemQueue()
      {
        this.items = new InputQueue<T>.Item[1];
      }

      public InputQueue<T>.Item DequeueAnyItem()
      {
        if (this.pendingCount == this.totalCount)
          --this.pendingCount;
        return this.DequeueItemCore();
      }

      public InputQueue<T>.Item DequeueAvailableItem()
      {
        Fx.AssertAndThrow(this.totalCount != this.pendingCount, "ItemQueue does not contain any available items");
        return this.DequeueItemCore();
      }

      public void EnqueueAvailableItem(InputQueue<T>.Item item)
      {
        this.EnqueueItemCore(item);
      }

      public void EnqueuePendingItem(InputQueue<T>.Item item)
      {
        this.EnqueueItemCore(item);
        ++this.pendingCount;
      }

      public void MakePendingItemAvailable()
      {
        Fx.AssertAndThrow(this.pendingCount != 0, "ItemQueue does not contain any pending items");
        --this.pendingCount;
      }

      private InputQueue<T>.Item DequeueItemCore()
      {
        Fx.AssertAndThrow(this.totalCount != 0, "ItemQueue does not contain any items");
        InputQueue<T>.Item obj = this.items[this.head];
        this.items[this.head] = new InputQueue<T>.Item();
        --this.totalCount;
        this.head = (this.head + 1) % this.items.Length;
        return obj;
      }

      private void EnqueueItemCore(InputQueue<T>.Item item)
      {
        if (this.totalCount == this.items.Length)
        {
          InputQueue<T>.Item[] objArray = new InputQueue<T>.Item[this.items.Length * 2];
          for (int index = 0; index < this.totalCount; ++index)
            objArray[index] = this.items[(this.head + index) % this.items.Length];
          this.head = 0;
          this.items = objArray;
        }
        this.items[(this.head + this.totalCount) % this.items.Length] = item;
        ++this.totalCount;
      }
    }

    private class WaitQueueReader : InputQueue<T>.IQueueReader
    {
      private Exception exception;
      private InputQueue<T> inputQueue;
      private T item;
      private ManualResetEvent waitEvent;

      public WaitQueueReader(InputQueue<T> inputQueue)
      {
        this.inputQueue = inputQueue;
        this.waitEvent = new ManualResetEvent(false);
      }

      public void Set(InputQueue<T>.Item item)
      {
        lock (this)
        {
          this.exception = item.Exception;
          this.item = item.Value;
          this.waitEvent.Set();
        }
      }

      public bool Wait(TimeSpan timeout, out T value)
      {
        bool flag = false;
        try
        {
          if (!TimeoutHelper.WaitOne((WaitHandle) this.waitEvent, timeout))
          {
            if (this.inputQueue.RemoveReader((InputQueue<T>.IQueueReader) this))
            {
              value = default (T);
              flag = true;
              return false;
            }
            else
              this.waitEvent.WaitOne();
          }
          flag = true;
        }
        finally
        {
          if (flag)
            this.waitEvent.Close();
        }
        if (this.exception != null)
          throw Fx.Exception.AsError(this.exception);
        value = this.item;
        return true;
      }
    }

    private class WaitQueueWaiter : InputQueue<T>.IQueueWaiter
    {
      private bool itemAvailable;
      private ManualResetEvent waitEvent;

      public WaitQueueWaiter()
      {
        this.waitEvent = new ManualResetEvent(false);
      }

      public void Set(bool itemAvailable)
      {
        lock (this)
        {
          this.itemAvailable = itemAvailable;
          this.waitEvent.Set();
        }
      }

      public bool Wait(TimeSpan timeout)
      {
        if (!TimeoutHelper.WaitOne((WaitHandle) this.waitEvent, timeout))
          return false;
        else
          return this.itemAvailable;
      }
    }
  }
}
