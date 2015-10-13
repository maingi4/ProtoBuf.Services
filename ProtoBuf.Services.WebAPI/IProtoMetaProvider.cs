using System;

namespace ProtoBuf.Services.WebAPI
{
    public interface IProtoMetaProvider
    {
        string GetMetaData(Type type);
    }
}