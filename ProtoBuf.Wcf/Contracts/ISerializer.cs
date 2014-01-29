using ProtoBuf.Wcf.Serialization;

namespace ProtoBuf.Wcf.Contracts
{
    public interface ISerializer
    {
        SerializationResult Serialize(object obj);
        SerializationResult Serialize(object obj, TypeMetaData metaData);
        T Deserialize<T>(byte[] data);
        T Deserialize<T>(byte[] data, TypeMetaData metaData);
    }
}