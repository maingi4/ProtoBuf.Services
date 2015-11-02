using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Services.Infrastructure;
using ProtoBuf.Services.WebAPI.Client;
using ProtoBuf.Wcf.Tests.Models.WebAPI;

namespace ProtoBuf.Wcf.Tests
{
    [TestClass]
    public class WebApiTests
    {
        #region Basic Tests

        [TestMethod, TestCategory("WebAPI")]
        public void BasicCommTest()
        {
            var client = new ProtoBufWebClient();

            var response = client.SendRequest<SampleModel>(new ProtoRequest(new Uri("http://localhost:62965/api/Sample/"), null, "GET"));

            Assert.IsNotNull(response);
            Assert.AreEqual("testVal", response.StringProp);
            Assert.IsNotNull(response.SubComplex1);
            Assert.AreEqual(2, response.SubComplex1.IntProp);

            var responseHeaders = client.ResponseHeaders;

            AssertResponseHeaders(responseHeaders);

            var contentType = GetContentType(responseHeaders);

            AssertContentType(RestfulServiceConstants.ProtoContentType, contentType);
        }

        [TestMethod, TestCategory("WebAPI")]
        public void ListTest()
        {
            var client = new ProtoBufWebClient();

            var response = client.SendRequest<List<SampleModel>>(new ProtoRequest(new Uri("http://localhost:62965/api/Sample/getlist"), null, "GET"));

            Assert.IsNotNull(response);

            const int resultCount = 1000;

            Assert.AreEqual(resultCount, response.Count);

            for (int i = 0; i < resultCount; i++)
            {
                Assert.AreEqual(i.GetHashCode().ToString(CultureInfo.InvariantCulture), response[i].StringProp);
                Assert.IsNotNull(response[i].SubComplex1);
                Assert.AreEqual(i, response[i].SubComplex1.IntProp);
            }

            var responseHeaders = client.ResponseHeaders;

            AssertResponseHeaders(responseHeaders);

            var contentType = GetContentType(responseHeaders);

            AssertContentType(RestfulServiceConstants.ProtoContentType, contentType);
        }

        #endregion

        #region Helpers

        private static void AssertResponseHeaders(IDictionary<string, string> responseHeaders)
        {
            Assert.IsNotNull(responseHeaders, "response headers cannot be null");
            Assert.AreNotEqual(0, responseHeaders.Count, "response headers cannot be empty.");
        }

        private static string GetContentType(IDictionary<string, string> responseHeaders)
        {
            string contentType;
            if (responseHeaders.TryGetValue("Content-Type", out contentType))
                return contentType;

            Assert.Fail("content type was not found in response headers.");
            return null;
        }

        private static void AssertContentType(string expected, string actual)
        {
            Assert.AreEqual(expected, actual,
                "The content type returned was unexpected, expected {0}, we got {1}",
                expected, actual);
        }

        #endregion
    }
}
