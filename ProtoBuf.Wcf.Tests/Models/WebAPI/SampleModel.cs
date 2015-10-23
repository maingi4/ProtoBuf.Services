using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Wcf.Tests.Models.WebAPI
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
