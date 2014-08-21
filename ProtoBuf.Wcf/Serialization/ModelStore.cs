using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ProtoBuf.Wcf.Channels.Contracts;

namespace ProtoBuf.Wcf.Channels.Serialization
{
    public class StaticModelStore : IModelStore
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
    }
}
