using System;
using System.ServiceModel.Channels;
using System.Threading;

namespace ProtoBuf.Wcf.Channels.Infrastructure
{
    internal sealed class ChainedAsyncResult : IAsyncResult
    {
        private readonly IAsyncResult _original;
        private readonly IReplyChannel _channel;
        private readonly AsyncCallback _originalCallback;
        private readonly object _originalState;

        public bool IsCompleted { get { return _original.IsCompleted; } }
        public WaitHandle AsyncWaitHandle { get { return _original.AsyncWaitHandle; } }
        public object AsyncState { get { return _original.AsyncState; } }
        public bool CompletedSynchronously { get { return _original.CompletedSynchronously; } }


        public ChainedAsyncResult(IReplyChannel channel, TimeSpan timeout, AsyncCallback originalCallback,
            object originalState)
        {
            _channel = channel;
            _originalCallback = originalCallback;
            _originalState = originalState;

            _original = _channel.BeginTryReceiveRequest(timeout, Callback, originalState);
        }

        private void Callback(IAsyncResult ar)
        {
            _originalCallback(ar);
        }
    }
}
