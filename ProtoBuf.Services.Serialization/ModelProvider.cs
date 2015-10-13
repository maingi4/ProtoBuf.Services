using System;
using ProtoBuf.Services.Serialization.Contracts;

namespace ProtoBuf.Services.Serialization
{
    internal sealed class ModelProvider : IModelProvider
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

        private ModelInfo GetModelInfoFromCache(Type type)
        {
            var store = ObjectBuilder.GetModelStore();

            return store.GetModel(type);
        }

        private void SetModelInfoIntoCache(Type type, ModelInfo modelInfo)
        {
            var store = ObjectBuilder.GetModelStore();

            store.SetModel(type, modelInfo);
        }

        private ModelInfo CreateNewModelInfo(Type type)
        {
            return CreateNewModelInfo(type, null);
        }

        private ModelInfo CreateNewModelInfo(Type type, TypeMetaData metaData)
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
