using System;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Exceptions
{
    [Serializable]
    public class ConfigurationException : Exception
    {
        #region Constructors

        public ConfigurationException()
        { }

        public ConfigurationException(string message)
            : base(message)
        { }

        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #endregion
    }
}
