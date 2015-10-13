using System;
using System.Runtime.Serialization;

namespace ProtoBuf.Services.Wcf.Exceptions
{
    [Serializable]
    public class SerializationException : Exception
    {
        #region Constructors

        public SerializationException()
        { }

        public SerializationException(string message)
            : base(message)
        { }

        public SerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public SerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #endregion
    }
}
