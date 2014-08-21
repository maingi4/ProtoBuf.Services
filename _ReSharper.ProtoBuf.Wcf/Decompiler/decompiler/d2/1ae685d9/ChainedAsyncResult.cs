// Type: System.ServiceModel.Channels.ChainedAsyncResult
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;

namespace System.ServiceModel.Channels
{
  internal class ChainedAsyncResult : AsyncResult
  {
    private static AsyncCallback begin1Callback = Fx.ThunkCallback(new AsyncCallback(ChainedAsyncResult.Begin1Callback));
    private static AsyncCallback begin2Callback = Fx.ThunkCallback(new AsyncCallback(ChainedAsyncResult.Begin2Callback));
    private ChainedBeginHandler begin2;
    private ChainedEndHandler end1;
    private ChainedEndHandler end2;
    private TimeoutHelper timeoutHelper;

    static ChainedAsyncResult()
    {
    }

    protected ChainedAsyncResult(TimeSpan timeout, AsyncCallback callback, object state)
      : base(callback, state)
    {
      this.timeoutHelper = new TimeoutHelper(timeout);
    }

    public ChainedAsyncResult(TimeSpan timeout, AsyncCallback callback, object state, ChainedBeginHandler begin1, ChainedEndHandler end1, ChainedBeginHandler begin2, ChainedEndHandler end2)
      : base(callback, state)
    {
      this.timeoutHelper = new TimeoutHelper(timeout);
      this.Begin(begin1, end1, begin2, end2);
    }

    protected void Begin(ChainedBeginHandler begin1, ChainedEndHandler end1, ChainedBeginHandler begin2, ChainedEndHandler end2)
    {
      this.end1 = end1;
      this.begin2 = begin2;
      this.end2 = end2;
      IAsyncResult result = begin1(this.timeoutHelper.RemainingTime(), ChainedAsyncResult.begin1Callback, (object) this);
      if (!result.CompletedSynchronously || !this.Begin1Completed(result))
        return;
      this.Complete(true);
    }

    public static void End(IAsyncResult result)
    {
      AsyncResult.End<ChainedAsyncResult>(result);
    }

    private static void Begin1Callback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      ChainedAsyncResult chainedAsyncResult = (ChainedAsyncResult) result.AsyncState;
      Exception exception = (Exception) null;
      bool flag;
      try
      {
        flag = chainedAsyncResult.Begin1Completed(result);
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
      chainedAsyncResult.Complete(false, exception);
    }

    private bool Begin1Completed(IAsyncResult result)
    {
      this.end1(result);
      result = this.begin2(this.timeoutHelper.RemainingTime(), ChainedAsyncResult.begin2Callback, (object) this);
      if (!result.CompletedSynchronously)
        return false;
      this.end2(result);
      return true;
    }

    private static void Begin2Callback(IAsyncResult result)
    {
      if (result.CompletedSynchronously)
        return;
      ChainedAsyncResult chainedAsyncResult = (ChainedAsyncResult) result.AsyncState;
      Exception exception = (Exception) null;
      try
      {
        chainedAsyncResult.end2(result);
      }
      catch (Exception ex)
      {
        if (Fx.IsFatal(ex))
          throw;
        else
          exception = ex;
      }
      chainedAsyncResult.Complete(false, exception);
    }
  }
}
