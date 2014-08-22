using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Channels.Infrastructure
{
    internal static class BinaryConverter
    {
        public static string ToString(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] FromString(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
