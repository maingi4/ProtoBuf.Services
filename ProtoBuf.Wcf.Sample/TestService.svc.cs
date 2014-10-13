using System;
using System.Collections.Generic;
using System.ServiceModel;
using ProtoBuf.Wcf.Sample.LongRunningService;

namespace ProtoBuf.Wcf.Sample
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    //[ExceptionCausingBehaviour]
    public class TestService : ITestService
    {
        public bool CallLongRunningService()
        {
            using (var client = new LongRunningClient())
            {
                client.DoWork();
            }

            return true;
        }

        public string GetData(int value)
        {
            if (value == 0)
                throw new ArgumentNullException("value");

            return string.Format("{0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
                throw new ArgumentNullException("composite");

            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }

            return composite;
        }

        public BigContract GetDataUsingBigDataContract(BigContract bigContract)
        {
            if (bigContract == null)
                throw new ArgumentNullException("bigContract");

            if (bigContract.CompositeTypes == null)
                throw new ArgumentNullException("bigContract.CompositeTypes");

            foreach (var compositeType in bigContract.CompositeTypes)
            {
                GetDataUsingDataContract(compositeType);
            }

            return bigContract;
        }

        public List<string> GetList()
        {
            return new List<string>() { "string", "test" };
        }

        public Dictionary<string, BigContract> GetDictionaryMixed()
        {
            return new Dictionary<string, BigContract>() { { "blah", new BigContract() }, { "blah2", new BigContract() } };
        }

        public Dictionary<string, string> GetDictionarySimple()
        {
            return new Dictionary<string, string>() { { "key", "value" }, { "key2", "value2" } };
        }

        public Dictionary<CompositeType, BigContract> GetDictionaryComplex()
        {
            return new Dictionary<CompositeType, BigContract>() { { new CompositeType(), new BigContract() }, { new CompositeType(), new BigContract() } };
        }

        public Dictionary<CompositeType, List<BigContract>> GetDictionaryListComplex()
        {
            return new Dictionary<CompositeType, List<BigContract>>()
                {
                    {new CompositeType(), new List<BigContract>() { new BigContract()}},
                    {new CompositeType(), new List<BigContract>() { new BigContract()}}
                };
        }

        //public List<Dictionary<CompositeType, List<BigContract>>> GetListDictionaryComplex()
        //{
        //    return new List<Dictionary<CompositeType, List<BigContract>>>()
        //        {
        //            new Dictionary<CompositeType, List<BigContract>>()
        //                {
        //                    {new CompositeType(), new List<BigContract>()},
        //                    {new CompositeType(), new List<BigContract>()}
        //                },
        //                new Dictionary<CompositeType, List<BigContract>>()
        //                {
        //                    {new CompositeType(), new List<BigContract>()},
        //                    {new CompositeType(), new List<BigContract>()}
        //                }
        //        };
        //}

        //public List<List<CompositeType>> GetListListComplex()
        //{
        //    return new List<List<CompositeType>>()
        //        {
        //            new List<CompositeType>()
        //                {
        //                    new CompositeType()
        //                },
        //                new List<CompositeType>()
        //                {
        //                    new CompositeType()
        //                }
        //        };
        //}
    }
}
