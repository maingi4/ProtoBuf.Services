using System;
using ProtoBuf.Wcf.Serialization;

namespace ProtoBuf.Wcf.Contracts
{
    public interface IModelStore
    {
        ModelInfo GetModel(Type type);
    }
}