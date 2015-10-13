using System;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface IModelProvider
    {
        ModelInfo CreateModelInfo(Type type);
        ModelInfo CreateModelInfo(Type type, TypeMetaData metaData);
    }
}