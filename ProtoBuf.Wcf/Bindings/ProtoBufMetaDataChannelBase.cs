using System;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public abstract class ProtoBufMetaDataChannelBase : ChannelBase
    {
        private readonly IChannel _innerChannel;

        protected ProtoBufMetaDataChannelBase(ChannelManagerBase parent,
                                              IChannel innerChannel)
            : base(parent)
        {
            _innerChannel = innerChannel;
        }

        #region ChannelBase Members

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginClose(timeout, callback, state);
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerChannel.Open(timeout);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerChannel.BeginOpen(timeout, callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            _innerChannel.EndOpen(result);
        }

        protected override void OnClose(TimeSpan timeout)
        {
            _innerChannel.Close(timeout);
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            _innerChannel.EndClose(result);
        }

        protected override void OnAbort()
        {
            _innerChannel.Abort();
        }

        #endregion
    }
}