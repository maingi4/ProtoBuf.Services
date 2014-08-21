using System;
using ProtoBuf.Wcf.Channels.Serialization;

namespace ProtoBuf.Wcf.Channels.Contracts
{
    public interface IModelProvider
    {
        ModelInfo CreateModelInfo(Type type);
        ModelInfo CreateModelInfo(Type type, TypeMetaData metaData);
    }
}