// Type: System.ServiceModel.Channels.TransportReplyChannelAcceptor
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.ServiceModel.dll

using System;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal class TransportReplyChannelAcceptor : ReplyChannelAcceptor
  {
    public TransportReplyChannelAcceptor(TransportChannelListener listener);

    protected override ReplyChannel OnCreateChannel();

    protected override void OnOpening();

    protected override void OnAbort();

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state);

    protected override void OnEndClose(IAsyncResult result);

    protected override void OnClose(TimeSpan timeout);

    protected class TransportReplyChannel : ReplyChannel
    {
      public TransportReplyChannel(ChannelManagerBase channelManager, EndpointAddress localAddress);

      public bool TransferTransportManagers(TransportManagerContainer transportManagerContainer);

      protected override void OnAbort();

      protected override void OnClose(TimeSpan timeout);

      protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state);

      protected override void OnEndClose(IAsyncResult result);
    }
  }
}
