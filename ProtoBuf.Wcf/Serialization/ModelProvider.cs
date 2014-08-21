using System;
using ProtoBuf.Wcf.Channels.Contracts;
using ProtoBuf.Wcf.Channels.Infrastructure;

namespace ProtoBuf.Wcf.Channels.Serialization
{
    public class ModelProvider : IModelProvider
    {
        #region IModelProvider Members

        public ModelInfo CreateModelInfo(Type type)
        {
            var modelInfo = GetModelInfoFromCache(type);

            if (modelInfo == null)
            {
                modelInfo = CreateNewModelInfo(type);

                SetModelInfoIntoCache(type, modelInfo);
            }

            return modelInfo;
        }

        public ModelInfo CreateModelInfo(Type type, TypeMetaData metaData)
        {
            var modelInfo = GetModelInfoFromCache(type);

            if (modelInfo == null)
            {
                modelInfo = CreateNewModelInfo(type, metaData);

                SetModelInfoIntoCache(type, modelInfo);
            }

            return modelInfo;
        }

        #endregion

        #region Protected Methods

        protected virtual ModelInfo GetModelInfoFromCache(Type type)
        {
            var store = ObjectBuilder.GetModelStore();

            return store.GetModel(type);
        }

        protected virtual void SetModelInfoIntoCache(Type type, ModelInfo modelInfo)
        {
            var store = ObjectBuilder.GetModelStore();

            store.SetModel(type, modelInfo);
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
