using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models
{
    [DataContract(Namespace = "proto")]
    public class ChildModelSimple : BaseModelSimple
    {
        [DataMember]
        public string ExtraProperty1 { get; set; }

        [DataMember]
        public string TestChildProperty1 { get; set; }

        [DataMember]
        public string TestChildProperty12 { get; set; }

        [DataMember]
        public string TestChildProperty2 { get; set; }
    }
}
