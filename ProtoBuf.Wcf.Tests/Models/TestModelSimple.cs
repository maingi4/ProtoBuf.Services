using System;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models
{
    [DataContract]
    public class TestModelSimple
    {
        [DataMember]
        public string TestProperty1 { get; set; }

        [DataMember]
        public int TestProperty2 { get; set; }

        [DataMember]
        public DateTime OtherFieldInfo;
    }
}