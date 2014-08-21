using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Wcf.Tests.TestService;

namespace ProtoBuf.Wcf.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void BasicCommTest()
        {
            string response;
            using (var client = new TestServiceClient())
            {
                response = client.GetData(2);

                var d = client.GetDataUsingDataContract(new CompositeType()
                    {
                        BoolValue = true
                    });
            }

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Equals("2", StringComparison.Ordinal), 
                "response was unexpected, expected : {0}, actual: {1}", "2", response);
        }
    }
}
