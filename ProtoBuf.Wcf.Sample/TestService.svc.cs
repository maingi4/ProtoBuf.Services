using System;
using System.ServiceModel;

namespace ProtoBuf.Wcf.Sample
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TestService : ITestService
    {
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
    }
}
