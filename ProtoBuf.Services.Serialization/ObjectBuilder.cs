using ProtoBuf.Services.Serialization.Contracts;

namespace ProtoBuf.Services.Serialization
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
