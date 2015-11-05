using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProtoBuf.Services.WebAPI.Sample.Models;

namespace ProtoBuf.Services.WebAPI.Sample.Controllers
{
    public class OtherController : ApiController
    {
        [AcceptVerbs("GET", "POST")]
        public IHttpActionResult Index()
        {
            return Ok(new SampleModel() { StringProp = "testVal", SubComplex1 = new SubSampleModel() { IntProp = 2 } });
        }
    }
}
