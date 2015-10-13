using System;

namespace ProtoBuf.Services.Infrastructure
{
    public sealed class TypeInfo
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public ParamType ParamType { get; set; }
    }

    public enum ParamType
    {
        Input = 0,
        Return = 1
    }
}