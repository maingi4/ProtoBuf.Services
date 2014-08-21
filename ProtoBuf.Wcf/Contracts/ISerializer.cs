using ProtoBuf.Wcf.Channels.Serialization;

namespace ProtoBuf.Wcf.Channels.Contracts
{
    public interface ISerializer
    {
        SerializationResult Serialize(object obj);
        SerializationResult Serialize(object obj, TypeMetaData metaData);
        T Deserialize<T>(byte[] data);
        T Deserialize<T>(byte[] data, TypeMetaData metaData);
    }
}