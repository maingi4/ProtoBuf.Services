// Type: System.ServiceModel.Channels.TransportReplyChannelAcceptor
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal class TransportReplyChannelAcceptor : ReplyChannelAcceptor
  {
    private TransportManagerContainer transportManagerContainer;
    private TransportChannelListener listener;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public TransportReplyChannelAcceptor(TransportChannelListener listener)
      : base((ChannelManagerBase) listener)
    {
      this.listener = listener;
    }

    protected override ReplyChannel OnCreateChannel()
    {
      return (ReplyChannel) new TransportReplyChannelAcceptor.TransportReplyChannel(this.ChannelManager, (EndpointAddress) null);
    }

    protected override void OnOpening()
    {
      base.OnOpening();
      this.transportManagerContainer = this.listener.GetTransportManagers();
      this.listener = (TransportChannelListener) null;
    }

    protected override void OnAbort()
    {
      base.OnAbort();
      if (this.transportManagerContainer == null || this.TransferTransportManagers())
        return;
      this.transportManagerContainer.Abort();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      ChainedBeginHandler begin2 = new ChainedBeginHandler(this.DummyBeginClose);
      ChainedEndHandler end2 = new ChainedEndHandler(this.DummyEndClose);
      if (this.transportManagerContainer != null && !this.TransferTransportManagers())
      {
        begin2 = new ChainedBeginHandler(this.transportManagerContainer.BeginClose);
        end2 = new ChainedEndHandler(this.transportManagerContainer.EndClose);
      }
      return (IAsyncResult) new ChainedAsyncResult(timeout, callback, state, new ChainedBeginHandler(((ChannelAcceptor<IReplyChannel>) this).OnBeginClose), new ChainedEndHandler(((ChannelAcceptor<IReplyChannel>) this).OnEndClose), begin2, end2);
    }

    protected override void OnEndClose(IAsyncResult result)
    {
      ChainedAsyncResult.End(result);
    }

    protected override void OnClose(TimeSpan timeout)
    {
      TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
      base.OnClose(timeoutHelper.RemainingTime());
      if (this.transportManagerContainer == null || this.TransferTransportManagers())
        return;
      this.transportManagerContainer.Close(timeoutHelper.RemainingTime());
    }

    private IAsyncResult DummyBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new CompletedAsyncResult(callback, state);
    }

    private void DummyEndClose(IAsyncResult result)
    {
      CompletedAsyncResult.End(result);
    }

    private bool TransferTransportManagers()
    {
      TransportReplyChannelAcceptor.TransportReplyChannel transportReplyChannel = (TransportReplyChannelAcceptor.TransportReplyChannel) this.GetCurrentChannel();
      if (transportReplyChannel == null)
        return false;
      else
        return transportReplyChannel.TransferTransportManagers(this.transportManagerContainer);
    }

    protected class TransportReplyChannel : ReplyChannel
    {
      private TransportManagerContainer transportManagerContainer;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public TransportReplyChannel(ChannelManagerBase channelManager, EndpointAddress localAddress)
        : base(channelManager, localAddress)
      {
      }

      public bool TransferTransportManagers(TransportManagerContainer transportManagerContainer)
      {
        lock (this.ThisLock)
        {
          if (this.State != CommunicationState.Opened)
            return false;
          this.transportManagerContainer = transportManagerContainer;
          return true;
        }
      }

      protected override void OnAbort()
      {
        if (this.transportManagerContainer != null)
          this.transportManagerContainer.Abort();
        base.OnAbort();
      }

      protected override void OnClose(TimeSpan timeout)
      {
        TimeoutHelper timeoutHelper = new TimeoutHelper(timeout);
        if (this.transportManagerContainer != null)
          this.transportManagerContainer.Close(timeoutHelper.RemainingTime());
        base.OnClose(timeoutHelper.RemainingTime());
      }

      protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
      {
        ChainedBeginHandler begin1 = new ChainedBeginHandler(this.DummyBeginClose);
        ChainedEndHandler end1 = new ChainedEndHandler(this.DummyEndClose);
        if (this.transportManagerContainer != null)
        {
          begin1 = new ChainedBeginHandler(this.transportManagerContainer.BeginClose);
          end1 = new ChainedEndHandler(this.transportManagerContainer.EndClose);
        }
        return (IAsyncResult) new ChainedAsyncResult(timeout, callback, state, begin1, end1, new ChainedBeginHandler(((InputQueueChannel<RequestContext>) this).OnBeginClose), new ChainedEndHandler(((InputQueueChannel<RequestContext>) this).OnEndClose));
      }

      protected override void OnEndClose(IAsyncResult result)
      {
        ChainedAsyncResult.End(result);
      }

      private IAsyncResult DummyBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
      {
        return (IAsyncResult) new CompletedAsyncResult(callback, state);
      }

      private void DummyEndClose(IAsyncResult result)
      {
        CompletedAsyncResult.End(result);
      }
    }
  }
}
