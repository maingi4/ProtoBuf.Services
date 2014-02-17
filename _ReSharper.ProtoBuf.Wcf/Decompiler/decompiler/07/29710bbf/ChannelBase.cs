// Type: System.ServiceModel.Channels.ChannelBase
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Provides the base implementation for custom channels.
  /// </summary>
  public abstract class ChannelBase : CommunicationObject, IChannel, ICommunicationObject, IDefaultCommunicationTimeouts
  {
    private ChannelManagerBase channelManager;

    TimeSpan IDefaultCommunicationTimeouts.CloseTimeout
    {
      get
      {
        return this.DefaultCloseTimeout;
      }
    }

    TimeSpan IDefaultCommunicationTimeouts.OpenTimeout
    {
      get
      {
        return this.DefaultOpenTimeout;
      }
    }

    TimeSpan IDefaultCommunicationTimeouts.ReceiveTimeout
    {
      get
      {
        return this.DefaultReceiveTimeout;
      }
    }

    TimeSpan IDefaultCommunicationTimeouts.SendTimeout
    {
      get
      {
        return this.DefaultSendTimeout;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for a close operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the close operation has to complete before timing out.
    /// </returns>
    protected override TimeSpan DefaultCloseTimeout
    {
      get
      {
        return this.channelManager.CloseTimeout;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for an open operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the open operation has to complete before timing out.
    /// </returns>
    protected override TimeSpan DefaultOpenTimeout
    {
      get
      {
        return this.channelManager.OpenTimeout;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for a receive operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the receive operation has to complete before timing out.
    /// </returns>
    protected TimeSpan DefaultReceiveTimeout
    {
      get
      {
        return this.channelManager.ReceiveTimeout;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for a send operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the send operation has to complete before timing out.
    /// </returns>
    protected TimeSpan DefaultSendTimeout
    {
      get
      {
        return this.channelManager.SendTimeout;
      }
    }

    /// <summary>
    /// Gets the channel manager that is associated with the current channel.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.ChannelManagerBase"/> that is associated with the current channel.
    /// </returns>
    protected ChannelManagerBase Manager
    {
      get
      {
        return this.channelManager;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.ChannelBase"/> class.
    /// </summary>
    /// <param name="channelManager">The <see cref="T:System.ServiceModel.Channels.ChannelManagerBase"/> that provides default timeouts for the channel operations (send, receive, open, and close).</param><exception cref="T:System.ArgumentNullException"><paramref name="channelManager"/> is null.</exception>
    protected ChannelBase(ChannelManagerBase channelManager)
    {
      if (channelManager == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("channelManager");
      this.channelManager = channelManager;
      if (!DiagnosticUtility.ShouldTraceVerbose)
        return;
      DiagnosticUtility.DiagnosticTrace.TraceEvent(TraceEventType.Verbose, TraceCode.ChannelCreated, System.ServiceModel.SR.GetString("TraceCodeChannelCreated", new object[1]
      {
        (object) DiagnosticTrace.CreateSourceString((object) this)
      }), (TraceRecord) null, (Exception) null, (object) this);
    }

    /// <summary>
    /// Returns the typed object requested, if present, from the appropriate layer in the channel stack.
    /// </summary>
    /// 
    /// <returns>
    /// The typed object <paramref name="T"/> requested, if it is present, or null, if it is not.
    /// </returns>
    /// <typeparam name="T">The typed object for which the method is querying.</typeparam>
    public virtual T GetProperty<T>() where T : class
    {
      IChannelFactory channelFactory = this.channelManager as IChannelFactory;
      if (channelFactory != null)
        return channelFactory.GetProperty<T>();
      IChannelListener channelListener = this.channelManager as IChannelListener;
      if (channelListener != null)
        return channelListener.GetProperty<T>();
      else
        return default (T);
    }

    /// <summary>
    /// Uses diagnostic tracing during the transition into the closing state.
    /// </summary>
    protected override void OnClosed()
    {
      base.OnClosed();
      if (!DiagnosticUtility.ShouldTraceVerbose)
        return;
      DiagnosticUtility.DiagnosticTrace.TraceEvent(TraceEventType.Verbose, TraceCode.ChannelDisposed, System.ServiceModel.SR.GetString("TraceCodeChannelDisposed", new object[1]
      {
        (object) DiagnosticTrace.CreateSourceString((object) this)
      }), (TraceRecord) null, (Exception) null, (object) this);
    }
  }
}
