using System;
using ProtoBuf.Services.Infrastructure;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface IModelStore
    {
        ModelInfo GetModel(Type type, ModeType appMode);
        void SetModel(Type type, ModelInfo modelInfo, ModeType appMode);
        void RemoveModel(Type type, ModeType appMode);
        void RemoveAll();
    }
}