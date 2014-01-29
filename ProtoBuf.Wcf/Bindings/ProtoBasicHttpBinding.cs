using System;
using System.ServiceModel.Channels;

namespace ProtoBuf.Wcf.Bindings
{
    public class MetaDataDownloaderBindingElement : BindingElement
    {
        public override BindingElement Clone()
        {
            throw new NotImplementedException();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class MetaDataUploaderBindingElement : BindingElement
    {
        public override BindingElement Clone()
        {
            throw new NotImplementedException();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            throw new NotImplementedException();
        }
    }

    //public class ProtoBufEncodingElement : MessageEncodingBindingElement
    //{
    //    public ProtoBufEncodingElement()
    //    {

    //    }

    //    private ProtoBufEncodingElement(ProtoBufEncodingElement clone)
    //        : base(clone)
    //    {

    //    }

    //    public override BindingElement Clone()
    //    {
    //        return new ProtoBufEncodingElement(this);
    //    }

    //    public override MessageEncoderFactory CreateMessageEncoderFactory()
    //    {
    //        return new ProtoBugMessageEncoderFactory();
    //    }

    //    public override MessageVersion MessageVersion { get; set; }
    //}

    //public class ProtoBugMessageEncoderFactory : MessageEncoderFactory
    //{
    //    #region MessageEncoderFactory Members

    //    private MessageEncoder _messageEncoder;

    //    public override MessageEncoder Encoder
    //    {
    //        get
    //        {
    //            return _messageEncoder ??
    //                    (_messageEncoder = new BinaryMessageEncodingBindingElement());
    //        }
    //    }

    //    public override MessageVersion MessageVersion
    //    {
    //        get { return MessageVersion.None; }
    //    }

    //    public string MediaType
    //    {
    //        get { return null; }
    //    }

    //    #endregion
    //}

    //public class ProtoBufMessageEncoder : MessageEncoder
    //{
    //    #region Fields

    //    private readonly ProtoBugMessageEncoderFactory _protoBugMessageEncoderFactory;

    //    #endregion

    //    #region Constructors

    //    public ProtoBufMessageEncoder(ProtoBugMessageEncoderFactory protoBugMessageEncoderFactory)
    //    {
    //        _protoBugMessageEncoderFactory = protoBugMessageEncoderFactory;
    //    }

    //    #endregion

    //    #region MessageEncoder Members

    //    public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
    //    {
    //        var data = new byte[maxSizeOfHeaders];
    //        stream.Write(data, 0, data.Length);

    //        return Message.CreateMessage()
    //    }

    //    public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager,
    //        string contentType)
    //    {
    //        var msgContents = new byte[buffer.Count];

    //        Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);

    //        bufferManager.ReturnBuffer(buffer.Array);

    //        var stream = new MemoryStream(msgContents);

    //        return ReadMessage(stream, int.MaxValue);
    //    }

    //    public override void WriteMessage(Message message, Stream stream)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string ContentType
    //    {
    //        get { return "binary"; }
    //    }

    //    public override string MediaType
    //    {
    //        get
    //        {
    //            return _protoBugMessageEncoderFactory.MediaType;
    //        }
    //    }

    //    public override MessageVersion MessageVersion
    //    {
    //        get { return _protoBugMessageEncoderFactory.MessageVersion; }
    //    }

    //    #endregion
    //}
}
