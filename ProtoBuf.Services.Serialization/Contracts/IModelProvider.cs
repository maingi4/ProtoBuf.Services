using System;
using ProtoBuf.Services.Wcf.Serialization;

namespace ProtoBuf.Services.Wcf.Contracts
{
    public interface IModelProvider
    {
        ModelInfo CreateModelInfo(Type type);
        ModelInfo CreateModelInfo(Type type, TypeMetaData metaData);
    }
}