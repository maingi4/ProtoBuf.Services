using System;
using ProtoBuf.Services.Wcf.Serialization;

namespace ProtoBuf.Services.Wcf.Contracts
{
    public interface IModelStore
    {
        ModelInfo GetModel(Type type);
        void SetModel(Type type, ModelInfo modelInfo);
        void RemoveModel(Type type);
        void RemoveAll();
    }
}