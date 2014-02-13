// Type: System.ServiceModel.Channels.ChannelListenerBase`1
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Provides a common base implementation for channel listeners on a service to accept channels produced by the client factories.
  /// </summary>
  /// <typeparam name="TChannel">The type of channel the channel listeners accept.</typeparam>
  public abstract class ChannelListenerBase<TChannel> : ChannelListenerBase, IChannelListener<TChannel>, IChannelListener, ICommunicationObject where TChannel : class, IChannel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.ChannelListenerBase`1"/> class.
    /// </summary>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ChannelListenerBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.ChannelListenerBase`1"/> class with specified default communication timeouts.
    /// </summary>
    /// <param name="timeouts">The <see cref="T:System.ServiceModel.IDefaultCommunicationTimeouts"/> that specify the default timeouts for open, send, receive, and close operations when exchanging messages.</param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ChannelListenerBase(IDefaultCommunicationTimeouts timeouts)
      : base(timeouts)
    {
    }

    /// <summary>
    /// When implemented in a derived class, provides an extensibility point when accepting a channel.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.IChannel"/> accepted.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the accept channel operation has to complete before timing out.</param>
    protected abstract TChannel OnAcceptChannel(TimeSpan timeout);

    /// <summary>
    /// When implemented in a derived class, provides an asynchronous extensibility point when beginning to accept a channel.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous accept channel operation.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the accept channel operation has to complete before timing out.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous completion of the accept channel operation.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous accept channel operation.</param>
    protected abstract IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// When implemented in a derived class, provides an asynchronous extensibility point when completing the acceptance a channel.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.IChannel"/> accepted by the listener.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="M:System.ServiceModel.Channels.ChannelListenerBase`1.OnBeginAcceptChannel(System.TimeSpan,System.AsyncCallback,System.Object)"/> method.</param>
    protected abstract TChannel OnEndAcceptChannel(IAsyncResult result);

    /// <summary>
    /// Accepts a channel of the type specified by the current channel listener.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.IChannel"/> accepted by the listener.
    /// </returns>
    public TChannel AcceptChannel()
    {
      return this.AcceptChannel(this.InternalReceiveTimeout);
    }

    /// <summary>
    /// Accepts a channel of the type specified by the current channel listener within a specified interval of time.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.IChannel"/> accepted by the listener.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the accept channel operation has to complete before timing out.</param>
    public TChannel AcceptChannel(TimeSpan timeout)
    {
      this.ThrowIfNotOpened();
      this.ThrowPending();
      return this.OnAcceptChannel(timeout);
    }

    /// <summary>
    /// Begins an asynchronous operation to accept a channel of the type specified by the current channel listener.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous accept channel operation.
    /// </returns>
    /// <param name="callback">The <see cref="T:System.Timespan"/> that specifies how long the accept channel operation has to complete before timing out.</param><param name="state">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous completion of the accept channel operation.</param>
    public IAsyncResult BeginAcceptChannel(AsyncCallback callback, object state)
    {
      return this.BeginAcceptChannel(this.InternalReceiveTimeout, callback, state);
    }

    /// <summary>
    /// When implemented in a derived class, begins an asynchronous operation to accept a channel of the type specified by the current channel listener within a specified interval of time.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous accept channel operation.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the accept channel operation has to complete before timing out.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous completion of the accept channel operation.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous accept channel operation.</param>
    public IAsyncResult BeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.ThrowIfNotOpened();
      this.ThrowPending();
      return this.OnBeginAcceptChannel(timeout, callback, state);
    }

    /// <summary>
    /// When implemented in a derived class, completes an asynchronous operation to accept a channel.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.IChannel"/> accepted by the listener.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="Overload:System.ServiceModel.Channels.ChannelListenerBase`1.BeginAcceptChannel"/> method.</param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public TChannel EndAcceptChannel(IAsyncResult result)
    {
      return this.OnEndAcceptChannel(result);
    }
  }
}
