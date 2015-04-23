using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models.Counterparts
{
    [DataContract(Namespace = "proto")]
    public class TestModelComplex
    {
        [DataMember]
        public string AAExtraProperty1 { get; set; }

        [DataMember]
        public BaseModelSimple BaseModel { get; set; }

        [DataMember]
        public string AAExtraProperty2 { get; set; }

        [DataMember]
        public string CCExtraProperty3 { get; set; }

        [DataMember]
        public TestModelSimple TestModelSimple { get; set; }

        [DataMember]
        public List<ListItem> ListItems { get; set; }

        [DataMember]
        public string ZZExtraProperty4 { get; set; }

        public ShouldNotBeCarriedForward ShouldNotBeCarriedForward { get; set; }
    }

    [DataContract(Namespace = "proto")]
    public class ShouldNotBeCarriedForward
    {
        [DataMember]
        public string Blah { get; set; }
    }

    [DataContract(Namespace = "proto")]
    public class ListItem
    {
        [DataMember]
        public string Item { get; set; }
    }
}
