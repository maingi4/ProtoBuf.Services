// Type: ProtoBuf.ServiceModel.XmlProtoSerializer
// Assembly: protobuf-net, Version=2.0.0.668, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// Assembly location: D:\Research\ProtoBuf.Wcf\ProtoBuf.Wcf\bin\Debug\protobuf-net.dll

using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// An xml object serializer that can embed protobuf data in a base-64 hunk (looking like a byte[])
  /// 
  /// </summary>
  public sealed class XmlProtoSerializer : XmlObjectSerializer
  {
    private const string PROTO_ELEMENT = "proto";
    private readonly TypeModel model;
    private readonly int key;
    private readonly bool isList;
    private readonly bool isEnum;
    private readonly Type type;

    internal XmlProtoSerializer(TypeModel model, int key, Type type, bool isList)
    {
      if (model == null)
        throw new ArgumentNullException("model");
      if (key < 0)
        throw new ArgumentOutOfRangeException("key");
      if (type == null)
        throw new ArgumentOutOfRangeException("type");
      this.model = model;
      this.key = key;
      this.isList = isList;
      this.type = type;
      this.isEnum = Helpers.IsEnum(type);
    }

    /// <summary>
    /// Creates a new serializer for the given model and type
    /// 
    /// </summary>
    public XmlProtoSerializer(TypeModel model, Type type)
    {
      if (model == null)
        throw new ArgumentNullException("model");
      if (type == null)
        throw new ArgumentNullException("type");
      this.key = XmlProtoSerializer.GetKey(model, ref type, out this.isList);
      this.model = model;
      this.type = type;
      this.isEnum = Helpers.IsEnum(type);
      if (this.key < 0)
        throw new ArgumentOutOfRangeException("type", "Type not recognised by the model: " + type.FullName);
    }

    /// <summary>
    /// Attempt to create a new serializer for the given model and type
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A new serializer instance if the type is recognised by the model; null otherwise
    /// </returns>
    public static XmlProtoSerializer TryCreate(TypeModel model, Type type)
    {
      if (model == null)
        throw new ArgumentNullException("model");
      if (type == null)
        throw new ArgumentNullException("type");
      bool isList;
      int key = XmlProtoSerializer.GetKey(model, ref type, out isList);
      if (key >= 0)
        return new XmlProtoSerializer(model, key, type, isList);
      else
        return (XmlProtoSerializer) null;
    }

    private static int GetKey(TypeModel model, ref Type type, out bool isList)
    {
      if (model != null && type != null)
      {
        int key1 = model.GetKey(ref type);
        if (key1 >= 0)
        {
          isList = false;
          return key1;
        }
        else
        {
          Type listItemType = TypeModel.GetListItemType(model, type);
          if (listItemType != null)
          {
            int key2 = model.GetKey(ref listItemType);
            if (key2 >= 0)
            {
              isList = true;
              return key2;
            }
          }
        }
      }
      isList = false;
      return -1;
    }

    /// <summary>
    /// Ends an object in the output
    /// 
    /// </summary>
    public override void WriteEndObject(XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");
      writer.WriteEndElement();
    }

    /// <summary>
    /// Begins an object in the output
    /// 
    /// </summary>
    public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");
      ((XmlWriter) writer).WriteStartElement("proto");
    }

    /// <summary>
    /// Writes the body of an object in the output
    /// 
    /// </summary>
    public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");
      if (graph == null)
      {
        ((XmlWriter) writer).WriteAttributeString("nil", "true");
      }
      else
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          if (this.isList)
          {
            this.model.Serialize((Stream) memoryStream, graph, (SerializationContext) null);
          }
          else
          {
            using (ProtoWriter dest = new ProtoWriter((Stream) memoryStream, this.model, (SerializationContext) null))
              this.model.Serialize(this.key, graph, dest);
          }
          byte[] buffer = memoryStream.GetBuffer();
          writer.WriteBase64(buffer, 0, (int) memoryStream.Length);
        }
      }
    }

    /// <summary>
    /// Indicates whether this is the start of an object we are prepared to handle
    /// 
    /// </summary>
    public override bool IsStartObject(XmlDictionaryReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException("reader");
      int num = (int) reader.MoveToContent();
      if (reader.NodeType == XmlNodeType.Element)
        return reader.Name == "proto";
      else
        return false;
    }

    /// <summary>
    /// Reads the body of an object
    /// 
    /// </summary>
    public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
    {
      if (reader == null)
        throw new ArgumentNullException("reader");
      int num = (int) reader.MoveToContent();
      bool isEmptyElement = reader.IsEmptyElement;
      bool flag = ((XmlReader) reader).GetAttribute("nil") == "true";
      ((XmlReader) reader).ReadStartElement("proto");
      if (flag)
      {
        if (!isEmptyElement)
          reader.ReadEndElement();
        return (object) null;
      }
      else if (isEmptyElement)
      {
        if (this.isList || this.isEnum)
          return this.model.Deserialize(Stream.Null, (object) null, this.type, (SerializationContext) null);
        ProtoReader protoReader = (ProtoReader) null;
        try
        {
          protoReader = ProtoReader.Create(Stream.Null, this.model, (SerializationContext) null, -1);
          return this.model.Deserialize(this.key, (object) null, protoReader);
        }
        finally
        {
          ProtoReader.Recycle(protoReader);
        }
      }
      else
      {
        object obj;
        using (MemoryStream memoryStream = new MemoryStream(reader.ReadContentAsBase64()))
        {
          if (this.isList || this.isEnum)
          {
            obj = this.model.Deserialize((Stream) memoryStream, (object) null, this.type, (SerializationContext) null);
          }
          else
          {
            ProtoReader protoReader = (ProtoReader) null;
            try
            {
              protoReader = ProtoReader.Create((Stream) memoryStream, this.model, (SerializationContext) null, -1);
              obj = this.model.Deserialize(this.key, (object) null, protoReader);
            }
            finally
            {
              ProtoReader.Recycle(protoReader);
            }
          }
        }
        reader.ReadEndElement();
        return obj;
      }
    }
  }
}
