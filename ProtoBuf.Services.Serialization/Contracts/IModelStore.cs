using System;

namespace ProtoBuf.Services.Serialization.Contracts
{
    public interface IModelStore
    {
        ModelInfo GetModel(Type type);
        void SetModel(Type type, ModelInfo modelInfo);
        void RemoveModel(Type type);
        void RemoveAll();
    }
}