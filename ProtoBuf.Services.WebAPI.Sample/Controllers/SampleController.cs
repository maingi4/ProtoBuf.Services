using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ProtoBuf.Services.WebAPI.Sample.Models;

namespace ProtoBuf.Services.WebAPI.Sample.Controllers
{
    public class SampleController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IHttpActionResult Index()
        {
            return Ok(new SampleModel() {StringProp = "testVal"});
        }
	}
}