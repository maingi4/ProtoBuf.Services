// Type: System.ServiceModel.Channels.ContextReplyChannel
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal class ContextReplyChannel : LayeredChannel<IReplyChannel>, IReplyChannel, IChannel, ICommunicationObject
  {
    private ContextExchangeMechanism contextExchangeMechanism;

    public EndpointAddress LocalAddress
    {
      get
      {
        return this.InnerChannel.LocalAddress;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ContextReplyChannel(ChannelManagerBase channelManager, IReplyChannel innerChannel, ContextExchangeMechanism contextExchangeMechanism)
      : base(channelManager, innerChannel)
    {
      this.contextExchangeMechanism = contextExchangeMechanism;
    }

    public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return this.InnerChannel.BeginReceiveRequest(timeout, callback, state);
    }

    public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
    {
      return this.InnerChannel.BeginReceiveRequest(callback, state);
    }

    public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return this.InnerChannel.BeginTryReceiveRequest(timeout, callback, state);
    }

    public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return this.InnerChannel.BeginWaitForRequest(timeout, callback, state);
    }

    public RequestContext EndReceiveRequest(IAsyncResult result)
    {
      RequestContext innerContext = this.InnerChannel.EndReceiveRequest(result);
      if (innerContext == null)
        return (RequestContext) null;
      else
        return (RequestContext) this.CreateContextChannelRequestContext(innerContext);
    }

    public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
    {
      context = (RequestContext) null;
      RequestContext context1;
      if (!this.InnerChannel.EndTryReceiveRequest(result, out context1))
        return false;
      if (context1 != null)
        context = (RequestContext) this.CreateContextChannelRequestContext(context1);
      return true;
    }

    public bool EndWaitForRequest(IAsyncResult result)
    {
      return this.InnerChannel.EndWaitForRequest(result);
    }

    public RequestContext ReceiveRequest(TimeSpan timeout)
    {
      RequestContext innerContext = this.InnerChannel.ReceiveRequest(timeout);
      if (innerContext == null)
        return (RequestContext) null;
      else
        return (RequestContext) this.CreateContextChannelRequestContext(innerContext);
    }

    public RequestContext ReceiveRequest()
    {
      RequestContext innerContext = this.InnerChannel.ReceiveRequest();
      if (innerContext == null)
        return (RequestContext) null;
      else
        return (RequestContext) this.CreateContextChannelRequestContext(innerContext);
    }

    public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
    {
      RequestContext context1;
      if (this.InnerChannel.TryReceiveRequest(timeout, out context1))
      {
        context = (RequestContext) this.CreateContextChannelRequestContext(context1);
        return true;
      }
      else
      {
        context = (RequestContext) null;
        return false;
      }
    }

    public bool WaitForRequest(TimeSpan timeout)
    {
      return this.InnerChannel.WaitForRequest(timeout);
    }

    private ContextChannelRequestContext CreateContextChannelRequestContext(RequestContext innerContext)
    {
      ServiceContextProtocol serviceContextProtocol = new ServiceContextProtocol(this.contextExchangeMechanism);
      serviceContextProtocol.OnIncomingMessage(innerContext.RequestMessage);
      return new ContextChannelRequestContext(innerContext, (ContextProtocol) serviceContextProtocol, this.DefaultSendTimeout);
    }
  }
}
