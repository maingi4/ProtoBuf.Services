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

        private static string ConvertToJson(TypeMetaData metaData)
        {
            using (var mem = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(TypeMetaData));

                serializer.WriteObject(mem, metaData);
                mem.Position = 0;
                using (var sr = new StreamReader(mem))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}