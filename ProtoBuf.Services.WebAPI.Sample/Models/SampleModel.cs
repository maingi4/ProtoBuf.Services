using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtoBuf.Services.WebAPI.Sample.Models
{
    public class SampleModel
    {
        public string StringProp { get; set; }

        public SubSampleModel SubComplex1 { get; set; }
    }

    public class SubSampleModel
    {
        public int IntProp { get; set; }
    }
}