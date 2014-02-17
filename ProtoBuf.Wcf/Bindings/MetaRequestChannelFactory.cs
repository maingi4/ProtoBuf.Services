using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public class MetaRequestChannelFactory : ChannelFactoryBase<IRequestChannel>
    {
        private readonly IChannelFactory<IRequestChannel> _innerFactory;

        public MetaRequestChannelFactory(IChannelFactory<IRequestChannel> innerFactory)
        {
            _innerFactory = innerFactory;
        }

        #region ChannelFactoryBase Members

        protected override IRequestChannel OnCreateChannel(EndpointAddress address, Uri via)
        {
            var innerChannel = _innerFactory.CreateChannel(address, via);

            return WrapChannel(innerChannel);
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return _innerFactory.BeginOpen(timeout, callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            _innerFactory.EndOpen(result);
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            _innerFactory.Open();
        }

        #endregion

        #region Protected Methods

        protected IRequestChannel WrapChannel(IRequestChannel innerChannel)
        {
            return new ProtoBufMetaDataRequestChannel(this, innerChannel);
        }

        #endregion
    }
}