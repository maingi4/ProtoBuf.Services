// Type: System.ServiceModel.Channels.InputQueue`1
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;
using System.Threading;

namespace System.ServiceModel.Channels
{
  internal class InputQueue<T> : IDisposable where T : class
  {
    private InputQueue<T>.ItemQueue itemQueue;
    private InputQueue<T>.QueueState queueState;
    private Queue<InputQueue<T>.IQueueReader> readerQueue;
    private List<InputQueue<T>.IQueueWaiter> waiterList;
    private static WaitCallback onInvokeDequeuedCallback;
    private static WaitCallback onDispatchCallback;
    private static WaitCallback completeOutstandingReadersCallback;
    private static WaitCallback completeWaitersFalseCallback;
    private static WaitCallback completeWaitersTrueCallback;

    public int PendingCount
    {
      get
      {
        lock (this.ThisLock)
          return this.itemQueue.ItemCount;
      }
    }

    object ThisLock
    {
      private get
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

    public IAsyncResult BeginDequeue(TimeSpan timeout, AsyncCallback callback, object state)
    {
      // ISSUE: unable to decompile the method.
    }

    public IAsyncResult BeginWaitForItem(TimeSpan timeout, AsyncCallback callback, object state)
    {
      // ISSUE: unable to decompile the method.
    }

    private static void CompleteOutstandingReadersCallback(object state)
    {
      foreach (InputQueue<T>.IQueueReader queueReader in (InputQueue<T>.IQueueReader[]) state)
        queueReader.Set(new InputQueue<T>.Item());
    }

    private static void CompleteWaitersFalseCallback(object state)
    {
      InputQueue<T>.CompleteWaiters(false, (InputQueue<T>.IQueueWaiter[]) state);
    }

    private static void CompleteWaitersTrueCallback(object state)
    {
      InputQueue<T>.CompleteWaiters(true, (InputQueue<T>.IQueueWaiter[]) state);
    }

    private static void CompleteWaiters(bool itemAvailable, InputQueue<T>.IQueueWaiter[] waiters)
    {
      for (int index = 0; index < waiters.Length; ++index)
        waiters[index].Set(itemAvailable);
    }

    private static void CompleteWaitersLater(bool itemAvailable, InputQueue<T>.IQueueWaiter[] waiters)
    {
      if (itemAvailable)
      {
        if (InputQueue<T>.completeWaitersTrueCallback == null)
          InputQueue<T>.completeWaitersTrueCallback = new WaitCallback(InputQueue<T>.CompleteWaitersTrueCallback);
        IOThreadScheduler.ScheduleCallback(InputQueue<T>.completeWaitersTrueCallback, (object) waiters);
      }
      else
      {
        if (InputQueue<T>.completeWaitersFalseCallback == null)
          InputQueue<T>.completeWaitersFalseCallback = new WaitCallback(InputQueue<T>.CompleteWaitersFalseCallback);
        IOThreadScheduler.ScheduleCallback(InputQueue<T>.completeWaitersFalseCallback, (object) waiters);
      }
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

    public void Close()
    {
      this.Dispose();
    }

    public void Shutdown()
    {
      this.Shutdown((CommunicationObject) null);
    }

    public void Shutdown(CommunicationObject communicationObject)
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
        Exception exception = communicationObject == null ? (Exception) null : communicationObject.GetPendingException();
        array[index].Set(new InputQueue<T>.Item(exception, (ItemDequeuedCallback) null));
      }
    }

    public T Dequeue(TimeSpan timeout)
    {
      T obj;
      if (this.Dequeue(timeout, out obj))
        return obj;
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new TimeoutException(System.ServiceModel.SR.GetString("TimeoutInputQueueDequeue1", new object[1]
      {
        (object) timeout
      })));
    }

    public bool Dequeue(TimeSpan timeout, out T value)
    {
      // ISSUE: unable to decompile the method.
    }

    public bool WaitForItem(TimeSpan timeout)
    {
      // ISSUE: unable to decompile the method.
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected void Dispose(bool disposing)
    {
      if (!disposing)
        return;
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
        obj.Dispose();
        InputQueue<T>.InvokeDequeuedCallback(obj.DequeuedCallback);
      }
    }

    public bool EndDequeue(IAsyncResult result, out T value)
    {
      if (!(result is TypedCompletedAsyncResult<T>))
        return InputQueue<T>.AsyncQueueReader.End(result, out value);
      value = TypedCompletedAsyncResult<T>.End(result);
      return true;
    }

