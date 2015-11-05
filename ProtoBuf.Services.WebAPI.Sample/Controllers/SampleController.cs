using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using ProtoBuf.Services.WebAPI.Sample.Models;

namespace ProtoBuf.Services.WebAPI.Sample.Controllers
{
    public class SampleController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult Index()
        {
            return Ok(new SampleModel() {StringProp = "testVal", SubComplex1 = new SubSampleModel(){IntProp = 2}});
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult GetList()
        {
            const int resultCount = 10000;
            var samples = new List<SampleModel>(resultCount);

            for (int i = 0; i < resultCount; i++)
            {
                samples.Add(new SampleModel()
                                {
                                    StringProp = i.GetHashCode().ToString(CultureInfo.InvariantCulture), 
                                    SubComplex1 = new SubSampleModel() { IntProp = i }
                                });
            }

            return Ok(samples);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult Reverse(SampleModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(HttpStatusCode.NoContent, Request);
            }
            return Ok("ok");
        }
	}
}