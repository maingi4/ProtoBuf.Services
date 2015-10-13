using System;

namespace ProtoBuf.Services.Infrastructure
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
