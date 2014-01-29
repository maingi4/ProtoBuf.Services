using System;
using ProtoBuf.Wcf.Serialization;

namespace ProtoBuf.Wcf.Contracts
{
    public interface IModelProvider
    {
        ModelInfo CreateModelInfo(Type type);
        ModelInfo CreateModelInfo(Type type, TypeMetaData metaData);
    }
}