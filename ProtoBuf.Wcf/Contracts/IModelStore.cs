using System;
using ProtoBuf.Wcf.Channels.Serialization;

namespace ProtoBuf.Wcf.Channels.Contracts
{
    public interface IModelStore
    {
        ModelInfo GetModel(Type type);
        void SetModel(Type type, ModelInfo modelInfo);
        void RemoveModel(Type type);
        void RemoveAll();
    }
}