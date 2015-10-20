using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Services.Serialization
{
    internal static class JsonSerializer
    {
        public static T FromJson<T>(byte[] json)
        {
            using (var mem = new MemoryStream(json))
            {
                mem.Position = 0;

                var serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(mem);
            }
        }

        public static string ConvertToJson(object obj)
        {
            if (obj == null)
                return null;

            using (var mem = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());

                serializer.WriteObject(mem, obj);
                mem.Position = 0;
                using (var sr = new StreamReader(mem))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
