using System;
using ProtoBuf.Wcf.Contracts;
using ProtoBuf.Wcf.Infrastructure;

namespace ProtoBuf.Wcf.Serialization
{
    public class ModelProvider : IModelProvider
    {
        #region IModelProvider Members

        public ModelInfo CreateModelInfo(Type type)
        {
            var modelInfo = GetModelInfoFromCache(type) ?? CreateNewModelInfo(type);

            return modelInfo;
        }

        public ModelInfo CreateModelInfo(Type type, TypeMetaData metaData)
        {
            var modelInfo = GetModelInfoFromCache(type) ?? CreateNewModelInfo(type, metaData);

            return modelInfo;
        }

        #endregion

        #region Protected Methods

        protected virtual ModelInfo GetModelInfoFromCache(Type type)
        {
            var store = ObjectBuilder.GetModelStore();

            return store.GetModel(type);
        }

        protected virtual ModelInfo CreateNewModelInfo(Type type)
        {
            return CreateNewModelInfo(type, null);
        }

        protected virtual ModelInfo CreateNewModelInfo(Type type, TypeMetaData metaData)
        {
            var modelGenerator = metaData == null ? 
                new ProtoBufModelGenerator(type) :
                new ProtoBufModelGenerator(type, metaData);

            var modelInfo = modelGenerator.ConfigureType(type, true, true);

            return modelInfo;
        }

        #endregion
    }
}
