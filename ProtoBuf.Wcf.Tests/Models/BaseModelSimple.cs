﻿using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models
{
    [DataContract]
    public class BaseModelSimple
    {
        [DataMember]
        public string TestBase1 { get; set; }

        [DataMember]
        public string TestBase2 { get; set; }
    }
}