    public T EndDequeue(IAsyncResult result)
    {
      T obj;
      if (!this.EndDequeue(result, out obj))
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new TimeoutException());
      else
        return obj;
    }

    public bool EndWaitForItem(IAsyncResult result)
    {
      if (result is TypedCompletedAsyncResult<bool>)
        return TypedCompletedAsyncResult<bool>.End(result);
      else
        return InputQueue<T>.AsyncQueueWaiter.End(result);
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
          InputQueue<T>.completeOutstandingReadersCallback = new WaitCallback(InputQueue<T>.CompleteOutstandingReadersCallback);
        IOThreadScheduler.ScheduleCallback(InputQueue<T>.completeOutstandingReadersCallback, (object) array);
      }
      if (waiters != null)
        InputQueue<T>.CompleteWaitersLater(itemAvailable, waiters);
      if (queueReader == null)
        return;
      InputQueue<T>.InvokeDequeuedCallback(obj.DequeuedCallback);
      queueReader.Set(obj);
    }

    public void EnqueueAndDispatch(T item)
    {
      this.EnqueueAndDispatch(item, (ItemDequeuedCallback) null);
    }

    public void EnqueueAndDispatch(T item, ItemDequeuedCallback dequeuedCallback)
    {
      this.EnqueueAndDispatch(item, dequeuedCallback, true);
    }

    public void EnqueueAndDispatch(Exception exception, ItemDequeuedCallback dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.EnqueueAndDispatch(new InputQueue<T>.Item(exception, dequeuedCallback), canDispatchOnThisThread);
    }

    public void EnqueueAndDispatch(T item, ItemDequeuedCallback dequeuedCallback, bool canDispatchOnThisThread)
    {
      this.EnqueueAndDispatch(new InputQueue<T>.Item(item, dequeuedCallback), canDispatchOnThisThread);
    }

    private void EnqueueAndDispatch(InputQueue<T>.Item item, bool canDispatchOnThisThread)
    {
      // ISSUE: unable to decompile the method.
    }

    public bool EnqueueWithoutDispatch(T item, ItemDequeuedCallback dequeuedCallback)
    {
      return this.EnqueueWithoutDispatch(new InputQueue<T>.Item(item, dequeuedCallback));
    }

    public bool EnqueueWithoutDispatch(Exception exception, ItemDequeuedCallback dequeuedCallback)
    {
      return this.EnqueueWithoutDispatch(new InputQueue<T>.Item(exception, dequeuedCallback));
    }

    private bool EnqueueWithoutDispatch(InputQueue<T>.Item item)
    {
      lock (this.ThisLock)
      {
        if (this.queueState != 2)
        {
          if (this.queueState != 1)
          {
            if (this.readerQueue.Count == 0)
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
      item.Dispose();
      InputQueue<T>.InvokeDequeuedCallbackLater(item.DequeuedCallback);
      return false;
    }

    private static void OnDispatchCallback(object state)
    {
      ((InputQueue<T>) state).Dispatch();
    }

    private static void InvokeDequeuedCallbackLater(ItemDequeuedCallback dequeuedCallback)
    {
      if (dequeuedCallback == null)
        return;
      if (InputQueue<T>.onInvokeDequeuedCallback == null)
        InputQueue<T>.onInvokeDequeuedCallback = new WaitCallback(InputQueue<T>.OnInvokeDequeuedCallback);
      IOThreadScheduler.ScheduleCallback(InputQueue<T>.onInvokeDequeuedCallback, (object) dequeuedCallback);
    }

    private static void InvokeDequeuedCallback(ItemDequeuedCallback dequeuedCallback)
    {
      if (dequeuedCallback == null)
        return;
      dequeuedCallback();
    }

    private static void OnInvokeDequeuedCallback(object state)
    {
      ((ItemDequeuedCallback) state)();
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
          if (!TimeoutHelper.WaitOne((WaitHandle) this.waitEvent, timeout, false))
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
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.exception);
        value = this.item;
        return true;
      }
    }

    private class AsyncQueueReader : TraceAsyncResult, InputQueue<T>.IQueueReader
    {
      private static WaitCallback timerCallback = new WaitCallback(InputQueue<T>.AsyncQueueReader.TimerCallback);
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

      private static void TimerCallback(object state)
      {
        InputQueue<T>.AsyncQueueReader asyncQueueReader = (InputQueue<T>.AsyncQueueReader) state;
        if (!asyncQueueReader.inputQueue.RemoveReader((InputQueue<T>.IQueueReader) asyncQueueReader))
          return;
        asyncQueueReader.expired = true;
        asyncQueueReader.Complete(false);
      }

      public void Set(InputQueue<T>.Item item)
      {
        this.item = item.Value;
        if (this.timer != null)
          this.timer.Cancel();
        this.Complete(false, item.Exception);
      }
    }

    private struct Item
    {
      private T value;
      private Exception exception;
      private ItemDequeuedCallback dequeuedCallback;

      public Exception Exception
      {
        get
        {
          return this.exception;
        }
      }

      public T Value
      {
        get
        {
          return this.value;
        }
      }

      public ItemDequeuedCallback DequeuedCallback
      {
        get
        {
          return this.dequeuedCallback;
        }
      }

      public Item(T value, ItemDequeuedCallback dequeuedCallback)
      {
        this = new InputQueue<T>.Item(value, (Exception) null, dequeuedCallback);
      }

      public Item(Exception exception, ItemDequeuedCallback dequeuedCallback)
      {
        this = new InputQueue<T>.Item(default (T), exception, dequeuedCallback);
      }

      private Item(T value, Exception exception, ItemDequeuedCallback dequeuedCallback)
      {
        this.value = value;
        this.exception = exception;
        this.dequeuedCallback = dequeuedCallback;
      }

      public void Dispose()
      {
        if ((object) this.value == null)
          return;
        if ((object) this.value is IDisposable)
        {
          ((IDisposable) (object) this.value).Dispose();
        }
        else
        {
          if (!((object) this.value is ICommunicationObject))
            return;
          ((ICommunicationObject) (object) this.value).Abort();
        }
      }

      public T GetValue()
      {
        if (this.exception != null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.exception);
        else
          return this.value;
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
        if (!TimeoutHelper.WaitOne((WaitHandle) this.waitEvent, timeout, false))
          return false;
        else
          return this.itemAvailable;
      }
    }

    private class AsyncQueueWaiter : AsyncResult, InputQueue<T>.IQueueWaiter
    {
      private static WaitCallback timerCallback = new WaitCallback(InputQueue<T>.AsyncQueueWaiter.TimerCallback);
      private object thisLock = new object();
      private IOThreadTimer timer;
      private bool itemAvailable;

      object ThisLock
      {
        private get
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

      private static void TimerCallback(object state)
      {
        ((AsyncResult) state).Complete(false);
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
    }

    private class ItemQueue
    {
      private InputQueue<T>.Item[] items;
      private int head;
      private int pendingCount;
      private int totalCount;

      public bool HasAvailableItem
      {
        get
        {
          return this.totalCount > this.pendingCount;
        }
      }

      public bool HasAnyItem
      {
        get
        {
          return this.totalCount > 0;
        }
      }

      public int ItemCount
      {
        get
        {
          return this.totalCount;
        }
      }

      public ItemQueue()
      {
        this.items = new InputQueue<T>.Item[1];
      }

      public InputQueue<T>.Item DequeueAvailableItem()
      {
        if (this.totalCount == this.pendingCount)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperInternal(false);
        else
          return this.DequeueItemCore();
      }

      public InputQueue<T>.Item DequeueAnyItem()
      {
        if (this.pendingCount == this.totalCount)
          --this.pendingCount;
        return this.DequeueItemCore();
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

      private InputQueue<T>.Item DequeueItemCore()
      {
        if (this.totalCount == 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperInternal(false);
        InputQueue<T>.Item obj = this.items[this.head];
        this.items[this.head] = new InputQueue<T>.Item();
        --this.totalCount;
        this.head = (this.head + 1) % this.items.Length;
        return obj;
      }

      public void EnqueuePendingItem(InputQueue<T>.Item item)
      {
        this.EnqueueItemCore(item);
        ++this.pendingCount;
      }

      public void EnqueueAvailableItem(InputQueue<T>.Item item)
      {
        this.EnqueueItemCore(item);
      }

      public void MakePendingItemAvailable()
      {
        if (this.pendingCount == 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperInternal(false);
        --this.pendingCount;
      }
    }
  }
}
