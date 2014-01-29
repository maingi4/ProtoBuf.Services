namespace ProtoBuf.Wcf.Serialization
{
    public sealed class SerializationResult
    {
        public byte[] Data { get; set; }
        public TypeMetaData MetaData { get; set; }

        internal SerializationResult(byte[] data, TypeMetaData metaData)
        {
            Data = data;
            MetaData = metaData;
        }
    }
}
