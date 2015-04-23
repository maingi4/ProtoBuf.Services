using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models
{
    [DataContract(Namespace = "proto")]
    public class TestModelSimple
    {
        [DataMember]
        public string TestProperty1 { get; set; }

        [DataMember]
        public int TestProperty2 { get; set; }

        [DataMember]
        public DateTime OtherFieldInfo;
        
        [DataMember]
        public TestEnum TestEnum { get; set; }

        [DataMember]
        public List<int> Ints { get; set; }
    }

    [DataContract(Namespace = "proto")]
    public enum TestEnum
    {
        [EnumMember]
        Nothing = 0,
        [EnumMember]
        Something = 1,
        Secret = 2
    }
}