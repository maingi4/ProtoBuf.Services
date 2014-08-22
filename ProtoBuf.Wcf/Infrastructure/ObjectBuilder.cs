using ProtoBuf.Wcf.Channels.Contracts;
using ProtoBuf.Wcf.Channels.Serialization;

namespace ProtoBuf.Wcf.Channels.Infrastructure
{
    public static class ObjectBuilder //TODO: IOC container from configuration?
    {
        public static IModelProvider GetModelProvider()
        {
            return new ModelProvider();
        }

        public static IModelStore GetModelStore()
        {
            return new StaticModelStore();
        }

        public static ISerializer GetSerializer()
        {
            return new ProtoBufSerializer();
        }
    }
}
