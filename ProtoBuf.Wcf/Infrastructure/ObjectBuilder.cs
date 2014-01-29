using ProtoBuf.Wcf.Contracts;
using ProtoBuf.Wcf.Serialization;

namespace ProtoBuf.Wcf.Infrastructure
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
