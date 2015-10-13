using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Services.Infrastructure
{
    internal static class AppMode
    {
        public enum ModeType
        {
            Wcf = 0,
            WebAPI = 1
        }

        public static ModeType Mode { get; set; }
    }
}
