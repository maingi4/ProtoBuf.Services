using System;

namespace ProtoBuf.Wcf.Sample
{
    public class TestService : ITestService
    {
        public string GetData(int value)
        {
            return string.Format("{0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
