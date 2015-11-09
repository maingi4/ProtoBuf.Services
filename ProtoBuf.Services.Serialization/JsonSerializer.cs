using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProtoBuf.Services.Serialization
{
    internal static class JsonSerializer
    {
        public static T FromJson<T>(byte[] json)
        {
            if (json == null || json.Length == 0)
                return default(T);

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(json));
        }

        public static string ConvertToJson(object obj)
        {
            if (obj == null)
                return null;

            return JsonConvert.SerializeObject(obj);
        }
    }
}
