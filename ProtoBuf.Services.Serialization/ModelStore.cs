using System;
using System.Collections.Concurrent;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Serialization.Contracts;

namespace ProtoBuf.Services.Serialization
{
    internal sealed class StaticModelStore : IModelStore
    {
        private static readonly ConcurrentDictionary<string, ModelInfo> InternalStorage = new ConcurrentDictionary<string, ModelInfo>();

        public ModelInfo GetModel(Type type, ModeType appMode)
        {
            ModelInfo modelInfo;

            InternalStorage.TryGetValue(GetKey(type, appMode), out modelInfo);

            return modelInfo;
        }

        public void SetModel(Type type, ModelInfo modelInfo, ModeType appMode)
        {
            InternalStorage.AddOrUpdate(GetKey(type, appMode), modelInfo, (type1, info) => info);
        }

        public void RemoveModel(Type type, ModeType appMode)
        {
            ModelInfo modelInfo;
            InternalStorage.TryRemove(GetKey(type, appMode), out modelInfo);
        }

        public void RemoveAll()
        {
            foreach (var key in InternalStorage.Keys)
            {
                ModelInfo modelInfo;
                InternalStorage.TryRemove(key, out modelInfo);
            }
        }

        private static string GetKey(Type type, ModeType appMode)
        {
            return string.Join(string.Empty, type.FullName, appMode == ModeType.Wcf ? "-w" : "-a");
        }
    }
}
