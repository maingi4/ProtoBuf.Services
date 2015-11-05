using System;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface IModelProvider
    {
        ModelInfo CreateModelInfo(Type type, ModeType appMode);
        ModelInfo CreateModelInfo(Type type, TypeMetaData metaData, ModeType appMode);
    }
}