// Type: System.ServiceModel.Channels.TransportChannelFactory`1
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.ObjectModel;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  internal abstract class TransportChannelFactory<TChannel> : ChannelFactoryBase<TChannel>, ITransportFactorySettings, IDefaultCommunicationTimeouts
  {
    private BufferManager bufferManager;
    private long maxBufferPoolSize;
    private long maxReceivedMessageSize;
    private MessageEncoderFactory messageEncoderFactory;
    private bool manualAddressing;
    private MessageVersion messageVersion;

    public BufferManager BufferManager
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.bufferManager;
      }
    }

    public long MaxBufferPoolSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxBufferPoolSize;
      }
    }

    public long MaxReceivedMessageSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.maxReceivedMessageSize;
      }
    }

    public MessageEncoderFactory MessageEncoderFactory
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.messageEncoderFactory;
      }
    }

    public MessageVersion MessageVersion
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.messageVersion;
      }
    }

    public bool ManualAddressing
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.manualAddressing;
      }
    }

    public abstract string Scheme { get; }

    long ITransportFactorySettings.MaxReceivedMessageSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.MaxReceivedMessageSize;
      }
    }

    BufferManager ITransportFactorySettings.BufferManager
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.BufferManager;
      }
    }

    bool ITransportFactorySettings.ManualAddressing
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.ManualAddressing;
      }
    }

    MessageEncoderFactory ITransportFactorySettings.MessageEncoderFactory
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.MessageEncoderFactory;
      }
    }

    protected TransportChannelFactory(TransportBindingElement bindingElement, BindingContext context)
      : this(bindingElement, context, TransportDefaults.GetDefaultMessageEncoderFactory())
    {
    }

    protected TransportChannelFactory(TransportBindingElement bindingElement, BindingContext context, MessageEncoderFactory defaultMessageEncoderFactory)
      : base((IDefaultCommunicationTimeouts) context.Binding)
    {
      this.manualAddressing = bindingElement.ManualAddressing;
      this.maxBufferPoolSize = bindingElement.MaxBufferPoolSize;
      this.maxReceivedMessageSize = bindingElement.MaxReceivedMessageSize;
      Collection<MessageEncodingBindingElement> all = context.BindingParameters.FindAll<MessageEncodingBindingElement>();
      if (all.Count > 1)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MultipleMebesInParameters")));
      if (all.Count == 1)
      {
        this.messageEncoderFactory = all[0].CreateMessageEncoderFactory();
        context.BindingParameters.Remove<MessageEncodingBindingElement>();
      }
      else
        this.messageEncoderFactory = defaultMessageEncoderFactory;
      if (this.messageEncoderFactory != null)
        this.messageVersion = this.messageEncoderFactory.MessageVersion;
      else
        this.messageVersion = MessageVersion.None;
    }

    public override T GetProperty<T>()
    {
      if (typeof (T) == typeof (MessageVersion))
        return (T) this.MessageVersion;
      if (typeof (T) == typeof (FaultConverter))
      {
        if (this.MessageEncoderFactory == null)
          return default (T);
        else
          return this.MessageEncoderFactory.Encoder.GetProperty<T>();
      }
      else if (typeof (T) == typeof (ITransportFactorySettings))
        return (T) this;
      else
        return base.GetProperty<T>();
    }

    protected override void OnAbort()
    {
      this.OnCloseOrAbort();
      base.OnAbort();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      this.OnCloseOrAbort();
      return base.OnBeginClose(timeout, callback, state);
    }

    protected override void OnClose(TimeSpan timeout)
    {
      this.OnCloseOrAbort();
      base.OnClose(timeout);
    }

    private void OnCloseOrAbort()
    {
      if (this.bufferManager == null)
        return;
      this.bufferManager.Clear();
    }

    internal virtual int GetMaxBufferSize()
    {
      if (this.MaxReceivedMessageSize > (long) int.MaxValue)
        return int.MaxValue;
      else
        return (int) this.MaxReceivedMessageSize;
    }

    protected override void OnOpening()
    {
      base.OnOpening();
      this.bufferManager = BufferManager.CreateBufferManager(this.MaxBufferPoolSize, this.GetMaxBufferSize());
    }

    internal void ValidateScheme(Uri via)
    {
      if (!(via.Scheme != this.Scheme) || string.Compare(via.Scheme, this.Scheme, StringComparison.OrdinalIgnoreCase) == 0)
        return;
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("via", System.ServiceModel.SR.GetString("InvalidUriScheme", (object) via.Scheme, (object) this.Scheme));
    }
  }
}
