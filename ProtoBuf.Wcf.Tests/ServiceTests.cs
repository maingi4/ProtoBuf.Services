using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Wcf.Tests.TestService;

namespace ProtoBuf.Wcf.Tests
{
    [TestClass]
    public class ServiceTests
    {
        private static readonly BigContract BigContract;
        private const int BigContractSize = 6200;
        static ServiceTests()
        {
            BigContract = new BigContract();
            BigContract.CompositeTypes = new CompositeType[BigContractSize];
            for (var i = 0; i < BigContractSize; i++)
            {
                BigContract.CompositeTypes[i] = new CompositeType()
                    {
                        BoolValue = true,
                        StringValue = "Test"
                    };
            }
        }


        [TestMethod]
        public void BasicCommTest()
        {
            string response;
            var start = Environment.TickCount;
            using (var client = new TestServiceClient("proto"))
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

        [TestMethod]
        public void PrimitiveGetProto()
        {
            PrimitiveGet("proto");
        }

        [TestMethod]
        public void ComplexTypeGetProto()
        {
            ComplexTest("proto");
        }

        [TestMethod]
        public void PrimitiveGetBasicHttp()
        {
            PrimitiveGet("basic");
        }

        [TestMethod]
        public void ComplexTypeGetBasicHttp()
        {
            ComplexTest("basic");
        }

        [TestMethod]
        public void BigComplexTypeGetBasicHttp()
        {
            BigComplexTest("basic");
        }

        [TestMethod]
        public void BigComplexTypeGetProto()
        {
            BigComplexTest("proto");
        }

        private void ComplexTest(string bindingName)
        {
            CompositeType compositeType;
            using (var client = new TestServiceClient(bindingName))
            {
                compositeType = client.GetDataUsingDataContract(new CompositeType()
                {
                    BoolValue = true,
                    StringValue = "Test"
                });
            }

            AssertComposite(compositeType);
        }

        private void BigComplexTest(string bindingName)
        {
            BigContract bigContract;
            using (var client = new TestServiceClient(bindingName))
            {
                bigContract = client.GetDataUsingBigDataContract(BigContract);
            }

            Assert.IsNotNull(bigContract);
            Assert.IsNotNull(bigContract.CompositeTypes);
            foreach (var compositeType in bigContract.CompositeTypes)
            {
                AssertComposite(compositeType);
            }
        }

        private void AssertComposite(CompositeType compositeType)
        {
            Assert.IsNotNull(compositeType);
            Assert.IsTrue(compositeType.BoolValue);
            Assert.AreEqual("TestSuffix", compositeType.StringValue);
        }

        private void PrimitiveGet(string bindingName)
        {
            string response;
            using (var client = new TestServiceClient(bindingName))
            {
                response = client.GetData(2);
            }
            Assert.IsNotNull(response);

            Assert.IsTrue(response.Equals("2", StringComparison.Ordinal),
                "response was unexpected, expected : {0}, actual: {1}", "2", response);
        }
    }
}
