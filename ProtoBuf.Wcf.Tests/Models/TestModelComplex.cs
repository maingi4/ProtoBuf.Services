using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Tests.Models
{
    [DataContract(Namespace = "proto")]
    public class TestModelComplex
    {
        [DataMember]
        public BaseModelSimple BaseModel { get; set; }

        [DataMember]
        public TestModelSimple TestModelSimple { get; set; }

        [DataMember]
        public List<ListItem> ListItems { get; set; }

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
