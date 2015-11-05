using System;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface ISerializer
    {
        SerializationResult Serialize(object obj, ModeType appMode);
        SerializationResult Serialize(object obj, TypeMetaData metaData, ModeType appMode);
        T Deserialize<T>(byte[] data, ModeType appMode);
        T Deserialize<T>(byte[] data, TypeMetaData metaData, ModeType appMode);
        object Deserialize(byte[] data, TypeMetaData metaData, Type type, ModeType appMode);
    }
}