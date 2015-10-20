using System;
using System.Collections.Generic;

namespace ProtoBuf.Services.WebAPI.Client
{
    public interface IWebClient
    {
        TRS SendRequest<TRS>(ProtoRequest protoRequest);
    }
}