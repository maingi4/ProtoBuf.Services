using System;
using System.Collections.Generic;
using ProtoBuf.Wcf.Contracts;

namespace ProtoBuf.Wcf.Serialization
{
    public class StaticModelStore : IModelStore
    {
        private static readonly IDictionary<Type, ModelInfo> InternalStorage = new Dictionary<Type, ModelInfo>();

        public ModelInfo GetModel(Type type)
        {
            ModelInfo modelInfo;

            InternalStorage.TryGetValue(type, out modelInfo);

            return modelInfo;
        }
    }
}
