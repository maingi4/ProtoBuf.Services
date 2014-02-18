// Type: System.ServiceModel.Channels.ChannelManagerBase
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Provides a base implementation for managing the default timeouts that are associated with channel and listener factories.
  /// </summary>
  public abstract class ChannelManagerBase : CommunicationObject, IDefaultCommunicationTimeouts
  {
    /// <summary>
    /// When overridden in a derived class, gets the default interval of time a channel has to complete the reception of a message.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the channel has to complete the reception of a message before timing out once the receive method has been invoked.
    /// </returns>
    protected abstract TimeSpan DefaultReceiveTimeout { get; }

    /// <summary>
    /// When overridden in a derived class, gets the default interval of time a channel has to complete the sending of a message.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the channel has to complete the sending of a message before timing out once the send method has been invoked.
    /// </returns>
    protected abstract TimeSpan DefaultSendTimeout { get; }

    internal TimeSpan InternalReceiveTimeout
    {
      get
      {
        return this.DefaultReceiveTimeout;
      }
    }

    internal TimeSpan InternalSendTimeout
    {
      get
      {
        return this.DefaultSendTimeout;
      }
    }

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

    internal Exception CreateChannelTypeNotSupportedException(Type type)
    {
      return (Exception) new ArgumentException(System.ServiceModel.SR.GetString("ChannelTypeNotSupported", new object[1]
      {
        (object) type
      }), "TChannel");
    }
  }
}
