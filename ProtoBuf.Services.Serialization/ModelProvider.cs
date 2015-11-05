using System;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.Serialization.Contracts;

namespace ProtoBuf.Services.Serialization
{
    internal sealed class ModelProvider : IModelProvider
    {
        #region IModelProvider Members

        public ModelInfo CreateModelInfo(Type type, ModeType appMode)
        {
            var modelInfo = GetModelInfoFromCache(type, appMode);

            if (modelInfo == null)
            {
                modelInfo = CreateNewModelInfo(type, appMode);

                SetModelInfoIntoCache(type, modelInfo, appMode);
            }

            return modelInfo;
        }

        public ModelInfo CreateModelInfo(Type type, TypeMetaData metaData, ModeType appMode)
        {
            var modelInfo = GetModelInfoFromCache(type, appMode);

            if (modelInfo == null)
            {
                modelInfo = CreateNewModelInfo(type, metaData, appMode);

                SetModelInfoIntoCache(type, modelInfo, appMode);
            }

            return modelInfo;
        }

        #endregion

        #region Protected Methods

        private ModelInfo GetModelInfoFromCache(Type type, ModeType appMode)
        {
            var store = ObjectBuilder.GetModelStore();

            return store.GetModel(type, appMode);
        }

        private void SetModelInfoIntoCache(Type type, ModelInfo modelInfo, ModeType appMode)
        {
            var store = ObjectBuilder.GetModelStore();

            store.SetModel(type, modelInfo, appMode);
        }

        private ModelInfo CreateNewModelInfo(Type type, ModeType appMode)
        {
            return CreateNewModelInfo(type, null, appMode);
        }

        private ModelInfo CreateNewModelInfo(Type type, TypeMetaData metaData, ModeType appMode)
        {
            var modelGenerator = metaData == null ? 
                new ProtoBufModelGenerator(type) :
                new ProtoBufModelGenerator(type, metaData);

            var modelInfo = modelGenerator.ConfigureType(type, true, true, appMode);

            return modelInfo;
        }

        #endregion
    }
}
