// Type: System.ServiceModel.Channels.Message
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Represents the unit of communication between endpoints in a distributed environment.
  /// </summary>
  [__DynamicallyInvokable]
  public abstract class Message : IDisposable
  {
    internal const int InitialBufferSize = 1024;
    private MessageState state;
    private SeekableMessageNavigator messageNavigator;

    /// <summary>
    /// When overridden in a derived class, gets the headers of the message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageHeaders"/> object that represents the headers of the message.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
    [__DynamicallyInvokable]
    public abstract MessageHeaders Headers { [__DynamicallyInvokable] get; }

    /// <summary>
    /// Returns a value that indicates whether the <see cref="T:System.ServiceModel.Channels.Message"/> is disposed.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message is disposed; otherwise, false.
    /// </returns>
    [__DynamicallyInvokable]
    protected bool IsDisposed
    {
      [__DynamicallyInvokable] get
      {
        return this.state == MessageState.Closed;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether this message generates any SOAP faults.
    /// </summary>
    /// 
    /// <returns>
    /// true if this message generates any SOAP faults; otherwise, false.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
    [__DynamicallyInvokable]
    public virtual bool IsFault
    {
      [__DynamicallyInvokable] get
      {
        if (this.IsDisposed)
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        else
          return false;
      }
    }

    /// <summary>
    /// Returns a value that indicates whether the <see cref="T:System.ServiceModel.Channels.Message"/> is empty.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message is empty; otherwise, false.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
    [__DynamicallyInvokable]
    public virtual bool IsEmpty
    {
      [__DynamicallyInvokable] get
      {
        if (this.IsDisposed)
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        else
          return false;
      }
    }

    /// <summary>
    /// When overridden in a derived class, gets a set of processing-level annotations to the message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageProperties"/> that contains a set of processing-level annotations to the message.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
    [__DynamicallyInvokable]
    public abstract MessageProperties Properties { [__DynamicallyInvokable] get; }

    /// <summary>
    /// When overridden in a derived class, gets the SOAP version of the message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that represents the SOAP version.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message has been disposed of.</exception>
    [__DynamicallyInvokable]
    public abstract MessageVersion Version { [__DynamicallyInvokable] get; }

    internal virtual RecycledMessageState RecycledMessageState
    {
      get
      {
        return (RecycledMessageState) null;
      }
    }

    /// <summary>
    /// Gets the current state of this <see cref="T:System.ServiceModel.Channels.Message"/>.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageState"/> that contains the current state of this <see cref="T:System.ServiceModel.Channels.Message"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public MessageState State
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.state;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.Message"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected Message()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void BodyToString(XmlDictionaryWriter writer)
    {
      this.OnBodyToString(writer);
    }

    /// <summary>
    /// Closes the <see cref="T:System.ServiceModel.Channels.Message"/> and releases any resources.
    /// </summary>
    [__DynamicallyInvokable]
    public void Close()
    {
      if (this.state != MessageState.Closed)
      {
        this.state = MessageState.Closed;
        this.OnClose();
        if (!System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
          return;
        TraceUtility.TraceEvent(TraceEventType.Verbose, 524304, System.ServiceModel.SR.GetString("TraceCodeMessageClosed"), this);
      }
      else
      {
        if (!System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
          return;
        TraceUtility.TraceEvent(TraceEventType.Verbose, 524305, System.ServiceModel.SR.GetString("TraceCodeMessageClosedAgain"), this);
      }
    }

    /// <summary>
    /// Stores an entire <see cref="T:System.ServiceModel.Channels.Message"/> into a memory buffer for future access.
    /// </summary>
    /// 
    /// <returns>
    /// A newly created <see cref="T:System.ServiceModel.Channels.MessageBuffer"/> object.
    /// </returns>
    /// <param name="maxBufferSize">The maximum size of the buffer to be created.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxBufferSize "/> is smaller than zero.</exception><exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public MessageBuffer CreateBufferedCopy(int maxBufferSize)
    {
      if (maxBufferSize < 0)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("maxBufferSize", (object) maxBufferSize, System.ServiceModel.SR.GetString("ValueMustBeNonNegative")), this);
      switch (this.state)
      {
        case MessageState.Created:
          this.state = MessageState.Copied;
          if (System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
            TraceUtility.TraceEvent(TraceEventType.Verbose, 524306, System.ServiceModel.SR.GetString("TraceCodeMessageCopied"), (object) this, this);
          return this.OnCreateBufferedCopy(maxBufferSize);
        case MessageState.Read:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenRead")), this);
        case MessageState.Written:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenWritten")), this);
        case MessageState.Copied:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenCopied")), this);
        case MessageState.Closed:
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        default:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidMessageState")), this);
      }
    }

    /// <summary>
    /// Creates a message with the specified version, action and body.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><param name="action">A description of how the message should be processed. </param><param name="body">The body of the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="action"/> or <paramref name="body"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action, object body)
    {
      return Message.CreateMessage(version, action, body, (XmlObjectSerializer) DataContractSerializerDefaults.CreateSerializer(Message.GetObjectType(body), int.MaxValue));
    }

    /// <summary>
    /// Creates a message using the specified version, action, message body and serializer.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><param name="action">A description of how the message should be processed. </param><param name="body">The body of the message. </param><param name="serializer">A <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> object used to serialize the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="fault"/> or <paramref name="action"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action, object body, XmlObjectSerializer serializer)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (serializer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("serializer"));
      else
        return (Message) new BodyWriterMessage(version, action, (BodyWriter) new XmlObjectSerializerBodyWriter(body, serializer));
    }

    /// <summary>
    /// Creates a message using the specified reader, action and version.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><param name="action">A description of how the message should be processed. </param><param name="body">The <see cref="T:System.Xml.XmlReader"/> object to be used for reading the SOAP message.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="action"/> or <paramref name="body"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action, XmlReader body)
    {
      return Message.CreateMessage(version, action, XmlDictionaryReader.CreateDictionaryReader(body));
    }

    /// <summary>
    /// Creates a message with the specified version, action and body.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><param name="action">A description of how the message should be processed. </param><param name="body">The body of the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="action"/> or <paramref name="body"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action, XmlDictionaryReader body)
    {
      if (body == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("body");
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("version");
      else
        return Message.CreateMessage(version, action, (BodyWriter) new XmlReaderBodyWriter(body, version.Envelope));
    }

    /// <summary>
    /// Creates a message with a body that consists of an array of bytes.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><param name="action">A description of how the message should be processed. </param><param name="body">A <see cref="T:System.ServiceModel.Channels.BodyWriter"/> of type byte. </param><exception cref="T:System.ArgumentNullException"><paramref name="Version"/>, <paramref name="action"/> or <paramref name="body"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action, BodyWriter body)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (body == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("body"));
      else
        return (Message) new BodyWriterMessage(version, action, body);
    }

    internal static Message CreateMessage(MessageVersion version, ActionHeader actionHeader, BodyWriter body)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (body == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("body"));
      else
        return (Message) new BodyWriterMessage(version, actionHeader, body);
    }

    /// <summary>
    /// Creates a message that contains a version and an action.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message.</param><param name="action">A description of how the message should be processed.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/> or <paramref name="action"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(MessageVersion version, string action)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      else
        return (Message) new BodyWriterMessage(version, action, (BodyWriter) EmptyBodyWriter.Value);
    }

    internal static Message CreateMessage(MessageVersion version, ActionHeader actionHeader)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      else
        return (Message) new BodyWriterMessage(version, actionHeader, (BodyWriter) EmptyBodyWriter.Value);
    }

    /// <summary>
    /// Creates a message using the specified reader, action and version.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="envelopeReader">The <see cref="T:System.Xml.XmlReader"/> object to be used for reading the SOAP message.</param><param name="maxSizeOfHeaders">The maximum size in bytes of a header. </param><param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message. </param><exception cref="T:System.ArgumentNullException"><paramref name="envelopeReader"/> or <paramref name="version"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(XmlReader envelopeReader, int maxSizeOfHeaders, MessageVersion version)
    {
      return Message.CreateMessage(XmlDictionaryReader.CreateDictionaryReader(envelopeReader), maxSizeOfHeaders, version);
    }

    /// <summary>
    /// Creates a message using the specified reader, action and version.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="envelopeReader">The <see cref="T:System.Xml.XmlDictionaryReader"/> object to be used for reading the SOAP message.</param><param name="maxSizeOfHeaders">The maximum size in bytes of a header. </param><param name="version">A valid <see cref="T:System.ServiceModel.Channels.MessageVersion"/> value that specifies the SOAP version to use for the message. </param><exception cref="T:System.ArgumentNullException"><paramref name="envelopeReader"/> or <paramref name="version"/> is null. </exception>
    [__DynamicallyInvokable]
    public static Message CreateMessage(XmlDictionaryReader envelopeReader, int maxSizeOfHeaders, MessageVersion version)
    {
      if (envelopeReader == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("envelopeReader"));
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      else
        return (Message) new StreamedMessage(envelopeReader, maxSizeOfHeaders, version);
    }

    /// <summary>
    /// Creates a message that contains a SOAP fault, the reason for the fault, a version and an action.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message.</param><param name="faultCode">A <see cref="T:System.ServiceModel.Channels.MessageFault"/> object that represents a SOAP fault. </param><param name="reason">The reason of the SOAP fault. </param><param name="action">A description of how the message should be processed.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="fault"/>, <paramref name="action"/> or <paramref name="faultCode"/> is null. </exception>
    public static Message CreateMessage(MessageVersion version, FaultCode faultCode, string reason, string action)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (faultCode == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("faultCode"));
      if (reason == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("reason"));
      else
        return Message.CreateMessage(version, MessageFault.CreateFault(faultCode, reason), action);
    }

    /// <summary>
    /// Creates a message that contains a SOAP fault, a reason and the detail for the fault, a version and an action.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message.</param><param name="faultCode">A <see cref="T:System.ServiceModel.Channels.MessageFault"/> object that represents a SOAP fault. </param><param name="reason">The reason of the SOAP fault. </param><param name="detail">The details of the SOAP fault.</param><param name="action">A description of how the message should be processed.</param><exception cref="T:System.ArgumentNullException"><paramref name="version"/>, <paramref name="fault"/>, <paramref name="action"/>, <paramref name="detail"/> or <paramref name="faultCode"/> is null. </exception>
    public static Message CreateMessage(MessageVersion version, FaultCode faultCode, string reason, object detail, string action)
    {
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      if (faultCode == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("faultCode"));
      if (reason == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("reason"));
      else
        return Message.CreateMessage(version, MessageFault.CreateFault(faultCode, new FaultReason(reason), detail), action);
    }

    /// <summary>
    /// Creates a message that contains a SOAP fault, a version and an action.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.Message"/> object for the message created.
    /// </returns>
    /// <param name="version">A <see cref="T:System.ServiceModel.Channels.MessageVersion"/> object that specifies the SOAP version to use for the message.</param><param name="fault">A <see cref="T:System.ServiceModel.Channels.MessageFault"/> object that represents a SOAP fault. </param><param name="action">A description of how the message should be processed. </param><exception cref="T:System.ArgumentNullException"><paramref name="Version"/>, <paramref name="fault"/> or <paramref name="action"/> is null. </exception>
    public static Message CreateMessage(MessageVersion version, MessageFault fault, string action)
    {
      if (fault == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("fault"));
      if (version == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("version"));
      else
        return (Message) new BodyWriterMessage(version, action, (BodyWriter) new FaultBodyWriter(fault, version.Envelope));
    }

    internal Exception CreateMessageDisposedException()
    {
      return (Exception) new ObjectDisposedException("", System.ServiceModel.SR.GetString("MessageClosed"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    void IDisposable.Dispose()
    {
      this.Close();
    }

    /// <summary>
    /// Retrieves the body of this <see cref="T:System.ServiceModel.Channels.Message"/> instance.
    /// </summary>
    /// 
    /// <returns>
    /// An object of type <paramref name="T"/> that contains the body of this message.
    /// </returns>
    /// <typeparam name="T">The body of the message.</typeparam>
    [__DynamicallyInvokable]
    public T GetBody<T>()
    {
      return this.OnGetBody<T>(this.GetReaderAtBodyContents());
    }

    /// <summary>
    /// Called when the body of the message is retrieved.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageBuffer"/> that represents the body of the message.
    /// </returns>
    /// <param name="reader">A <see cref="T:System.Xml.XmlDictionaryReader)"/> object used to read the body of the message.</param><typeparam name="T">The type of the message body.</typeparam>
    [__DynamicallyInvokable]
    protected virtual T OnGetBody<T>(XmlDictionaryReader reader)
    {
      return this.GetBodyCore<T>(reader, (XmlObjectSerializer) DataContractSerializerDefaults.CreateSerializer(typeof (T), int.MaxValue));
    }

    /// <summary>
    /// Retrieves the body of this <see cref="T:System.ServiceModel.Channels.Message"/> using the specified serializer.
    /// </summary>
    /// 
    /// <returns>
    /// An object of type <paramref name="T"/> that contains the body of this message.
    /// </returns>
    /// <param name="serializer">A <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> object used to read the body of the message.</param><typeparam name="T">The body of the message.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="serializer"/> is null. </exception>
    [__DynamicallyInvokable]
    public T GetBody<T>(XmlObjectSerializer serializer)
    {
      if (serializer == null)
        throw System.ServiceModel.DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("serializer"));
      else
        return this.GetBodyCore<T>(this.GetReaderAtBodyContents(), serializer);
    }

    private T GetBodyCore<T>(XmlDictionaryReader reader, XmlObjectSerializer serializer)
    {
      T obj;
      using (reader)
      {
        obj = (T) serializer.ReadObject(reader);
        this.ReadFromBodyContentsToEnd(reader);
      }
      return obj;
    }

    internal virtual XmlDictionaryReader GetReaderAtHeader()
    {
      XmlBuffer xmlBuffer = new XmlBuffer(int.MaxValue);
      XmlDictionaryWriter writer = xmlBuffer.OpenSection(XmlDictionaryReaderQuotas.Max);
      this.WriteStartEnvelope(writer);
      MessageHeaders headers = this.Headers;
      for (int headerIndex = 0; headerIndex < headers.Count; ++headerIndex)
        headers.WriteHeader(headerIndex, writer);
      writer.WriteEndElement();
      writer.WriteEndElement();
      xmlBuffer.CloseSection();
      xmlBuffer.Close();
      XmlDictionaryReader reader = xmlBuffer.GetReader(0);
      ((XmlReader) reader).ReadStartElement();
      reader.MoveToStartElement();
      return reader;
    }

    /// <summary>
    /// Gets the XML dictionary reader that accesses the body content of this message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.Xml.XmlDictionaryReader"/> object that accesses the body content of this message.
    /// </returns>
    /// <exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message is empty, or has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public XmlDictionaryReader GetReaderAtBodyContents()
    {
      this.EnsureReadMessageState();
      if (this.IsEmpty)
        throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageIsEmpty")), this);
      else
        return this.OnGetReaderAtBodyContents();
    }

    internal void EnsureReadMessageState()
    {
      switch (this.state)
      {
        case MessageState.Created:
          this.state = MessageState.Read;
          if (!System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
            break;
          TraceUtility.TraceEvent(TraceEventType.Verbose, 524307, System.ServiceModel.SR.GetString("TraceCodeMessageRead"), this);
          break;
        case MessageState.Read:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenRead")), this);
        case MessageState.Written:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenWritten")), this);
        case MessageState.Copied:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenCopied")), this);
        case MessageState.Closed:
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        default:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidMessageState")), this);
      }
    }

    internal SeekableMessageNavigator GetNavigator(bool navigateBody, int maxNodes)
    {
      if (this.IsDisposed)
        throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
      if (this.messageNavigator == null)
        this.messageNavigator = new SeekableMessageNavigator(this, maxNodes, XmlSpace.Default, navigateBody, false);
      else
        this.messageNavigator.ForkNodeCount(maxNodes);
      return this.messageNavigator;
    }

    internal void InitializeReply(Message request)
    {
      UniqueId messageId = request.Headers.MessageId;
      if (messageId == (UniqueId) null)
        throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("RequestMessageDoesNotHaveAMessageID")), request);
      this.Headers.RelatesTo = messageId;
    }

    internal static bool IsFaultStartElement(XmlDictionaryReader reader, EnvelopeVersion version)
    {
      return reader.IsStartElement(XD.MessageDictionary.Fault, version.DictionaryNamespace);
    }

    /// <summary>
    /// Called when the message body is converted to a string.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to convert the message body to a string. </param>
    [__DynamicallyInvokable]
    protected virtual void OnBodyToString(XmlDictionaryWriter writer)
    {
      ((XmlWriter) writer).WriteString(System.ServiceModel.SR.GetString("MessageBodyIsUnknown"));
    }

    /// <summary>
    /// Called when a message buffer is created to store this message.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.ServiceModel.Channels.MessageBuffer"/> object for the newly created message copy.
    /// </returns>
    /// <param name="maxBufferSize">The maximum size of the buffer to be created.</param>
    [__DynamicallyInvokable]
    protected virtual MessageBuffer OnCreateBufferedCopy(int maxBufferSize)
    {
      return this.OnCreateBufferedCopy(maxBufferSize, XmlDictionaryReaderQuotas.Max);
    }

    internal MessageBuffer OnCreateBufferedCopy(int maxBufferSize, XmlDictionaryReaderQuotas quotas)
    {
      XmlBuffer msgBuffer = new XmlBuffer(maxBufferSize);
      this.OnWriteMessage(msgBuffer.OpenSection(quotas));
      msgBuffer.CloseSection();
      msgBuffer.Close();
      return (MessageBuffer) new DefaultMessageBuffer(this, msgBuffer);
    }

    /// <summary>
    /// Called when the message is closing.
    /// </summary>
    [__DynamicallyInvokable]
    protected virtual void OnClose()
    {
    }

    /// <summary>
    /// Called when an XML dictionary reader that accesses the body content of this message is retrieved.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.Xml.XmlDictionaryReader"/> object that accesses the body content of this message.
    /// </returns>
    [__DynamicallyInvokable]
    protected virtual XmlDictionaryReader OnGetReaderAtBodyContents()
    {
      XmlBuffer xmlBuffer = new XmlBuffer(int.MaxValue);
      XmlDictionaryWriter writer = xmlBuffer.OpenSection(XmlDictionaryReaderQuotas.Max);
      if (this.Version.Envelope != EnvelopeVersion.None)
      {
        this.OnWriteStartEnvelope(writer);
        this.OnWriteStartBody(writer);
      }
      this.OnWriteBodyContents(writer);
      if (this.Version.Envelope != EnvelopeVersion.None)
      {
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      xmlBuffer.CloseSection();
      xmlBuffer.Close();
      XmlDictionaryReader reader = xmlBuffer.GetReader(0);
      if (this.Version.Envelope != EnvelopeVersion.None)
      {
        ((XmlReader) reader).ReadStartElement();
        ((XmlReader) reader).ReadStartElement();
      }
      int num = (int) reader.MoveToContent();
      return reader;
    }

    /// <summary>
    /// Called when the start body is written to an XML file.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write the start body to an XML file.</param>
    [__DynamicallyInvokable]
    protected virtual void OnWriteStartBody(XmlDictionaryWriter writer)
    {
      MessageDictionary messageDictionary = XD.MessageDictionary;
      writer.WriteStartElement(messageDictionary.Prefix.Value, messageDictionary.Body, this.Version.Envelope.DictionaryNamespace);
    }

    /// <summary>
    /// Serializes the body content using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the body element.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer "/>is null.</exception><exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public void WriteBodyContents(XmlDictionaryWriter writer)
    {
      this.EnsureWriteMessageState(writer);
      this.OnWriteBodyContents(writer);
    }

    /// <summary>
    /// Starts the asynchronous writing of the contents of the message body.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.
    /// </returns>
    /// <param name="writer">The writer used to serialize the message body.</param><param name="callback">The delegate method that receives the notification when the operation completed.</param><param name="state">The user-defined object that represents the state of the operation.</param>
    public IAsyncResult BeginWriteBodyContents(XmlDictionaryWriter writer, AsyncCallback callback, object state)
    {
      this.EnsureWriteMessageState(writer);
      return this.OnBeginWriteBodyContents(writer, callback, state);
    }

    /// <summary>
    /// Ends the asynchronous writing of the contents of the message body.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.</param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void EndWriteBodyContents(IAsyncResult result)
    {
      this.OnEndWriteBodyContents(result);
    }

    /// <summary>
    /// Called when the message body is written to an XML file.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write this message body to an XML file.</param>
    [__DynamicallyInvokable]
    protected abstract void OnWriteBodyContents(XmlDictionaryWriter writer);

    /// <summary>
    /// Raises an event when the message starts writing the contents of the message body.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.
    /// </returns>
    /// <param name="writer">The writer used to serialize the contents of the message body.</param><param name="callback">The delegate method that receives the notification when the operation completed.</param><param name="state">The user-defined object that represents the state of the operation.</param>
    protected virtual IAsyncResult OnBeginWriteBodyContents(XmlDictionaryWriter writer, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new Message.OnWriteBodyContentsAsyncResult(writer, this, callback, state);
    }

    /// <summary>
    /// Raises an event when writing of the contents of the message body ends.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.</param>
    protected virtual void OnEndWriteBodyContents(IAsyncResult result)
    {
      ScheduleActionItemAsyncResult.End(result);
    }

    /// <summary>
    /// Serializes the start envelope using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the start envelope.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer "/>is null.</exception>
    [__DynamicallyInvokable]
    public void WriteStartEnvelope(XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentNullException("writer"), this);
      this.OnWriteStartEnvelope(writer);
    }

    /// <summary>
    /// Called when the start envelope is written to an XML file.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write the start envelope to an XML file.</param>
    [__DynamicallyInvokable]
    protected virtual void OnWriteStartEnvelope(XmlDictionaryWriter writer)
    {
      EnvelopeVersion envelope = this.Version.Envelope;
      if (envelope == EnvelopeVersion.None)
        return;
      MessageDictionary messageDictionary = XD.MessageDictionary;
      writer.WriteStartElement(messageDictionary.Prefix.Value, messageDictionary.Envelope, envelope.DictionaryNamespace);
      this.WriteSharedHeaderPrefixes(writer);
    }

    /// <summary>
    /// Called when the start header is written to an XML file.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write the start header to an XML file.</param>
    [__DynamicallyInvokable]
    protected virtual void OnWriteStartHeaders(XmlDictionaryWriter writer)
    {
      EnvelopeVersion envelope = this.Version.Envelope;
      if (envelope == EnvelopeVersion.None)
        return;
      MessageDictionary messageDictionary = XD.MessageDictionary;
      writer.WriteStartElement(messageDictionary.Prefix.Value, messageDictionary.Header, envelope.DictionaryNamespace);
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="T:System.ServiceModel.Channels.Message"/> instance.
    /// </summary>
    /// 
    /// <returns>
    /// The string representation of the current <see cref="T:System.ServiceModel.Channels.Message"/> instance.
    /// </returns>
    [__DynamicallyInvokable]
    public override string ToString()
    {
      if (this.IsDisposed)
        return base.ToString();
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      EncodingFallbackAwareXmlTextWriter awareXmlTextWriter = new EncodingFallbackAwareXmlTextWriter((TextWriter) stringWriter);
      awareXmlTextWriter.Formatting = Formatting.Indented;
      XmlDictionaryWriter dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter((XmlWriter) awareXmlTextWriter);
      try
      {
        this.ToString(dictionaryWriter);
        dictionaryWriter.Flush();
        return stringWriter.ToString();
      }
      catch (XmlException ex)
      {
        return System.ServiceModel.SR.GetString("MessageBodyToStringError", (object) ex.GetType().ToString(), (object) ex.Message);
      }
    }

    internal void ToString(XmlDictionaryWriter writer)
    {
      if (this.IsDisposed)
        throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
      if (this.Version.Envelope != EnvelopeVersion.None)
      {
        this.WriteStartEnvelope(writer);
        this.WriteStartHeaders(writer);
        MessageHeaders headers = this.Headers;
        for (int headerIndex = 0; headerIndex < headers.Count; ++headerIndex)
          headers.WriteHeader(headerIndex, writer);
        writer.WriteEndElement();
        MessageDictionary messageDictionary = XD.MessageDictionary;
        this.WriteStartBody(writer);
      }
      this.BodyToString(writer);
      if (this.Version.Envelope == EnvelopeVersion.None)
        return;
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    /// <summary>
    /// Retrieves the attributes of the message body.
    /// </summary>
    /// 
    /// <returns>
    /// The attributes of the message body.
    /// </returns>
    /// <param name="localName">The local name of the XML node.The name of the element that corresponds to this member. This string must be a valid XML element name.</param><param name="ns">The namespace to which this XML element belongs.The namespace URI of the element that corresponds to this member. The system does not validate any URIs other than transport addresses.</param><exception cref="T:System.ArgumentNullException"><paramref name="localName"/> or <paramref name="ns"/> is null. </exception><exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public string GetBodyAttribute(string localName, string ns)
    {
      if (localName == null)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentNullException("localName"), this);
      if (ns == null)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentNullException("ns"), this);
      switch (this.state)
      {
        case MessageState.Created:
          return this.OnGetBodyAttribute(localName, ns);
        case MessageState.Read:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenRead")), this);
        case MessageState.Written:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenWritten")), this);
        case MessageState.Copied:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenCopied")), this);
        case MessageState.Closed:
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        default:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidMessageState")), this);
      }
    }

    /// <summary>
    /// Called when the attributes of the message body is retrieved.
    /// </summary>
    /// 
    /// <returns>
    /// The attributes of the message body.
    /// </returns>
    /// <param name="localName">The local name of the XML node.The name of the element that corresponds to this member. This string must be a valid XML element name.</param><param name="ns">The namespace to which this XML element belongs.The namespace URI of the element that corresponds to this member. The system does not validate any URIs other than transport addresses.</param>
    [__DynamicallyInvokable]
    protected virtual string OnGetBodyAttribute(string localName, string ns)
    {
      return (string) null;
    }

    internal void ReadFromBodyContentsToEnd(XmlDictionaryReader reader)
    {
      Message.ReadFromBodyContentsToEnd(reader, this.Version.Envelope);
    }

    internal static bool ReadStartBody(XmlDictionaryReader reader, EnvelopeVersion envelopeVersion, out bool isFault, out bool isEmpty)
    {
      if (reader.IsEmptyElement)
      {
        reader.Read();
        isEmpty = true;
        isFault = false;
        reader.ReadEndElement();
        return false;
      }
      else
      {
        reader.Read();
        if (reader.NodeType != XmlNodeType.Element)
        {
          int num = (int) reader.MoveToContent();
        }
        if (reader.NodeType == XmlNodeType.Element)
        {
          isFault = Message.IsFaultStartElement(reader, envelopeVersion);
          isEmpty = false;
        }
        else if (reader.NodeType == XmlNodeType.EndElement)
        {
          isEmpty = true;
          isFault = false;
          Message.ReadFromBodyContentsToEnd(reader, envelopeVersion);
          return false;
        }
        else
        {
          isEmpty = false;
          isFault = false;
        }
        return true;
      }
    }

    /// <summary>
    /// Serializes the message body using the specified <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> object to be used to write the body of the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception><exception cref="T:System.ObjectDisposedException">The message is disposed. </exception>
    [__DynamicallyInvokable]
    public void WriteBody(XmlWriter writer)
    {
      this.WriteBody(XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Writes the body element using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the body element.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception>
    [__DynamicallyInvokable]
    public void WriteBody(XmlDictionaryWriter writer)
    {
      this.WriteStartBody(writer);
      this.WriteBodyContents(writer);
      writer.WriteEndElement();
    }

    /// <summary>
    /// Serializes the start body of the message using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the start body of the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception>
    [__DynamicallyInvokable]
    public void WriteStartBody(XmlWriter writer)
    {
      this.WriteStartBody(XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Serializes the start body of the message using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the start body.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception>
    [__DynamicallyInvokable]
    public void WriteStartBody(XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentNullException("writer"), this);
      this.OnWriteStartBody(writer);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void WriteStartHeaders(XmlDictionaryWriter writer)
    {
      this.OnWriteStartHeaders(writer);
    }

    /// <summary>
    /// Serializes the entire message using the specified <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> object to be used to write the entire message.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception><exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public void WriteMessage(XmlWriter writer)
    {
      this.WriteMessage(XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Serializes the entire message using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> object to be used to write the message.</param><exception cref="T:System.ArgumentNullException"><paramref name="writer"/> is null. </exception><exception cref="T:System.ObjectDisposedException">The message is closed.</exception><exception cref="T:System.InvalidOperationException">The message has been copied, read or written.</exception>
    [__DynamicallyInvokable]
    public void WriteMessage(XmlDictionaryWriter writer)
    {
      this.EnsureWriteMessageState(writer);
      this.OnWriteMessage(writer);
    }

    /// <summary>
    /// Starts the asynchronous writing of the entire message.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.
    /// </returns>
    /// <param name="writer">The writer used to serialize the entire message.</param><param name="callback">The delegate method that receives the notification when the operation completed.</param><param name="state">The user-defined object that represents the state of the operation.</param>
    public IAsyncResult BeginWriteMessage(XmlDictionaryWriter writer, AsyncCallback callback, object state)
    {
      this.EnsureWriteMessageState(writer);
      return this.OnBeginWriteMessage(writer, callback, state);
    }

    /// <summary>
    /// Ends the asynchronous writing of the entire message.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.</param>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void EndWriteMessage(IAsyncResult result)
    {
      this.OnEndWriteMessage(result);
    }

    /// <summary>
    /// Called when the entire message is written to an XML file.
    /// </summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlDictionaryWriter"/> that is used to write this message to an XML file.</param>
    [__DynamicallyInvokable]
    protected virtual void OnWriteMessage(XmlDictionaryWriter writer)
    {
      this.WriteMessagePreamble(writer);
      this.OnWriteBodyContents(writer);
      this.WriteMessagePostamble(writer);
    }

    internal void WriteMessagePreamble(XmlDictionaryWriter writer)
    {
      if (this.Version.Envelope == EnvelopeVersion.None)
        return;
      this.OnWriteStartEnvelope(writer);
      MessageHeaders headers = this.Headers;
      int count = headers.Count;
      if (count > 0)
      {
        this.OnWriteStartHeaders(writer);
        for (int headerIndex = 0; headerIndex < count; ++headerIndex)
          headers.WriteHeader(headerIndex, writer);
        writer.WriteEndElement();
      }
      this.OnWriteStartBody(writer);
    }

    internal void WriteMessagePostamble(XmlDictionaryWriter writer)
    {
      if (this.Version.Envelope == EnvelopeVersion.None)
        return;
      writer.WriteEndElement();
      writer.WriteEndElement();
    }

    /// <summary>
    /// Raises an event the writing of entire messages starts.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.
    /// </returns>
    /// <param name="writer">The writer used to serialize the entire message.</param><param name="callback">The delegate method that receives the notification when the operation completed.</param><param name="state">The user-defined object that represents the state of the operation.</param>
    protected virtual IAsyncResult OnBeginWriteMessage(XmlDictionaryWriter writer, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new Message.OnWriteMessageAsyncResult(writer, this, callback, state);
    }

    /// <summary>
    /// Raises an event when the writing of the entire message ends.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> object that represents the result of the asynchronous operation.</param>
    protected virtual void OnEndWriteMessage(IAsyncResult result)
    {
      ScheduleActionItemAsyncResult.End(result);
    }

    private static System.Type GetObjectType(object value)
    {
      if (value != null)
        return value.GetType();
      else
        return typeof (object);
    }

    private static void ReadFromBodyContentsToEnd(XmlDictionaryReader reader, EnvelopeVersion envelopeVersion)
    {
      if (envelopeVersion != EnvelopeVersion.None)
      {
        reader.ReadEndElement();
        reader.ReadEndElement();
      }
      int num = (int) reader.MoveToContent();
    }

    private void EnsureWriteMessageState(XmlDictionaryWriter writer)
    {
      if (writer == null)
        throw TraceUtility.ThrowHelperError((Exception) new ArgumentNullException("writer"), this);
      switch (this.state)
      {
        case MessageState.Created:
          this.state = MessageState.Written;
          if (!System.ServiceModel.DiagnosticUtility.ShouldTraceVerbose)
            break;
          TraceUtility.TraceEvent(TraceEventType.Verbose, 524308, System.ServiceModel.SR.GetString("TraceCodeMessageWritten"), this);
          break;
        case MessageState.Read:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenRead")), this);
        case MessageState.Written:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenWritten")), this);
        case MessageState.Copied:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("MessageHasBeenCopied")), this);
        case MessageState.Closed:
          throw TraceUtility.ThrowHelperError(this.CreateMessageDisposedException(), this);
        default:
          throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InvalidMessageState")), this);
      }
    }

    private void WriteSharedHeaderPrefixes(XmlDictionaryWriter writer)
    {
      MessageHeaders headers = this.Headers;
      int count = headers.Count;
      int num1 = 0;
      for (int index = 0; index < count; ++index)
      {
        if (this.Version.Addressing != AddressingVersion.None || !(headers[index].Namespace == AddressingVersion.None.Namespace))
        {
          IMessageHeaderWithSharedNamespace withSharedNamespace = headers[index] as IMessageHeaderWithSharedNamespace;
          if (withSharedNamespace != null)
          {
            string prefix = withSharedNamespace.SharedPrefix.Value;
            if (prefix.Length != 1)
              throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "IMessageHeaderWithSharedNamespace must use a single lowercase letter prefix.", new object[0])), this);
            int num2 = (int) prefix[0] - 97;
            if (num2 < 0 || num2 >= 26)
              throw TraceUtility.ThrowHelperError((Exception) new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "IMessageHeaderWithSharedNamespace must use a single lowercase letter prefix.", new object[0])), this);
            int num3 = 1 << num2;
            if ((num1 & num3) == 0)
            {
              writer.WriteXmlnsAttribute(prefix, withSharedNamespace.SharedNamespace);
              num1 |= num3;
            }
          }
        }
      }
    }

    private class OnWriteBodyContentsAsyncResult : ScheduleActionItemAsyncResult
    {
      private Message message;
      private XmlDictionaryWriter writer;

      public OnWriteBodyContentsAsyncResult(XmlDictionaryWriter writer, Message message, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.message = message;
        this.writer = writer;
        this.Schedule();
      }

      protected override void OnDoWork()
      {
        this.message.OnWriteBodyContents(this.writer);
      }
    }

    private class OnWriteMessageAsyncResult : ScheduleActionItemAsyncResult
    {
      private Message message;
      private XmlDictionaryWriter writer;

      public OnWriteMessageAsyncResult(XmlDictionaryWriter writer, Message message, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.message = message;
        this.writer = writer;
        this.Schedule();
      }

      protected override void OnDoWork()
      {
        this.message.OnWriteMessage(this.writer);
      }
    }
  }
}
