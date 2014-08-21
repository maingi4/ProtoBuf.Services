// Type: System.ServiceModel.Channels.MessageHeaders
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.Xml;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Represents a collection of message headers for a message. This class cannot be inherited.
  /// </summary>
  [__DynamicallyInvokable]
  public sealed class MessageHeaders : IEnumerable<MessageHeaderInfo>, IEnumerable
  {
    internal const string WildcardAction = "*";
    private int collectionVersion;
    private int headerCount;
    private MessageHeaders.Header[] headers;
    private MessageVersion version;
    private IBufferedMessageData bufferedMessageData;
    private UnderstoodHeaders understoodHeaders;
    private static XmlDictionaryString[] localNames;
    private int nodeCount;
    private int attrCount;
    private bool understoodHeadersModified;
    private const int InitialHeaderCount = 4;
    private const int MaxRecycledArrayLength = 8;
    private const int MaxBufferedHeaderNodes = 4096;
    private const int MaxBufferedHeaderAttributes = 2048;

    /// <summary>
    /// Gets or sets a description of how the message should be processed.
    /// </summary>
    /// 
    /// <returns>
    /// A description of how the message should be processed.
    /// </returns>
    [__DynamicallyInvokable]
    public string Action
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.Action);
        if (headerProperty < 0)
          return (string) null;
        ActionHeader actionHeader = this.headers[headerProperty].HeaderInfo as ActionHeader;
        if (actionHeader != null)
          return actionHeader.Action;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return ActionHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != null)
          this.SetActionHeader(ActionHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.Action, (MessageHeader) null);
      }
    }

    internal bool CanRecycle
    {
      get
      {
        return this.headers.Length <= 8;
      }
    }

    internal bool ContainsOnlyBufferedMessageHeaders
    {
      get
      {
        if (this.bufferedMessageData != null)
          return this.collectionVersion == 0;
        else
          return false;
      }
    }

    internal int CollectionVersion
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.collectionVersion;
      }
    }

    /// <summary>
    /// Gets the number of message headers in this collection.
    /// </summary>
    /// 
    /// <returns>
    /// The number of message headers in this collection.
    /// </returns>
    [__DynamicallyInvokable]
    public int Count
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.headerCount;
      }
    }

    /// <summary>
    /// Gets or sets the address of the node to which faults should be sent.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.EndpointAddress"/> of the node to which faults should be sent.
    /// </returns>
    [__DynamicallyInvokable]
    public EndpointAddress FaultTo
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.FaultTo);
        if (headerProperty < 0)
          return (EndpointAddress) null;
        FaultToHeader faultToHeader = this.headers[headerProperty].HeaderInfo as FaultToHeader;
        if (faultToHeader != null)
          return faultToHeader.FaultTo;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return FaultToHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != (EndpointAddress) null)
          this.SetFaultToHeader(FaultToHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.FaultTo, (MessageHeader) null);
      }
    }

    /// <summary>
    /// Gets or sets the address of the node that sent the message.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.EndpointAddress"/> of the node that sent the message.
    /// </returns>
    [__DynamicallyInvokable]
    public EndpointAddress From
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.From);
        if (headerProperty < 0)
          return (EndpointAddress) null;
        FromHeader fromHeader = this.headers[headerProperty].HeaderInfo as FromHeader;
        if (fromHeader != null)
          return fromHeader.From;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return FromHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != (EndpointAddress) null)
          this.SetFromHeader(FromHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.From, (MessageHeader) null);
      }
    }

    internal bool HasMustUnderstandBeenModified
    {
      get
      {
        if (this.understoodHeaders != null)
          return this.understoodHeaders.Modified;
        else
          return this.understoodHeadersModified;
      }
    }

    /// <summary>
    /// Gets or sets the unique ID of the message.
    /// </summary>
    /// 
    /// <returns>
    /// The unique ID of the message.
    /// </returns>
    [__DynamicallyInvokable]
    public UniqueId MessageId
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.MessageId);
        if (headerProperty < 0)
          return (UniqueId) null;
        MessageIDHeader messageIdHeader = this.headers[headerProperty].HeaderInfo as MessageIDHeader;
        if (messageIdHeader != null)
          return messageIdHeader.MessageId;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return MessageIDHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != (UniqueId) null)
          this.SetMessageIDHeader(MessageIDHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.MessageId, (MessageHeader) null);
      }
    }

    /// <summary>
    /// Gets the SOAP version of the message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> that is SOAP version of the message.
    /// </returns>
    [__DynamicallyInvokable]
    public MessageVersion MessageVersion
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.version;
      }
    }

    /// <summary>
    /// Gets the IDs of messages that are related to this message.
    /// </summary>
    /// 
    /// <returns>
    /// The IDs of messages that are related to this message.
    /// </returns>
    [__DynamicallyInvokable]
    public UniqueId RelatesTo
    {
      [__DynamicallyInvokable] get
      {
        return this.GetRelatesTo(RelatesToHeader.ReplyRelationshipType);
      }
      [__DynamicallyInvokable] set
      {
        this.SetRelatesTo(RelatesToHeader.ReplyRelationshipType, value);
      }
    }

    /// <summary>
    /// Gets or sets the address of the node to which a reply should be sent for a request.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.EndpointAddress"/> of the node to which a reply should be sent for a request.
    /// </returns>
    [__DynamicallyInvokable]
    public EndpointAddress ReplyTo
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.ReplyTo);
        if (headerProperty < 0)
          return (EndpointAddress) null;
        ReplyToHeader replyToHeader = this.headers[headerProperty].HeaderInfo as ReplyToHeader;
        if (replyToHeader != null)
          return replyToHeader.ReplyTo;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return ReplyToHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != (EndpointAddress) null)
          this.SetReplyToHeader(ReplyToHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.ReplyTo, (MessageHeader) null);
      }
    }

    /// <summary>
    /// Gets or sets the destination endpoint of a message.
    /// </summary>
    /// 
    /// <returns>
    /// A URI that contains the destination endpoint of a message.
    /// </returns>
    [__DynamicallyInvokable]
    public Uri To
    {
      [__DynamicallyInvokable] get
      {
        int headerProperty = this.FindHeaderProperty(MessageHeaders.HeaderKind.To);
        if (headerProperty < 0)
          return (Uri) null;
        ToHeader toHeader = this.headers[headerProperty].HeaderInfo as ToHeader;
        if (toHeader != null)
          return toHeader.To;
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerProperty))
          return ToHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing);
      }
      [__DynamicallyInvokable] set
      {
        if (value != (Uri) null)
          this.SetToHeader(ToHeader.Create(value, this.version.Addressing));
        else
          this.SetHeaderProperty(MessageHeaders.HeaderKind.To, (MessageHeader) null);
      }
    }

    /// <summary>
    /// Gets all the message headers that must be understood, according to SOAP 1.1/1.2 specification.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.UnderstoodHeaders"/> that represents the message headers that must be understood.
    /// </returns>
    public UnderstoodHeaders UnderstoodHeaders
    {
      get
      {
        if (this.understoodHeaders == null)
          this.understoodHeaders = new UnderstoodHeaders(this, this.understoodHeadersModified);
        return this.understoodHeaders;
      }
    }

    /// <summary>
    /// Retrieves a header at the given index.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.MessageHeaderInfo"/> in the collection. If <paramref name="index"/> is greater than or equal to the number of headers in the list, this returns null.
    /// </returns>
    /// <param name="index">The zero-based index of the header in the list.</param>
    [__DynamicallyInvokable]
    public MessageHeaderInfo this[int index]
    {
      [__DynamicallyInvokable] get
      {
        if (index >= 0 && index < this.headerCount)
          return this.headers[index].HeaderInfo;
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("index", (object) index, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> class with the specified message version and size.
    /// </summary>
    /// <param name="version">The SOAP version of the message.</param><param name="initialSize">The size of the header.</param>
    [__DynamicallyInvokable]
    public MessageHeaders(MessageVersion version, int initialSize)
    {
      this.Init(version, initialSize);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> class with the specified message version.
    /// </summary>
    /// <param name="version">The SOAP version of the message.</param>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public MessageHeaders(MessageVersion version)
      : this(version, 4)
    {
    }

    internal MessageHeaders(MessageVersion version, XmlDictionaryReader reader, XmlAttributeHolder[] envelopeAttributes, XmlAttributeHolder[] headerAttributes, ref int maxSizeOfHeaders)
      : this(version)
    {
      if (maxSizeOfHeaders < 0)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("maxSizeOfHeaders", (object) maxSizeOfHeaders, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")));
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (reader == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("reader"));
      if (reader.IsEmptyElement)
      {
        reader.Read();
      }
      else
      {
        XmlBuffer buffer = (XmlBuffer) null;
        EnvelopeVersion envelope = version.Envelope;
        reader.ReadStartElement(XD.MessageDictionary.Header, envelope.DictionaryNamespace);
        while (((XmlReader) reader).IsStartElement())
        {
          if (buffer == null)
            buffer = new XmlBuffer(maxSizeOfHeaders);
          BufferedHeader bufferedHeader = new BufferedHeader(version, buffer, reader, envelopeAttributes, headerAttributes);
          MessageHeaders.HeaderProcessing processing = bufferedHeader.MustUnderstand ? MessageHeaders.HeaderProcessing.MustUnderstand : (MessageHeaders.HeaderProcessing) 0;
          MessageHeaders.HeaderKind headerKind = this.GetHeaderKind((MessageHeaderInfo) bufferedHeader);
          if (headerKind != MessageHeaders.HeaderKind.Unknown)
          {
            processing |= MessageHeaders.HeaderProcessing.Understood;
            MessageHeaders.TraceUnderstood((MessageHeaderInfo) bufferedHeader);
          }
          this.AddHeader(new MessageHeaders.Header(headerKind, (ReadableMessageHeader) bufferedHeader, processing));
        }
        if (buffer != null)
        {
          buffer.Close();
          maxSizeOfHeaders -= buffer.BufferSize;
        }
        reader.ReadEndElement();
        this.collectionVersion = 0;
      }
    }

    internal MessageHeaders(MessageVersion version, XmlDictionaryReader reader, IBufferedMessageData bufferedMessageData, RecycledMessageState recycledMessageState, bool[] understoodHeaders, bool understoodHeadersModified)
    {
      this.headers = new MessageHeaders.Header[4];
      this.Init(version, reader, bufferedMessageData, recycledMessageState, understoodHeaders, understoodHeadersModified);
    }

    internal MessageHeaders(MessageVersion version, MessageHeaders headers, IBufferedMessageData bufferedMessageData)
    {
      this.version = version;
      this.bufferedMessageData = bufferedMessageData;
      this.headerCount = headers.headerCount;
      this.headers = new MessageHeaders.Header[this.headerCount];
      Array.Copy((Array) headers.headers, (Array) this.headers, this.headerCount);
      this.collectionVersion = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> class with the specified collection of message headers.
    /// </summary>
    /// <param name="collection">A collection of message headers.</param>
    [__DynamicallyInvokable]
    public MessageHeaders(MessageHeaders collection)
    {
      if (collection == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("collection");
      this.Init(collection.version, collection.headers.Length);
      this.CopyHeadersFrom(collection);
      this.collectionVersion = 0;
    }

    /// <summary>
    /// Adds the specified message header to the collection.
    /// </summary>
    /// <param name="header">A message header to be added to the collection.</param>
    [__DynamicallyInvokable]
    public void Add(MessageHeader header)
    {
      this.Insert(this.headerCount, header);
    }

    internal void AddActionHeader(ActionHeader actionHeader)
    {
      this.Insert(this.headerCount, (MessageHeader) actionHeader, MessageHeaders.HeaderKind.Action);
    }

    internal void AddMessageIDHeader(MessageIDHeader messageIDHeader)
    {
      this.Insert(this.headerCount, (MessageHeader) messageIDHeader, MessageHeaders.HeaderKind.MessageId);
    }

    internal void AddRelatesToHeader(RelatesToHeader relatesToHeader)
    {
      this.Insert(this.headerCount, (MessageHeader) relatesToHeader, MessageHeaders.HeaderKind.RelatesTo);
    }

    internal void AddReplyToHeader(ReplyToHeader replyToHeader)
    {
      this.Insert(this.headerCount, (MessageHeader) replyToHeader, MessageHeaders.HeaderKind.ReplyTo);
    }

    internal void AddToHeader(ToHeader toHeader)
    {
      this.Insert(this.headerCount, (MessageHeader) toHeader, MessageHeaders.HeaderKind.To);
    }

    internal void AddUnderstood(int i)
    {
      this.headers[i].HeaderProcessing |= MessageHeaders.HeaderProcessing.Understood;
      MessageHeaders.TraceUnderstood(this.headers[i].HeaderInfo);
    }

    internal void AddUnderstood(MessageHeaderInfo headerInfo)
    {
      if (headerInfo == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("headerInfo"));
      for (int i = 0; i < this.headerCount; ++i)
      {
        if (this.headers[i].HeaderInfo == headerInfo)
        {
          if ((this.headers[i].HeaderProcessing & MessageHeaders.HeaderProcessing.Understood) != (MessageHeaders.HeaderProcessing) 0)
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentException(System.ServiceModel.SR.GetString("HeaderAlreadyUnderstood", (object) headerInfo.Name, (object) headerInfo.Namespace), "headerInfo"));
          else
            this.AddUnderstood(i);
        }
      }
    }

    /// <summary>
    /// Removes all the headers from the collection.
    /// </summary>
    [__DynamicallyInvokable]
    public void Clear()
    {
      for (int index = 0; index < this.headerCount; ++index)
        this.headers[index] = new MessageHeaders.Header();
      this.headerCount = 0;
      ++this.collectionVersion;
      this.bufferedMessageData = (IBufferedMessageData) null;
    }

    /// <summary>
    /// Copies the header content located at the specified index from the specified message to this instance.
    /// </summary>
    /// <param name="message">A message from which the header is copied from.</param><param name="headerIndex">The location of the original message header, from which the content is copied over.</param>
    [__DynamicallyInvokable]
    public void CopyHeaderFrom(Message message, int headerIndex)
    {
      if (message == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("message"));
      this.CopyHeaderFrom(message.Headers, headerIndex);
    }

    /// <summary>
    /// Copies the header content located at the specified index from the specified message header collection to this instance.
    /// </summary>
    /// <param name="collection">A <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> collection object.</param><param name="headerIndex">The location of the original message header, from which the content is copied over.</param>
    [__DynamicallyInvokable]
    public void CopyHeaderFrom(MessageHeaders collection, int headerIndex)
    {
      if (collection == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("collection");
      if (collection.version != this.version)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentException(System.ServiceModel.SR.GetString("MessageHeaderVersionMismatch", (object) collection.version.ToString(), (object) this.version.ToString()), "collection"));
      else if (headerIndex < 0 || headerIndex >= collection.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) collection.headerCount)));
      }
      else
      {
        MessageHeaders.Header header = collection.headers[headerIndex];
        MessageHeaders.HeaderProcessing processing = header.HeaderInfo.MustUnderstand ? MessageHeaders.HeaderProcessing.MustUnderstand : (MessageHeaders.HeaderProcessing) 0;
        if ((header.HeaderProcessing & MessageHeaders.HeaderProcessing.Understood) != (MessageHeaders.HeaderProcessing) 0 || header.HeaderKind != MessageHeaders.HeaderKind.Unknown)
          processing |= MessageHeaders.HeaderProcessing.Understood;
        switch (header.HeaderType)
        {
          case MessageHeaders.HeaderType.ReadableHeader:
            this.AddHeader(new MessageHeaders.Header(header.HeaderKind, header.ReadableHeader, processing));
            break;
          case MessageHeaders.HeaderType.BufferedMessageHeader:
            this.AddHeader(new MessageHeaders.Header(header.HeaderKind, (ReadableMessageHeader) this.CaptureBufferedHeader(collection.bufferedMessageData, header.HeaderInfo, headerIndex), processing));
            break;
          case MessageHeaders.HeaderType.WriteableHeader:
            this.AddHeader(new MessageHeaders.Header(header.HeaderKind, header.MessageHeader, processing));
            break;
          default:
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
            {
              (object) header.HeaderType
            })));
        }
      }
    }

    /// <summary>
    /// Copies the content of all the headers from the specified message to this instance.
    /// </summary>
    /// <param name="message">A message from which the headers are copied from.</param>
    [__DynamicallyInvokable]
    public void CopyHeadersFrom(Message message)
    {
      if (message == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("message"));
      this.CopyHeadersFrom(message.Headers);
    }

    /// <summary>
    /// Copies the content from the specified header collection to this instance.
    /// </summary>
    /// <param name="collection">A <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> collection object from which the headers are copied to this instance.</param>
    [__DynamicallyInvokable]
    public void CopyHeadersFrom(MessageHeaders collection)
    {
      if (collection == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("collection"));
      for (int headerIndex = 0; headerIndex < collection.headerCount; ++headerIndex)
        this.CopyHeaderFrom(collection, headerIndex);
    }

    /// <summary>
    /// Copies the headers from this collection to an array, starting at a particular index of the array.
    /// </summary>
    /// <param name="array">The one-dimensional Array that is the destination of the message header objects copied from this instance. The Array must have zero-based indexing. </param><param name="index">The zero-based index in the array at which copying begins. </param>
    [__DynamicallyInvokable]
    public void CopyTo(MessageHeaderInfo[] array, int index)
    {
      if (array == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("array");
      if (index < 0 || index + this.headerCount > array.Length)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("index", (object) index, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) (array.Length - this.headerCount))));
      }
      else
      {
        for (int index1 = 0; index1 < this.headerCount; ++index1)
          array[index1 + index] = this.headers[index1].HeaderInfo;
      }
    }

    /// <summary>
    /// Finds a message header in this collection by the specified LocalName and namespace URI of the header element.
    /// </summary>
    /// 
    /// <returns>
    /// The index of the message header in this collection if found or -1 if the header specified does not exist.
    /// </returns>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param><exception cref="T:System.ArgumentNullException">Arguments are null.</exception><exception cref="T:System.ServiceModel.MessageHeaderException">The header specified by the arguments exists multiple times.</exception>
    [__DynamicallyInvokable]
    public int FindHeader(string name, string ns)
    {
      if (name == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("name"));
      if (ns == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("ns"));
      if (ns == this.version.Addressing.Namespace)
        return this.FindAddressingHeader(name, ns);
      else
        return this.FindNonAddressingHeader(name, ns, this.version.Envelope.UltimateDestinationActorValues);
    }

    /// <summary>
    /// Finds a message header in this collection by the specified LocalName, namespace URI and actors of the header element.
    /// </summary>
    /// 
    /// <returns>
    /// The index of the message header in this collection if found, or -1 if the header specified does not exist.
    /// </returns>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param><param name="actors">The targeted recipient of the message header.</param><exception cref="T:System.ArgumentNullException">Arguments are null.</exception><exception cref="T:System.ServiceModel.MessageHeaderException">The header specified by the arguments exists multiple times.</exception>
    [__DynamicallyInvokable]
    public int FindHeader(string name, string ns, params string[] actors)
    {
      if (name == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("name"));
      if (ns == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("ns"));
      if (actors == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("actors"));
      int num = -1;
      for (int index1 = 0; index1 < this.headerCount; ++index1)
      {
        MessageHeaderInfo headerInfo = this.headers[index1].HeaderInfo;
        if (headerInfo.Name == name && headerInfo.Namespace == ns)
        {
          for (int index2 = 0; index2 < actors.Length; ++index2)
          {
            if (actors[index2] == headerInfo.Actor)
            {
              if (num >= 0)
              {
                if (actors.Length == 1)
                  throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeadersWithActor", (object) name, (object) ns, (object) actors[0]), name, ns, true));
                else
                  throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeaders", (object) name, (object) ns), name, ns, true));
              }
              else
                num = index1;
            }
          }
        }
      }
      return num;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator for iterating through the collection. This method cannot be inherited.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    [__DynamicallyInvokable]
    public IEnumerator<MessageHeaderInfo> GetEnumerator()
    {
      MessageHeaderInfo[] messageHeaderInfoArray = new MessageHeaderInfo[this.headerCount];
      this.CopyTo(messageHeaderInfoArray, 0);
      return this.GetEnumerator(messageHeaderInfoArray);
    }

    internal IEnumerator<MessageHeaderInfo> GetUnderstoodEnumerator()
    {
      List<MessageHeaderInfo> list = new List<MessageHeaderInfo>();
      for (int index = 0; index < this.headerCount; ++index)
      {
        if ((this.headers[index].HeaderProcessing & MessageHeaders.HeaderProcessing.Understood) != (MessageHeaders.HeaderProcessing) 0)
          list.Add(this.headers[index].HeaderInfo);
      }
      return (IEnumerator<MessageHeaderInfo>) list.GetEnumerator();
    }

    /// <summary>
    /// Finds a message header in this collection by the specified LocalName and namespace URI of the header element.
    /// </summary>
    /// 
    /// <returns>
    /// A message header with the specified name.
    /// </returns>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param><typeparam name="T">The type of the message header.</typeparam>
    [__DynamicallyInvokable]
    public T GetHeader<T>(string name, string ns)
    {
      return this.GetHeader<T>(name, ns, (XmlObjectSerializer) DataContractSerializerDefaults.CreateSerializer(typeof (T), name, ns, int.MaxValue));
    }

    /// <summary>
    /// Retrieves a message header in this collection by the specified LocalName, namespace URI and actors of the header element.
    /// </summary>
    /// 
    /// <returns>
    /// A message header with the specified name.
    /// </returns>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param><param name="actors">The targeted recipient of the message header.</param><typeparam name="T">The type of the message header.</typeparam>
    [__DynamicallyInvokable]
    public T GetHeader<T>(string name, string ns, params string[] actors)
    {
      int header = this.FindHeader(name, ns, actors);
      if (header >= 0)
        return this.GetHeader<T>(header);
      throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("HeaderNotFound", (object) name, (object) ns), name, ns));
    }

    /// <summary>
    /// Retrieves a message header in this collection by the specified LocalName, namespace URI and serializer.
    /// </summary>
    /// 
    /// <returns>
    /// A message header with the specified name.
    /// </returns>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param><param name="serializer">An <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> that is used to serialize the header.</param><typeparam name="T">The type of the message header.</typeparam>
    [__DynamicallyInvokable]
    public T GetHeader<T>(string name, string ns, XmlObjectSerializer serializer)
    {
      if (serializer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("serializer"));
      int header = this.FindHeader(name, ns);
      if (header >= 0)
        return this.GetHeader<T>(header, serializer);
      throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("HeaderNotFound", (object) name, (object) ns), name, ns));
    }

    /// <summary>
    /// Retrieves a message header at a specific position in this collection.
    /// </summary>
    /// 
    /// <returns>
    /// A message header at the specified index.
    /// </returns>
    /// <param name="index">The zero-based index of the header to get.</param><typeparam name="T">The type of the message header.</typeparam>
    [__DynamicallyInvokable]
    public T GetHeader<T>(int index)
    {
      if (index < 0 || index >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("index", (object) index, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        MessageHeaderInfo headerInfo = this.headers[index].HeaderInfo;
        return this.GetHeader<T>(index, (XmlObjectSerializer) DataContractSerializerDefaults.CreateSerializer(typeof (T), headerInfo.Name, headerInfo.Namespace, int.MaxValue));
      }
    }

    /// <summary>
    /// Retrieves a message header at a specific position in this collection.
    /// </summary>
    /// 
    /// <returns>
    /// A message header at the specified index.
    /// </returns>
    /// <param name="index">The zero-based index of the header to get.</param><param name="serializer">An <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> that is used to serialize the header.</param><typeparam name="T">The type of the message header.</typeparam>
    [__DynamicallyInvokable]
    public T GetHeader<T>(int index, XmlObjectSerializer serializer)
    {
      if (serializer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("serializer"));
      using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(index))
        return (T) serializer.ReadObject(readerAtHeader);
    }

    /// <summary>
    /// Gets a XML dictionary reader that consumes the message header at the specified location of the collection.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.Xml.XmlDictionaryReader"/> object that consumes the message header at the specified location of the collection.
    /// </returns>
    /// <param name="headerIndex">The zero-based index of the header to get.</param>
    [__DynamicallyInvokable]
    public XmlDictionaryReader GetReaderAtHeader(int headerIndex)
    {
      if (headerIndex < 0 || headerIndex >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        switch (this.headers[headerIndex].HeaderType)
        {
          case MessageHeaders.HeaderType.ReadableHeader:
            return this.headers[headerIndex].ReadableHeader.GetHeaderReader();
          case MessageHeaders.HeaderType.BufferedMessageHeader:
            return this.GetBufferedMessageHeaderReader(this.bufferedMessageData, headerIndex);
          case MessageHeaders.HeaderType.WriteableHeader:
            BufferedHeader bufferedHeader = this.CaptureWriteableHeader(this.headers[headerIndex].MessageHeader);
            this.headers[headerIndex] = new MessageHeaders.Header(this.headers[headerIndex].HeaderKind, (ReadableMessageHeader) bufferedHeader, this.headers[headerIndex].HeaderProcessing);
            ++this.collectionVersion;
            return bufferedHeader.GetHeaderReader();
          default:
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
            {
              (object) this.headers[headerIndex].HeaderType
            })));
        }
      }
    }

    internal UniqueId GetRelatesTo(Uri relationshipType)
    {
      if (relationshipType == (Uri) null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("relationshipType"));
      UniqueId messageId;
      this.FindRelatesTo(relationshipType, out messageId);
      return messageId;
    }

    internal string[] GetHeaderAttributes(string localName, string ns)
    {
      string[] strArray = (string[]) null;
      if (this.ContainsOnlyBufferedMessageHeaders)
      {
        XmlDictionaryReader messageReader = this.bufferedMessageData.GetMessageReader();
        ((XmlReader) messageReader).ReadStartElement();
        ((XmlReader) messageReader).ReadStartElement();
        int index = 0;
        while (((XmlReader) messageReader).IsStartElement())
        {
          string attribute = ((XmlReader) messageReader).GetAttribute(localName, ns);
          if (attribute != null)
          {
            if (strArray == null)
              strArray = new string[this.headerCount];
            strArray[index] = attribute;
          }
          if (index != this.headerCount - 1)
          {
            messageReader.Skip();
            ++index;
          }
          else
            break;
        }
        messageReader.Close();
      }
      else
      {
        for (int headerIndex = 0; headerIndex < this.headerCount; ++headerIndex)
        {
          if (this.headers[headerIndex].HeaderType != MessageHeaders.HeaderType.WriteableHeader)
          {
            using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(headerIndex))
            {
              string attribute = ((XmlReader) readerAtHeader).GetAttribute(localName, ns);
              if (attribute != null)
              {
                if (strArray == null)
                  strArray = new string[this.headerCount];
                strArray[headerIndex] = attribute;
              }
            }
          }
        }
      }
      return strArray;
    }

    internal MessageHeader GetMessageHeader(int index)
    {
      if (index < 0 || index >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) index, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        switch (this.headers[index].HeaderType)
        {
          case MessageHeaders.HeaderType.ReadableHeader:
          case MessageHeaders.HeaderType.WriteableHeader:
            return this.headers[index].MessageHeader;
          case MessageHeaders.HeaderType.BufferedMessageHeader:
            MessageHeader header = (MessageHeader) this.CaptureBufferedHeader(this.bufferedMessageData, this.headers[index].HeaderInfo, index);
            this.headers[index] = new MessageHeaders.Header(this.headers[index].HeaderKind, header, this.headers[index].HeaderProcessing);
            ++this.collectionVersion;
            return header;
          default:
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
            {
              (object) this.headers[index].HeaderType
            })));
        }
      }
    }

    internal Collection<MessageHeaderInfo> GetHeadersNotUnderstood()
    {
      Collection<MessageHeaderInfo> collection = (Collection<MessageHeaderInfo>) null;
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderProcessing == MessageHeaders.HeaderProcessing.MustUnderstand)
        {
          if (collection == null)
            collection = new Collection<MessageHeaderInfo>();
          MessageHeaderInfo headerInfo = this.headers[index].HeaderInfo;
          if (System.ServiceModel.DiagnosticUtility.ShouldTraceWarning)
            TraceUtility.TraceEvent(TraceEventType.Warning, 524302, System.ServiceModel.SR.GetString("TraceCodeDidNotUnderstandMessageHeader"), (TraceRecord) new MessageHeaderInfoTraceRecord(headerInfo), (object) null, (Exception) null);
          collection.Add(headerInfo);
        }
      }
      return collection;
    }

    /// <summary>
    /// Verifies whether all the message headers marked with MustUnderstand have been properly interpreted and processed.
    /// </summary>
    /// 
    /// <returns>
    /// true if the recipients specified by <paramref name="actors"/> have properly interpreted and processed all the message headers marked with MustUnderstand; otherwise, false.
    /// </returns>
    [__DynamicallyInvokable]
    public bool HaveMandatoryHeadersBeenUnderstood()
    {
      return this.HaveMandatoryHeadersBeenUnderstood(this.version.Envelope.MustUnderstandActorValues);
    }

    /// <summary>
    /// Verifies whether the specified recipients have properly interpreted and processed all the message headers marked with MustUnderstand.
    /// </summary>
    /// 
    /// <returns>
    /// true if the recipients specified by <paramref name="actors"/> have properly interpreted and processed all the message headers marked with MustUnderstand; otherwise, false.
    /// </returns>
    /// <param name="actors">The targeted recipient of the message header.</param>
    [__DynamicallyInvokable]
    public bool HaveMandatoryHeadersBeenUnderstood(params string[] actors)
    {
      if (actors == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("actors"));
      for (int index1 = 0; index1 < this.headerCount; ++index1)
      {
        if (this.headers[index1].HeaderProcessing == MessageHeaders.HeaderProcessing.MustUnderstand)
        {
          for (int index2 = 0; index2 < actors.Length; ++index2)
          {
            if (this.headers[index1].HeaderInfo.Actor == actors[index2])
              return false;
          }
        }
      }
      return true;
    }

    internal void Init(MessageVersion version, int initialSize)
    {
      this.nodeCount = 0;
      this.attrCount = 0;
      if (initialSize < 0)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("initialSize", (object) initialSize, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")));
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("version");
      this.version = version;
      this.headers = new MessageHeaders.Header[initialSize];
    }

    internal void Init(MessageVersion version)
    {
      this.nodeCount = 0;
      this.attrCount = 0;
      this.version = version;
      this.collectionVersion = 0;
    }

    internal void Init(MessageVersion version, XmlDictionaryReader reader, IBufferedMessageData bufferedMessageData, RecycledMessageState recycledMessageState, bool[] understoodHeaders, bool understoodHeadersModified)
    {
      this.nodeCount = 0;
      this.attrCount = 0;
      this.version = version;
      this.bufferedMessageData = bufferedMessageData;
      if (version.Envelope != EnvelopeVersion.None)
      {
        this.understoodHeadersModified = understoodHeaders != null && understoodHeadersModified;
        if (reader.IsEmptyElement)
        {
          reader.Read();
          return;
        }
        else
        {
          EnvelopeVersion envelope = version.Envelope;
          ((XmlReader) reader).ReadStartElement();
          AddressingDictionary addressingDictionary = XD.AddressingDictionary;
          if (MessageHeaders.localNames == null)
          {
            XmlDictionaryString[] dictionaryStringArray = new XmlDictionaryString[7];
            dictionaryStringArray[6] = addressingDictionary.To;
            dictionaryStringArray[0] = addressingDictionary.Action;
            dictionaryStringArray[3] = addressingDictionary.MessageId;
            dictionaryStringArray[5] = addressingDictionary.RelatesTo;
            dictionaryStringArray[4] = addressingDictionary.ReplyTo;
            dictionaryStringArray[2] = addressingDictionary.From;
            dictionaryStringArray[1] = addressingDictionary.FaultTo;
            Thread.MemoryBarrier();
            MessageHeaders.localNames = dictionaryStringArray;
          }
          int num = 0;
          while (((XmlReader) reader).IsStartElement())
            this.ReadBufferedHeader(reader, recycledMessageState, MessageHeaders.localNames, understoodHeaders != null && understoodHeaders[num++]);
          reader.ReadEndElement();
        }
      }
      this.collectionVersion = 0;
    }

    /// <summary>
    /// Inserts a message header into the collection at the specified index.
    /// </summary>
    /// <param name="headerIndex">The zero-based index at which <paramref name="header"/> should be inserted.</param><param name="header">A message header to insert.</param>
    [__DynamicallyInvokable]
    public void Insert(int headerIndex, MessageHeader header)
    {
      if (header == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("header"));
      if (!header.IsMessageVersionSupported(this.version))
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentException(System.ServiceModel.SR.GetString("MessageHeaderVersionNotSupported", (object) header.GetType().FullName, (object) this.version.Envelope.ToString()), "header"));
      else
        this.Insert(headerIndex, header, this.GetHeaderKind((MessageHeaderInfo) header));
    }

    internal bool IsUnderstood(int i)
    {
      return (this.headers[i].HeaderProcessing & MessageHeaders.HeaderProcessing.Understood) != (MessageHeaders.HeaderProcessing) 0;
    }

    internal bool IsUnderstood(MessageHeaderInfo headerInfo)
    {
      if (headerInfo == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("headerInfo"));
      for (int i = 0; i < this.headerCount; ++i)
      {
        if (this.headers[i].HeaderInfo == headerInfo && this.IsUnderstood(i))
          return true;
      }
      return false;
    }

    internal void Recycle(HeaderInfoCache headerInfoCache)
    {
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderKind == MessageHeaders.HeaderKind.Unknown)
          headerInfoCache.ReturnHeaderInfo(this.headers[index].HeaderInfo);
      }
      this.Clear();
      this.collectionVersion = 0;
      if (this.understoodHeaders == null)
        return;
      this.understoodHeaders.Modified = false;
    }

    internal void RemoveUnderstood(MessageHeaderInfo headerInfo)
    {
      if (headerInfo == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("headerInfo"));
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderInfo == headerInfo)
        {
          if ((this.headers[index].HeaderProcessing & MessageHeaders.HeaderProcessing.Understood) == (MessageHeaders.HeaderProcessing) 0)
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentException(System.ServiceModel.SR.GetString("HeaderAlreadyNotUnderstood", (object) headerInfo.Name, (object) headerInfo.Namespace), "headerInfo"));
          else
            this.headers[index].HeaderProcessing &= ~MessageHeaders.HeaderProcessing.Understood;
        }
      }
    }

    /// <summary>
    /// Removes all headers with the specified name and namespace from the collection.
    /// </summary>
    /// <param name="name">The LocalName of the header XML element.</param><param name="ns">The namespace URI of the header XML element.</param>
    [__DynamicallyInvokable]
    public void RemoveAll(string name, string ns)
    {
      if (name == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("name"));
      if (ns == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("ns"));
      for (int headerIndex = this.headerCount - 1; headerIndex >= 0; --headerIndex)
      {
        MessageHeaderInfo headerInfo = this.headers[headerIndex].HeaderInfo;
        if (headerInfo.Name == name && headerInfo.Namespace == ns)
          this.RemoveAt(headerIndex);
      }
    }

    /// <summary>
    /// Removes the message header at the specified index from the collection.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to remove.</param>
    [__DynamicallyInvokable]
    public void RemoveAt(int headerIndex)
    {
      if (headerIndex < 0 || headerIndex >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        if (this.bufferedMessageData != null && this.headers[headerIndex].HeaderType == MessageHeaders.HeaderType.BufferedMessageHeader)
          this.CaptureBufferedHeaders(headerIndex);
        Array.Copy((Array) this.headers, headerIndex + 1, (Array) this.headers, headerIndex, this.headerCount - headerIndex - 1);
        this.headers[--this.headerCount] = new MessageHeaders.Header();
        ++this.collectionVersion;
      }
    }

    internal void ReplaceAt(int headerIndex, MessageHeader header)
    {
      if (headerIndex < 0 || headerIndex >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        if (header == null)
          throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("header");
        this.ReplaceAt(headerIndex, header, this.GetHeaderKind((MessageHeaderInfo) header));
      }
    }

    /// <summary>
    /// Sets the action element of the header.
    /// </summary>
    /// <param name="action">A description of how the message should be processed.</param>
    [__DynamicallyInvokable]
    public void SetAction(XmlDictionaryString action)
    {
      if (action == null)
        this.SetHeaderProperty(MessageHeaders.HeaderKind.Action, (MessageHeader) null);
      else
        this.SetActionHeader(ActionHeader.Create(action, this.version.Addressing));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetActionHeader(ActionHeader actionHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.Action, (MessageHeader) actionHeader);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetFaultToHeader(FaultToHeader faultToHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.FaultTo, (MessageHeader) faultToHeader);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetFromHeader(FromHeader fromHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.From, (MessageHeader) fromHeader);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetMessageIDHeader(MessageIDHeader messageIDHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.MessageId, (MessageHeader) messageIDHeader);
    }

    internal void SetRelatesTo(Uri relationshipType, UniqueId messageId)
    {
      if (relationshipType == (Uri) null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("relationshipType");
      RelatesToHeader relatesToHeader = object.ReferenceEquals((object) messageId, (object) null) ? (RelatesToHeader) null : RelatesToHeader.Create(messageId, this.version.Addressing, relationshipType);
      this.SetRelatesTo(RelatesToHeader.ReplyRelationshipType, relatesToHeader);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetReplyToHeader(ReplyToHeader replyToHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.ReplyTo, (MessageHeader) replyToHeader);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void SetToHeader(ToHeader toHeader)
    {
      this.SetHeaderProperty(MessageHeaders.HeaderKind.To, (MessageHeader) toHeader);
    }

    private void SetHeaderProperty(MessageHeaders.HeaderKind kind, MessageHeader header)
    {
      int headerProperty = this.FindHeaderProperty(kind);
      if (headerProperty >= 0)
      {
        if (header == null)
          this.RemoveAt(headerProperty);
        else
          this.ReplaceAt(headerProperty, header, kind);
      }
      else
      {
        if (header == null)
          return;
        this.Add(header, kind);
      }
    }

    /// <summary>
    /// Serializes the header from the specified location using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlWriter"/> that is used to serialize the header.</param>
    [__DynamicallyInvokable]
    public void WriteHeader(int headerIndex, XmlWriter writer)
    {
      this.WriteHeader(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Serializes the header from the specified location using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to serialize the header.</param>
    [__DynamicallyInvokable]
    public void WriteHeader(int headerIndex, XmlDictionaryWriter writer)
    {
      this.WriteStartHeader(headerIndex, writer);
      this.WriteHeaderContents(headerIndex, writer);
      writer.WriteEndElement();
    }

    /// <summary>
    /// Serializes the start header using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlWriter"/> that is used to serialize the start header.</param>
    [__DynamicallyInvokable]
    public void WriteStartHeader(int headerIndex, XmlWriter writer)
    {
      this.WriteStartHeader(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Serializes the start header using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to serialize the start header.</param>
    [__DynamicallyInvokable]
    public void WriteStartHeader(int headerIndex, XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
      if (headerIndex < 0 || headerIndex >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        switch (this.headers[headerIndex].HeaderType)
        {
          case MessageHeaders.HeaderType.ReadableHeader:
          case MessageHeaders.HeaderType.WriteableHeader:
            this.headers[headerIndex].MessageHeader.WriteStartHeader(writer, this.version);
            break;
          case MessageHeaders.HeaderType.BufferedMessageHeader:
            this.WriteStartBufferedMessageHeader(this.bufferedMessageData, headerIndex, (XmlWriter) writer);
            break;
          default:
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
            {
              (object) this.headers[headerIndex].HeaderType
            })));
        }
      }
    }

    /// <summary>
    /// Serializes the specified header content using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlWriter"/> that is used to serialize the header contents.</param>
    [__DynamicallyInvokable]
    public void WriteHeaderContents(int headerIndex, XmlWriter writer)
    {
      this.WriteHeaderContents(headerIndex, XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Serializes the specified header content using the specified XML writer.
    /// </summary>
    /// <param name="headerIndex">The zero-based index of the header to be serialized.</param><param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to serialize the header contents.</param>
    [__DynamicallyInvokable]
    public void WriteHeaderContents(int headerIndex, XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
      if (headerIndex < 0 || headerIndex >= this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        switch (this.headers[headerIndex].HeaderType)
        {
          case MessageHeaders.HeaderType.ReadableHeader:
          case MessageHeaders.HeaderType.WriteableHeader:
            this.headers[headerIndex].MessageHeader.WriteHeaderContents(writer, this.version);
            break;
          case MessageHeaders.HeaderType.BufferedMessageHeader:
            this.WriteBufferedMessageHeaderContents(this.bufferedMessageData, headerIndex, (XmlWriter) writer);
            break;
          default:
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
            {
              (object) this.headers[headerIndex].HeaderType
            })));
        }
      }
    }

    private void Add(MessageHeader header, MessageHeaders.HeaderKind kind)
    {
      this.Insert(this.headerCount, header, kind);
    }

    private void AddHeader(MessageHeaders.Header header)
    {
      this.InsertHeader(this.headerCount, header);
    }

    private void CaptureBufferedHeaders()
    {
      this.CaptureBufferedHeaders(-1);
    }

    private void CaptureBufferedHeaders(int exceptIndex)
    {
      using (XmlDictionaryReader atHeaderContents = MessageHeaders.GetBufferedMessageHeaderReaderAtHeaderContents(this.bufferedMessageData))
      {
        for (int index = 0; index < this.headerCount; ++index)
        {
          if (atHeaderContents.NodeType != XmlNodeType.Element)
          {
            if (atHeaderContents.MoveToContent() != XmlNodeType.Element)
              break;
          }
          MessageHeaders.Header header = this.headers[index];
          if (index == exceptIndex || header.HeaderType != MessageHeaders.HeaderType.BufferedMessageHeader)
            atHeaderContents.Skip();
          else
            this.headers[index] = new MessageHeaders.Header(header.HeaderKind, (ReadableMessageHeader) this.CaptureBufferedHeader(atHeaderContents, header.HeaderInfo), header.HeaderProcessing);
        }
      }
      this.bufferedMessageData = (IBufferedMessageData) null;
    }

    private BufferedHeader CaptureBufferedHeader(XmlDictionaryReader reader, MessageHeaderInfo headerInfo)
    {
      XmlBuffer buffer = new XmlBuffer(int.MaxValue);
      buffer.OpenSection(this.bufferedMessageData.Quotas).WriteNode(reader, false);
      buffer.CloseSection();
      buffer.Close();
      return new BufferedHeader(this.version, buffer, 0, headerInfo);
    }

    private BufferedHeader CaptureBufferedHeader(IBufferedMessageData bufferedMessageData, MessageHeaderInfo headerInfo, int bufferedMessageHeaderIndex)
    {
      XmlBuffer buffer = new XmlBuffer(int.MaxValue);
      XmlDictionaryWriter dictionaryWriter = buffer.OpenSection(bufferedMessageData.Quotas);
      this.WriteBufferedMessageHeader(bufferedMessageData, bufferedMessageHeaderIndex, (XmlWriter) dictionaryWriter);
      buffer.CloseSection();
      buffer.Close();
      return new BufferedHeader(this.version, buffer, 0, headerInfo);
    }

    private BufferedHeader CaptureWriteableHeader(MessageHeader writeableHeader)
    {
      XmlBuffer buffer = new XmlBuffer(int.MaxValue);
      XmlDictionaryWriter writer = buffer.OpenSection(XmlDictionaryReaderQuotas.Max);
      writeableHeader.WriteHeader(writer, this.version);
      buffer.CloseSection();
      buffer.Close();
      return new BufferedHeader(this.version, buffer, 0, (MessageHeaderInfo) writeableHeader);
    }

    private Exception CreateDuplicateHeaderException(MessageHeaders.HeaderKind kind)
    {
      string headerName;
      switch (kind)
      {
        case MessageHeaders.HeaderKind.Action:
          headerName = "Action";
          break;
        case MessageHeaders.HeaderKind.FaultTo:
          headerName = "FaultTo";
          break;
        case MessageHeaders.HeaderKind.From:
          headerName = "From";
          break;
        case MessageHeaders.HeaderKind.MessageId:
          headerName = "MessageID";
          break;
        case MessageHeaders.HeaderKind.ReplyTo:
          headerName = "ReplyTo";
          break;
        case MessageHeaders.HeaderKind.To:
          headerName = "To";
          break;
        default:
          throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidEnumValue", new object[1]
          {
            (object) kind
          })));
      }
      return (Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeaders", (object) headerName, (object) this.version.Addressing.Namespace), headerName, this.version.Addressing.Namespace, true);
    }

    private int FindAddressingHeader(string name, string ns)
    {
      int num = -1;
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderKind != MessageHeaders.HeaderKind.Unknown)
        {
          MessageHeaderInfo headerInfo = this.headers[index].HeaderInfo;
          if (headerInfo.Name == name && headerInfo.Namespace == ns)
          {
            if (num >= 0)
              throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeaders", (object) name, (object) ns), name, ns, true));
            else
              num = index;
          }
        }
      }
      return num;
    }

    private int FindNonAddressingHeader(string name, string ns, string[] actors)
    {
      int num = -1;
      for (int index1 = 0; index1 < this.headerCount; ++index1)
      {
        if (this.headers[index1].HeaderKind == MessageHeaders.HeaderKind.Unknown)
        {
          MessageHeaderInfo headerInfo = this.headers[index1].HeaderInfo;
          if (headerInfo.Name == name && headerInfo.Namespace == ns)
          {
            for (int index2 = 0; index2 < actors.Length; ++index2)
            {
              if (actors[index2] == headerInfo.Actor)
              {
                if (num >= 0)
                {
                  if (actors.Length == 1)
                    throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeadersWithActor", (object) name, (object) ns, (object) actors[0]), name, ns, true));
                  else
                    throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleMessageHeaders", (object) name, (object) ns), name, ns, true));
                }
                else
                  num = index1;
              }
            }
          }
        }
      }
      return num;
    }

    private int FindHeaderProperty(MessageHeaders.HeaderKind kind)
    {
      int num = -1;
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderKind == kind)
        {
          if (num >= 0)
            throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError(this.CreateDuplicateHeaderException(kind));
          num = index;
        }
      }
      return num;
    }

    private int FindRelatesTo(Uri relationshipType, out UniqueId messageId)
    {
      UniqueId uniqueId = (UniqueId) null;
      int num = -1;
      for (int index = 0; index < this.headerCount; ++index)
      {
        if (this.headers[index].HeaderKind == MessageHeaders.HeaderKind.RelatesTo)
        {
          Uri relationshipType1;
          UniqueId messageId1;
          this.GetRelatesToValues(index, out relationshipType1, out messageId1);
          if (relationshipType == relationshipType1)
          {
            if (uniqueId != (UniqueId) null)
            {
              throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new MessageHeaderException(System.ServiceModel.SR.GetString("MultipleRelatesToHeaders", new object[1]
              {
                (object) relationshipType.AbsoluteUri
              }), "RelatesTo", this.version.Addressing.Namespace, true));
            }
            else
            {
              uniqueId = messageId1;
              num = index;
            }
          }
        }
      }
      messageId = uniqueId;
      return num;
    }

    private IEnumerator<MessageHeaderInfo> GetEnumerator(MessageHeaderInfo[] headers)
    {
      return Array.AsReadOnly<MessageHeaderInfo>(headers).GetEnumerator();
    }

    private static XmlDictionaryReader GetBufferedMessageHeaderReaderAtHeaderContents(IBufferedMessageData bufferedMessageData)
    {
      XmlDictionaryReader messageReader = bufferedMessageData.GetMessageReader();
      if (messageReader.NodeType == XmlNodeType.Element)
        messageReader.Read();
      else
        ((XmlReader) messageReader).ReadStartElement();
      if (messageReader.NodeType == XmlNodeType.Element)
        messageReader.Read();
      else
        ((XmlReader) messageReader).ReadStartElement();
      return messageReader;
    }

    private XmlDictionaryReader GetBufferedMessageHeaderReader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex)
    {
      if (this.nodeCount > 4096 || this.attrCount > 2048)
      {
        this.CaptureBufferedHeaders();
        return this.headers[bufferedMessageHeaderIndex].ReadableHeader.GetHeaderReader();
      }
      else
      {
        XmlDictionaryReader atHeaderContents = MessageHeaders.GetBufferedMessageHeaderReaderAtHeaderContents(bufferedMessageData);
        while (true)
        {
          if (atHeaderContents.NodeType != XmlNodeType.Element)
          {
            int num = (int) atHeaderContents.MoveToContent();
          }
          if (bufferedMessageHeaderIndex != 0)
          {
            this.Skip(atHeaderContents);
            --bufferedMessageHeaderIndex;
          }
          else
            break;
        }
        return atHeaderContents;
      }
    }

    private void Skip(XmlDictionaryReader reader)
    {
      if (reader.MoveToContent() == XmlNodeType.Element && !reader.IsEmptyElement)
      {
        int depth = reader.Depth;
        do
        {
          this.attrCount += reader.AttributeCount;
          ++this.nodeCount;
        }
        while (reader.Read() && depth < reader.Depth);
        if (reader.NodeType != XmlNodeType.EndElement)
          return;
        ++this.nodeCount;
        reader.Read();
      }
      else
      {
        this.attrCount += reader.AttributeCount;
        ++this.nodeCount;
        reader.Read();
      }
    }

    private MessageHeaders.HeaderKind GetHeaderKind(MessageHeaderInfo headerInfo)
    {
      MessageHeaders.HeaderKind headerKind = MessageHeaders.HeaderKind.Unknown;
      if (headerInfo.Namespace == this.version.Addressing.Namespace && this.version.Envelope.IsUltimateDestinationActor(headerInfo.Actor))
      {
        string name = headerInfo.Name;
        if (name.Length > 0)
        {
          switch (name[0])
          {
            case 'M':
              if (name == "MessageID")
              {
                headerKind = MessageHeaders.HeaderKind.MessageId;
                break;
              }
              else
                break;
            case 'R':
              if (name == "ReplyTo")
              {
                headerKind = MessageHeaders.HeaderKind.ReplyTo;
                break;
              }
              else if (name == "RelatesTo")
              {
                headerKind = MessageHeaders.HeaderKind.RelatesTo;
                break;
              }
              else
                break;
            case 'T':
              if (name == "To")
              {
                headerKind = MessageHeaders.HeaderKind.To;
                break;
              }
              else
                break;
            case 'A':
              if (name == "Action")
              {
                headerKind = MessageHeaders.HeaderKind.Action;
                break;
              }
              else
                break;
            case 'F':
              if (name == "From")
              {
                headerKind = MessageHeaders.HeaderKind.From;
                break;
              }
              else if (name == "FaultTo")
              {
                headerKind = MessageHeaders.HeaderKind.FaultTo;
                break;
              }
              else
                break;
          }
        }
      }
      this.ValidateHeaderKind(headerKind);
      return headerKind;
    }

    private void ValidateHeaderKind(MessageHeaders.HeaderKind headerKind)
    {
      if (this.version.Envelope == EnvelopeVersion.None && headerKind != MessageHeaders.HeaderKind.Action && headerKind != MessageHeaders.HeaderKind.To)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("HeadersCannotBeAddedToEnvelopeVersion", new object[1]
        {
          (object) this.version.Envelope
        })));
      }
      else
      {
        if (this.version.Addressing != AddressingVersion.None || headerKind == MessageHeaders.HeaderKind.Unknown || (headerKind == MessageHeaders.HeaderKind.Action || headerKind == MessageHeaders.HeaderKind.To))
          return;
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("AddressingHeadersCannotBeAddedToAddressingVersion", new object[1]
        {
          (object) this.version.Addressing
        })));
      }
    }

    private void GetRelatesToValues(int index, out Uri relationshipType, out UniqueId messageId)
    {
      RelatesToHeader relatesToHeader = this.headers[index].HeaderInfo as RelatesToHeader;
      if (relatesToHeader != null)
      {
        relationshipType = relatesToHeader.RelationshipType;
        messageId = relatesToHeader.UniqueId;
      }
      else
      {
        using (XmlDictionaryReader readerAtHeader = this.GetReaderAtHeader(index))
          RelatesToHeader.ReadHeaderValue(readerAtHeader, this.version.Addressing, out relationshipType, out messageId);
      }
    }

    private void Insert(int headerIndex, MessageHeader header, MessageHeaders.HeaderKind kind)
    {
      ReadableMessageHeader readableHeader = header as ReadableMessageHeader;
      MessageHeaders.HeaderProcessing processing = header.MustUnderstand ? MessageHeaders.HeaderProcessing.MustUnderstand : (MessageHeaders.HeaderProcessing) 0;
      if (kind != MessageHeaders.HeaderKind.Unknown)
        processing |= MessageHeaders.HeaderProcessing.Understood;
      if (readableHeader != null)
        this.InsertHeader(headerIndex, new MessageHeaders.Header(kind, readableHeader, processing));
      else
        this.InsertHeader(headerIndex, new MessageHeaders.Header(kind, header, processing));
    }

    private void InsertHeader(int headerIndex, MessageHeaders.Header header)
    {
      this.ValidateHeaderKind(header.HeaderKind);
      if (headerIndex < 0 || headerIndex > this.headerCount)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("headerIndex", (object) headerIndex, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 0, (object) this.headerCount)));
      }
      else
      {
        if (this.headerCount == this.headers.Length)
        {
          if (this.headers.Length == 0)
          {
            this.headers = new MessageHeaders.Header[1];
          }
          else
          {
            MessageHeaders.Header[] headerArray = new MessageHeaders.Header[this.headers.Length * 2];
            this.headers.CopyTo((Array) headerArray, 0);
            this.headers = headerArray;
          }
        }
        if (headerIndex < this.headerCount)
        {
          if (this.bufferedMessageData != null)
          {
            for (int index = headerIndex; index < this.headerCount; ++index)
            {
              if (this.headers[index].HeaderType == MessageHeaders.HeaderType.BufferedMessageHeader)
              {
                this.CaptureBufferedHeaders();
                break;
              }
            }
          }
          Array.Copy((Array) this.headers, headerIndex, (Array) this.headers, headerIndex + 1, this.headerCount - headerIndex);
        }
        this.headers[headerIndex] = header;
        ++this.headerCount;
        ++this.collectionVersion;
      }
    }

    private void ReadBufferedHeader(XmlDictionaryReader reader, RecycledMessageState recycledMessageState, XmlDictionaryString[] localNames, bool understood)
    {
      if (this.version.Addressing == AddressingVersion.None && reader.NamespaceURI == AddressingVersion.None.Namespace)
      {
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("AddressingHeadersCannotBeAddedToAddressingVersion", new object[1]
        {
          (object) this.version.Addressing
        })));
      }
      else
      {
        string actor;
        bool mustUnderstand;
        bool relay;
        bool isReferenceParameter;
        MessageHeader.GetHeaderAttributes(reader, this.version, out actor, out mustUnderstand, out relay, out isReferenceParameter);
        MessageHeaders.HeaderKind kind = MessageHeaders.HeaderKind.Unknown;
        MessageHeaderInfo info = (MessageHeaderInfo) null;
        if (this.version.Envelope.IsUltimateDestinationActor(actor))
        {
          kind = (MessageHeaders.HeaderKind) reader.IndexOfLocalName(localNames, this.version.Addressing.DictionaryNamespace);
          switch (kind)
          {
            case MessageHeaders.HeaderKind.Action:
              info = (MessageHeaderInfo) ActionHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.FaultTo:
              info = (MessageHeaderInfo) FaultToHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.From:
              info = (MessageHeaderInfo) FromHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.MessageId:
              info = (MessageHeaderInfo) MessageIDHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.ReplyTo:
              info = (MessageHeaderInfo) ReplyToHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.RelatesTo:
              info = (MessageHeaderInfo) RelatesToHeader.ReadHeader(reader, this.version.Addressing, actor, mustUnderstand, relay);
              break;
            case MessageHeaders.HeaderKind.To:
              info = (MessageHeaderInfo) ToHeader.ReadHeader(reader, this.version.Addressing, recycledMessageState.UriCache, actor, mustUnderstand, relay);
              break;
            default:
              kind = MessageHeaders.HeaderKind.Unknown;
              break;
          }
        }
        if (info == null)
        {
          info = recycledMessageState.HeaderInfoCache.TakeHeaderInfo(reader, actor, mustUnderstand, relay, isReferenceParameter);
          reader.Skip();
        }
        MessageHeaders.HeaderProcessing processing = mustUnderstand ? MessageHeaders.HeaderProcessing.MustUnderstand : (MessageHeaders.HeaderProcessing) 0;
        if (kind != MessageHeaders.HeaderKind.Unknown || understood)
        {
          processing |= MessageHeaders.HeaderProcessing.Understood;
          MessageHeaders.TraceUnderstood(info);
        }
        this.AddHeader(new MessageHeaders.Header(kind, info, processing));
      }
    }

    private void ReplaceAt(int headerIndex, MessageHeader header, MessageHeaders.HeaderKind kind)
    {
      MessageHeaders.HeaderProcessing processing = header.MustUnderstand ? MessageHeaders.HeaderProcessing.MustUnderstand : (MessageHeaders.HeaderProcessing) 0;
      if (kind != MessageHeaders.HeaderKind.Unknown)
        processing |= MessageHeaders.HeaderProcessing.Understood;
      ReadableMessageHeader readableHeader = header as ReadableMessageHeader;
      this.headers[headerIndex] = readableHeader == null ? new MessageHeaders.Header(kind, header, processing) : new MessageHeaders.Header(kind, readableHeader, processing);
      ++this.collectionVersion;
    }

    private void SetRelatesTo(Uri relationshipType, RelatesToHeader relatesToHeader)
    {
      UniqueId messageId;
      int relatesTo = this.FindRelatesTo(relationshipType, out messageId);
      if (relatesTo >= 0)
      {
        if (relatesToHeader == null)
          this.RemoveAt(relatesTo);
        else
          this.ReplaceAt(relatesTo, (MessageHeader) relatesToHeader, MessageHeaders.HeaderKind.RelatesTo);
      }
      else
      {
        if (relatesToHeader == null)
          return;
        this.Add((MessageHeader) relatesToHeader, MessageHeaders.HeaderKind.RelatesTo);
      }
    }

    private static void TraceUnderstood(MessageHeaderInfo info)
    {
      if (!System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
        return;
      TraceUtility.TraceEvent(TraceEventType.Verbose, 524303, System.ServiceModel.SR.GetString("TraceCodeUnderstoodMessageHeader"), (TraceRecord) new MessageHeaderInfoTraceRecord(info), (object) null, (Exception) null);
    }

    private void WriteBufferedMessageHeader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
    {
      using (XmlReader reader = (XmlReader) this.GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
        writer.WriteNode(reader, false);
    }

    private void WriteStartBufferedMessageHeader(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
    {
      using (XmlReader reader = (XmlReader) this.GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
      {
        writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
        writer.WriteAttributes(reader, false);
      }
    }

    private void WriteBufferedMessageHeaderContents(IBufferedMessageData bufferedMessageData, int bufferedMessageHeaderIndex, XmlWriter writer)
    {
      using (XmlReader reader = (XmlReader) this.GetBufferedMessageHeaderReader(bufferedMessageData, bufferedMessageHeaderIndex))
      {
        if (reader.IsEmptyElement)
          return;
        reader.ReadStartElement();
        while (reader.NodeType != XmlNodeType.EndElement)
          writer.WriteNode(reader, false);
        reader.ReadEndElement();
      }
    }

    private enum HeaderType : byte
    {
      Invalid,
      ReadableHeader,
      BufferedMessageHeader,
      WriteableHeader,
    }

    private enum HeaderKind : byte
    {
      Action,
      FaultTo,
      From,
      MessageId,
      ReplyTo,
      RelatesTo,
      To,
      Unknown,
    }

    [System.Flags]
    private enum HeaderProcessing : byte
    {
      MustUnderstand = (byte) 1,
      Understood = (byte) 2,
    }

    private struct Header
    {
      private MessageHeaders.HeaderType type;
      private MessageHeaders.HeaderKind kind;
      private MessageHeaders.HeaderProcessing processing;
      private MessageHeaderInfo info;

      public MessageHeaders.HeaderType HeaderType
      {
        get
        {
          return this.type;
        }
      }

      public MessageHeaders.HeaderKind HeaderKind
      {
        get
        {
          return this.kind;
        }
      }

      public MessageHeaderInfo HeaderInfo
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.info;
        }
      }

      public MessageHeader MessageHeader
      {
        get
        {
          return (MessageHeader) this.info;
        }
      }

      public MessageHeaders.HeaderProcessing HeaderProcessing
      {
        get
        {
          return this.processing;
        }
        set
        {
          this.processing = value;
        }
      }

      public ReadableMessageHeader ReadableHeader
      {
        get
        {
          return (ReadableMessageHeader) this.info;
        }
      }

      public Header(MessageHeaders.HeaderKind kind, MessageHeaderInfo info, MessageHeaders.HeaderProcessing processing)
      {
        this.kind = kind;
        this.type = MessageHeaders.HeaderType.BufferedMessageHeader;
        this.info = info;
        this.processing = processing;
      }

      public Header(MessageHeaders.HeaderKind kind, ReadableMessageHeader readableHeader, MessageHeaders.HeaderProcessing processing)
      {
        this.kind = kind;
        this.type = MessageHeaders.HeaderType.ReadableHeader;
        this.info = (MessageHeaderInfo) readableHeader;
        this.processing = processing;
      }

      public Header(MessageHeaders.HeaderKind kind, MessageHeader header, MessageHeaders.HeaderProcessing processing)
      {
        this.kind = kind;
        this.type = MessageHeaders.HeaderType.WriteableHeader;
        this.info = (MessageHeaderInfo) header;
        this.processing = processing;
      }
    }
  }
}
