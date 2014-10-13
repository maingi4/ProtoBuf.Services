using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProtoBuf.Wcf.Sample
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LongRunning" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LongRunning.svc or LongRunning.svc.cs at the Solution Explorer and start debugging.
    public class LongRunning : ILongRunning
    {
        public void DoWork()
        {
           
        }
    }
}
