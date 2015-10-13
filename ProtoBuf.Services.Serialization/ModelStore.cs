using System;
using System.Collections.Concurrent;
using ProtoBuf.Services.Wcf.Contracts;

namespace ProtoBuf.Services.Wcf.Serialization
{
    internal sealed class StaticModelStore : IModelStore
    {
        private static readonly ConcurrentDictionary<Type, ModelInfo> InternalStorage = new ConcurrentDictionary<Type, ModelInfo>();

        public ModelInfo GetModel(Type type)
        {
            ModelInfo modelInfo;

            InternalStorage.TryGetValue(type, out modelInfo);

            return modelInfo;
        }

        public void SetModel(Type type, ModelInfo modelInfo)
        {
            InternalStorage.AddOrUpdate(type, modelInfo, (type1, info) => info);
        }

        public void RemoveModel(Type type)
        {
            ModelInfo modelInfo;
            InternalStorage.TryRemove(type, out modelInfo);
        }

        public void RemoveAll()
        {
            foreach (var type in InternalStorage.Keys)
            {
                RemoveModel(type);
            }
        }
    }
}
