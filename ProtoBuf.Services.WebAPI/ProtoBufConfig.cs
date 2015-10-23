namespace ProtoBuf.Services.WebAPI
{
    /// <summary>
    /// Configuration details for protobuf behaviour
    /// </summary>
    public sealed class ProtoBufConfig
    {
        public string PathPrefix { get; set; }
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Inits the protobuf config object.
        /// </summary>
        /// <param name="encryptionKey">The 16 digit key used for encryption model headers, 
        /// leaving this blank may be a security threat as the server side model's fully qualified assembly name 
        /// will get passed in clear text. 
        /// Ensure that all the servers load balanced for this API uses the same encryption key.</param>
        public ProtoBufConfig(string encryptionKey)
        {
            PathPrefix = "api";
        }
    }
}