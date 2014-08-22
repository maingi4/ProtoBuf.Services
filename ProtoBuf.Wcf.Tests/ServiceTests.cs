using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Wcf.Tests.TestService;

namespace ProtoBuf.Wcf.Tests
{
    [TestClass]
    public class ServiceTests
    {
        [TestInitialize]
        public void Init()
        {
            BasicCommTest();
        }

        [TestMethod]
        public void BasicCommTest()
        {
            string response;
            var start = Environment.TickCount;
            using (var client = new TestServiceClient())
            {
                var d = client.GetDataUsingDataContract(new CompositeType()
                {
                    BoolValue = true
                });

                response = client.GetData(2);

                
            }
            var end = Environment.TickCount - start;

            Console.WriteLine(end);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Equals("2", StringComparison.Ordinal), 
                "response was unexpected, expected : {0}, actual: {1}", "2", response);
        }
    }
}
