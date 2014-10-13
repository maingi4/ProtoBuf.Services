using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ProtoBuf.Wcf.Sample
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        bool CallLongRunningService();

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        BigContract GetDataUsingBigDataContract(BigContract bigContract);

        [OperationContract]
        List<string> GetList();

        [OperationContract]
        Dictionary<string, BigContract> GetDictionaryMixed();

        [OperationContract]
        Dictionary<string, string> GetDictionarySimple();

        [OperationContract]
        Dictionary<CompositeType, BigContract> GetDictionaryComplex();

        [OperationContract]
        Dictionary<CompositeType, List<BigContract>> GetDictionaryListComplex();

        //[OperationContract]
        //List<Dictionary<CompositeType, List<BigContract>>> GetListDictionaryComplex();

        //[OperationContract]
        //List<List<CompositeType>> GetListListComplex();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class BigContract
    {
        [DataMember]
        public List<CompositeType> CompositeTypes { get; set; }
    }
}
