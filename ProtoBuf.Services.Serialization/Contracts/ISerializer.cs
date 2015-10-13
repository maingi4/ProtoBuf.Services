using System;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface ISerializer
    {
        SerializationResult Serialize(object obj);
        SerializationResult Serialize(object obj, TypeMetaData metaData);
        T Deserialize<T>(byte[] data);
        T Deserialize<T>(byte[] data, TypeMetaData metaData);
        object Deserialize(byte[] data, TypeMetaData metaData, Type type);
    }
}