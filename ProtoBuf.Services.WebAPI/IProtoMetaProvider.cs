using System;
using ProtoBuf.Services.Serialization;

namespace ProtoBuf.Services.WebAPI
{
    public interface IProtoMetaProvider
    {
        string GetMetaData(Type type);
        TypeMetaData FromJson(byte[] json);
    }
}