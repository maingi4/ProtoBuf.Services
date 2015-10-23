using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models.Counterparts
{
    [DataContract(Namespace = "proto")]
    public class ChildModelSimple : BaseModelSimple
    {
        [DataMember]
        public string TestChildProperty1 { get; set; }

        [DataMember]
        public string TestChildProperty2 { get; set; }
    }
}
