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


        [TestMethod, TestCategory("ProtoService")]
        public void BasicCommTest()
        {
            string response;
            var start = Environment.TickCount;
            using (var client = new TestServiceClient("proto"))
            {
                var d = client.GetDataUsingDataContractAsync(new CompositeType()
                {
                    BoolValue = true,
                    StringValue = "Test"
                });

                var composite = d.GetAwaiter().GetResult();

                AssertComposite(composite);

                response = client.GetData(2);
            }
            var end = Environment.TickCount - start;

            Console.WriteLine(end);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Equals("2", StringComparison.Ordinal),
                "response was unexpected, expected : {0}, actual: {1}", "2", response);
        }

        [TestMethod, TestCategory("ProtoService")]
        public void ListGetProto()
        {
            using (var client = new TestServiceClient("proto"))
            {
                var list = client.GetList();

                Assert.IsNotNull(list);
            }
        }

        [TestMethod, TestCategory("ProtoService")]
        public void DictionaryComplexGetProto()
        {
            using (var client = new TestServiceClient("proto"))
            {
                var dictionary = client.GetDictionaryComplex();

                Assert.IsNotNull(dictionary);
            }
        }

        [TestMethod, TestCategory("ProtoService")]
        public void DictionarySimpleGetProto()
        {
            using (var client = new TestServiceClient("proto"))
            {
                var dictionary = client.GetDictionarySimple();

                Assert.IsNotNull(dictionary);
            }
        }

        [TestMethod, TestCategory("ProtoService")]
        public void DictionaryListComplexGetProto()
        {
            using (var client = new TestServiceClient("proto"))
            {
                var dictionary = client.GetDictionaryListComplex();

                Assert.IsNotNull(dictionary);
            }
        }

        //[TestMethod, TestCategory("ProtoService")]
        //public void ListDictionaryComplexGetProto()
        //{
        //    using (var client = new TestServiceClient("proto"))
        //    {
        //        var dictionary = client.GetListDictionaryComplex();

        //        Assert.IsNotNull(dictionary);
        //    }
        //}

        //[TestMethod, TestCategory("ProtoService")]
        //public void ListListComplexGetProto()
        //{
        //    using (var client = new TestServiceClient("proto"))
        //    {
        //        var dictionary = client.GetListListComplex();

        //        Assert.IsNotNull(dictionary);
        //    }
        //}

        [TestMethod, TestCategory("ProtoService")]
        public void DictionaryMixedGetProto()
        {
            using (var client = new TestServiceClient("proto"))
            {
                var dictionary = client.GetDictionaryMixed();

                Assert.IsNotNull(dictionary);
            }
        }

        [TestMethod, TestCategory("ProtoService")]
        public void PrimitiveGetProto()
        {
            PrimitiveGet("proto");
        }

        [TestMethod, TestCategory("ProtoService")]
        public void ComplexTypeGetProto()
        {
            ComplexTest("proto");
        }

        [TestMethod, TestCategory("BasicHttpService")]
        public void PrimitiveGetBasicHttp()
        {
            PrimitiveGet("basic");
        }

        [TestMethod, TestCategory("BasicHttpService")]
        public void ComplexTypeGetBasicHttp()
        {
            ComplexTest("basic");
        }

        [TestMethod, TestCategory("BasicHttpService")]
        public void BigComplexTypeGetBasicHttp()
        {
            BigComplexTest("basic");
        }

        [TestMethod, TestCategory("ProtoService")]
        public void BigComplexTypeGetProto()
        {
            BigComplexTest("proto");
        }

        private void ComplexTest(string bindingName)
        {
            var compositeType = new CompositeType()
                        {
                            BoolValue = true,
                            StringValue = "Test"
                        };
            try
            {
                using (var client = new TestServiceClient(bindingName))
                {
                    compositeType = client.GetDataUsingDataContract(compositeType);
                }
            }
            catch (Exception)
            {
                using (var client = new TestServiceClient(bindingName))
                {
                    compositeType = client.GetDataUsingDataContract(compositeType);
                }
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
