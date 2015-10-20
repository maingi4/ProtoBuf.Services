using System;
using System.IO;
using System.Runtime.Serialization.Json;
using ProtoBuf.Services.Serialization;

namespace ProtoBuf.Services.WebAPI
{
    public class JsonMetaDataProvider : IProtoMetaProvider
    {
        public string GetMetaData(Type type)
        {
            var modelProvider = ObjectBuilder.GetModelProvider();

            var modelInfo = modelProvider.CreateModelInfo(type);

            var metaData = modelInfo.MetaData;

            var result = ConvertToJson(metaData);

            return result;
        }

        public TypeMetaData FromJson(byte[] json)
        {
            return JsonSerializer.FromJson<TypeMetaData>(json);
        }

        private static string ConvertToJson(TypeMetaData metaData)
        {
            return JsonSerializer.ConvertToJson(metaData);
        }
    }
}