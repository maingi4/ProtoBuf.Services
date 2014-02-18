// Type: System.Xml.XmlDictionaryReader
// Assembly: System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Runtime.Serialization.dll

using System;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml
{
  /// <summary>
  /// An abstract class that the Windows Communication Foundation (WCF) derives from to do serialization and deserialization.
  /// </summary>
  [__DynamicallyInvokable]
  public abstract class XmlDictionaryReader : XmlReader
  {
    internal const int MaxInitialArrayLength = 65535;

    /// <summary>
    /// This property always returns false. Its derived classes can override to return true if they support canonicalization.
    /// </summary>
    /// 
    /// <returns>
    /// Returns false.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual bool CanCanonicalize
    {
      [__DynamicallyInvokable] get
      {
        return false;
      }
    }

    /// <summary>
    /// Gets the quota values that apply to the current instance of this class.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> that applies to the current instance of this class.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual XmlDictionaryReaderQuotas Quotas
    {
      [__DynamicallyInvokable] get
      {
        return XmlDictionaryReaderQuotas.Max;
      }
    }

    /// <summary>
    /// Creates an instance of this class.  Invoked only by its derived classes.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected XmlDictionaryReader()
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> from an existing <see cref="T:System.Xml.XmlReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="reader">An instance of <see cref="T:System.Xml.XmlReader"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
    [__DynamicallyInvokable]
    public static XmlDictionaryReader CreateDictionaryReader(XmlReader reader)
    {
      if (reader == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
      else
        return reader as XmlDictionaryReader ?? (XmlDictionaryReader) new XmlDictionaryReader.XmlWrappedReader(reader, (XmlNamespaceManager) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="quotas">The quotas that apply to this operation.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception>
    [__DynamicallyInvokable]
    public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
    {
      if (buffer == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
      else
        return XmlDictionaryReader.CreateBinaryReader(buffer, 0, buffer.Length, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="quotas">The quotas that apply to this operation.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is less than zero or greater than the buffer length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is less than zero or greater than the buffer length minus the offset.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, (IXmlDictionary) null, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="dictionary"><see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas">The quotas that apply to this operation.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is less than zero or greater than the buffer length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is less than zero or greater than the buffer length minus the offset.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, dictionary, quotas, (XmlBinaryReaderSession) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="dictionary">The <see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply.</param><param name="session">The <see cref="T:System.Xml.XmlBinaryReaderSession"/> to use.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is less than zero or greater than the buffer length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is less than zero or greater than the buffer length minus the offset.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
    {
      return XmlDictionaryReader.CreateBinaryReader(buffer, offset, count, dictionary, quotas, session, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="dictionary">The <see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply.</param><param name="session">The <see cref="T:System.Xml.XmlBinaryReaderSession"/> to use.</param><param name="onClose">Delegate to be called when the reader is closed.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is less than zero or greater than the buffer length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is less than zero or greater than the buffer length minus the offset.</exception>
    public static XmlDictionaryReader CreateBinaryReader(byte[] buffer, int offset, int count, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
    {
      XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
      xmlBinaryReader.SetInput(buffer, offset, count, dictionary, quotas, session, onClose);
      return (XmlDictionaryReader) xmlBinaryReader;
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="quotas">The quotas that apply to this operation.</param><exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(Stream stream, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateBinaryReader(stream, (IXmlDictionary) null, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="dictionary"><see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas">The quotas that apply to this operation.</param><exception cref="T:System.ArgumentNullException"><paramref name="stream"/> or <paramref name="quotas"/> is null.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateBinaryReader(stream, dictionary, quotas, (XmlBinaryReaderSession) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="dictionary"><see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas">The quotas that apply to this operation.</param><param name="session"><see cref="T:System.Xml.XmlBinaryReaderSession"/> to use.</param><exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session)
    {
      return XmlDictionaryReader.CreateBinaryReader(stream, dictionary, quotas, session, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that can read .NET Binary XML Format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="dictionary"><see cref="T:System.Xml.XmlDictionary"/> to use.</param><param name="quotas"><see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply.</param><param name="session"><see cref="T:System.Xml.XmlBinaryReaderSession"/> to use.</param><param name="onClose">Delegate to be called when the reader is closed.</param><exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
    public static XmlDictionaryReader CreateBinaryReader(Stream stream, IXmlDictionary dictionary, XmlDictionaryReaderQuotas quotas, XmlBinaryReaderSession session, OnXmlDictionaryReaderClose onClose)
    {
      XmlBinaryReader xmlBinaryReader = new XmlBinaryReader();
      xmlBinaryReader.SetInput(stream, dictionary, quotas, session, onClose);
      return (XmlDictionaryReader) xmlBinaryReader;
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="quotas">The quotas applied to the reader.</param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception>
    [__DynamicallyInvokable]
    public static XmlDictionaryReader CreateTextReader(byte[] buffer, XmlDictionaryReaderQuotas quotas)
    {
      if (buffer == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
      else
        return XmlDictionaryReader.CreateTextReader(buffer, 0, buffer.Length, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="quotas">The quotas applied to the reader.</param>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateTextReader(buffer, offset, count, (Encoding) null, quotas, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="encoding">The <see cref="T:System.Text.Encoding"/> object that specifies the encoding properties to apply.</param><param name="quotas">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply.</param><param name="onClose">The delegate to be called when the reader is closed.</param>
    public static XmlDictionaryReader CreateTextReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
    {
      XmlUTF8TextReader xmlUtF8TextReader = new XmlUTF8TextReader();
      xmlUtF8TextReader.SetInput(buffer, offset, count, encoding, quotas, onClose);
      return (XmlDictionaryReader) xmlUtF8TextReader;
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="quotas">The quotas applied to the reader.</param>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateTextReader(Stream stream, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateTextReader(stream, (Encoding) null, quotas, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="encoding">The <see cref="T:System.Text.Encoding"/> object that specifies the encoding properties to apply.</param><param name="quotas">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply.</param><param name="onClose">The delegate to be called when the reader is closed.</param>
    public static XmlDictionaryReader CreateTextReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas, OnXmlDictionaryReaderClose onClose)
    {
      XmlUTF8TextReader xmlUtF8TextReader = new XmlUTF8TextReader();
      xmlUtF8TextReader.SetInput(stream, encoding, quotas, onClose);
      return (XmlDictionaryReader) xmlUtF8TextReader;
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="encoding">The possible character encoding of the stream.</param><param name="quotas">The quotas to apply to this reader.</param><exception cref="T:System.ArgumentNullException"><paramref name="encoding"/> is null.</exception><filterpriority>2</filterpriority>
    public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding encoding, XmlDictionaryReaderQuotas quotas)
    {
      if (encoding == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
      return XmlDictionaryReader.CreateMtomReader(stream, new Encoding[1]
      {
        encoding
      }, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="encodings">The possible character encodings of the stream.</param><param name="quotas">The quotas to apply to this reader.</param><exception cref="T:System.ArgumentNullException"><paramref name="encoding"/> is null.</exception><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateMtomReader(stream, encodings, (string) null, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="encodings">The possible character encodings of the stream.</param><param name="contentType">The Content-Type MIME type of the message.</param><param name="quotas">The quotas to apply to this reader.</param><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateMtomReader(stream, encodings, contentType, quotas, int.MaxValue, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="stream">The stream from which to read.</param><param name="encodings">The possible character encodings of the stream.</param><param name="contentType">The Content-Type MIME type of the message.</param><param name="quotas">The MIME type of the message.</param><param name="maxBufferSize">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply to the reader.</param><param name="onClose">The delegate to be called when the reader is closed.</param><filterpriority>2</filterpriority>
    public static XmlDictionaryReader CreateMtomReader(Stream stream, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
    {
      XmlMtomReader xmlMtomReader = new XmlMtomReader();
      xmlMtomReader.SetInput(stream, encodings, contentType, quotas, maxBufferSize, onClose);
      return (XmlDictionaryReader) xmlMtomReader;
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="encoding">The possible character encoding of the input.</param><param name="quotas">The quotas to apply to this reader.</param><exception cref="T:System.ArgumentNullException"><paramref name="encoding"/> is null.</exception><filterpriority>2</filterpriority>
    public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding encoding, XmlDictionaryReaderQuotas quotas)
    {
      if (encoding == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
      return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, new Encoding[1]
      {
        encoding
      }, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="encodings">The possible character encodings of the input.</param><param name="quotas">The quotas to apply to this reader.</param><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, encodings, (string) null, quotas);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="encodings">The possible character encodings of the input.</param><param name="contentType">The Content-Type MIME type of the message.</param><param name="quotas">The quotas to apply to this reader.</param><filterpriority>2</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas)
    {
      return XmlDictionaryReader.CreateMtomReader(buffer, offset, count, encodings, contentType, quotas, int.MaxValue, (OnXmlDictionaryReaderClose) null);
    }

    /// <summary>
    /// Creates an instance of <see cref="T:System.Xml.XmlDictionaryReader"/> that reads XML in the MTOM format.
    /// </summary>
    /// 
    /// <returns>
    /// An instance of <see cref="T:System.Xml.XmlDictionaryReader"/>.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><param name="encodings">The possible character encodings of the input.</param><param name="contentType">The Content-Type MIME type of the message.</param><param name="quotas">The <see cref="T:System.Xml.XmlDictionaryReaderQuotas"/> to apply to the reader.</param><param name="maxBufferSize">The maximum allowed size of the buffer.</param><param name="onClose">The delegate to be called when the reader is closed.</param><filterpriority>2</filterpriority>
    public static XmlDictionaryReader CreateMtomReader(byte[] buffer, int offset, int count, Encoding[] encodings, string contentType, XmlDictionaryReaderQuotas quotas, int maxBufferSize, OnXmlDictionaryReaderClose onClose)
    {
      XmlMtomReader xmlMtomReader = new XmlMtomReader();
      xmlMtomReader.SetInput(buffer, offset, count, encodings, contentType, quotas, maxBufferSize, onClose);
      return (XmlDictionaryReader) xmlMtomReader;
    }

    /// <summary>
    /// This method is not yet implemented.
    /// </summary>
    /// <param name="stream">The stream to read from.</param><param name="includeComments">Determines whether comments are included.</param><param name="inclusivePrefixes">The prefixes to be included.</param><exception cref="T:System.NotSupportedException">Always.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    /// <summary>
    /// This method is not yet implemented.
    /// </summary>
    /// <exception cref="T:System.NotSupportedException">Always.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual void EndCanonicalization()
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    /// <summary>
    /// Tests whether the current content node is a start element or an empty element.
    /// </summary>
    [__DynamicallyInvokable]
    public virtual void MoveToStartElement()
    {
      if (base.IsStartElement())
        return;
      XmlExceptionHelper.ThrowStartElementExpected(this);
    }

    /// <summary>
    /// Tests whether the current content node is a start element or an empty element and if the <see cref="P:System.Xml.XmlReader.Name"/> property of the element matches the given argument.
    /// </summary>
    /// <param name="name">The <see cref="P:System.Xml.XmlReader.Name"/> property of the element.</param>
    [__DynamicallyInvokable]
    public virtual void MoveToStartElement(string name)
    {
      if (base.IsStartElement(name))
        return;
      XmlExceptionHelper.ThrowStartElementExpected(this, name);
    }

    /// <summary>
    /// Tests whether the current content node is a start element or an empty element and if the <see cref="P:System.Xml.XmlReader.LocalName"/> and <see cref="P:System.Xml.XmlReader.NamespaceURI"/> properties of the element matches the given arguments.
    /// </summary>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual void MoveToStartElement(string localName, string namespaceUri)
    {
      if (base.IsStartElement(localName, namespaceUri))
        return;
      XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
    }

    /// <summary>
    /// Tests whether the current content node is a start element or an empty element and if the <see cref="P:System.Xml.XmlReader.LocalName"/> and <see cref="P:System.Xml.XmlReader.NamespaceURI"/> properties of the element matches the given argument.
    /// </summary>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual void MoveToStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      if (this.IsStartElement(localName, namespaceUri))
        return;
      XmlExceptionHelper.ThrowStartElementExpected(this, localName, namespaceUri);
    }

    /// <summary>
    /// Checks whether the parameter, <paramref name="localName"/>, is the local name of the current node.
    /// </summary>
    /// 
    /// <returns>
    /// true if <paramref name="localName"/> matches local name of the current node; otherwise false.
    /// </returns>
    /// <param name="localName">The local name of the current node.</param>
    [__DynamicallyInvokable]
    public virtual bool IsLocalName(string localName)
    {
      return this.LocalName == localName;
    }

    /// <summary>
    /// Checks whether the parameter, <paramref name="localName"/>, is the local name of the current node.
    /// </summary>
    /// 
    /// <returns>
    /// true if <paramref name="localName"/> matches local name of the current node; otherwise false.
    /// </returns>
    /// <param name="localName">An <see cref="T:System.Xml.XmlDictionaryString"/> that represents the local name of the current node.</param><exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual bool IsLocalName(XmlDictionaryString localName)
    {
      if (localName == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localName");
      else
        return this.IsLocalName(localName.Value);
    }

    /// <summary>
    /// Checks whether the parameter, <paramref name="namespaceUri"/>, is the namespace of the current node.
    /// </summary>
    /// 
    /// <returns>
    /// true if <paramref name="namespaceUri"/> matches namespace of the current node; otherwise false.
    /// </returns>
    /// <param name="namespaceUri">The namespace of current node.</param><exception cref="T:System.ArgumentNullException"><paramref name="namespaceUri"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual bool IsNamespaceUri(string namespaceUri)
    {
      if (namespaceUri == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
      else
        return this.NamespaceURI == namespaceUri;
    }

    /// <summary>
    /// Checks whether the parameter, <paramref name="namespaceUri"/>, is the namespace of the current node.
    /// </summary>
    /// 
    /// <returns>
    /// true if <paramref name="namespaceUri"/> matches namespace of the current node; otherwise false.
    /// </returns>
    /// <param name="namespaceUri">Namespace of current node.</param><exception cref="T:System.ArgumentNullException"><paramref name="namespaceUri"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual bool IsNamespaceUri(XmlDictionaryString namespaceUri)
    {
      if (namespaceUri == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
      else
        return this.IsNamespaceUri(namespaceUri.Value);
    }

    /// <summary>
    /// Checks whether the current node is an element and advances the reader to the next node.
    /// </summary>
    /// <exception cref="T:System.Xml.XmlException"><see cref="M:System.Xml.XmlDictionaryReader.IsStartElement(System.Xml.XmlDictionaryString,System.Xml.XmlDictionaryString)"/> returns false.</exception>
    [__DynamicallyInvokable]
    public virtual void ReadFullStartElement()
    {
      this.MoveToStartElement();
      if (this.IsEmptyElement)
        XmlExceptionHelper.ThrowFullStartElementExpected(this);
      this.Read();
    }

    /// <summary>
    /// Checks whether the current node is an element with the given <paramref name="name"/> and advances the reader to the next node.
    /// </summary>
    /// <param name="name">The qualified name of the element.</param><exception cref="T:System.Xml.XmlException"><see cref="M:System.Xml.XmlDictionaryReader.IsStartElement(System.Xml.XmlDictionaryString,System.Xml.XmlDictionaryString)"/> returns false.</exception>
    [__DynamicallyInvokable]
    public virtual void ReadFullStartElement(string name)
    {
      this.MoveToStartElement(name);
      if (this.IsEmptyElement)
        XmlExceptionHelper.ThrowFullStartElementExpected(this, name);
      this.Read();
    }

    /// <summary>
    /// Checks whether the current node is an element with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> and advances the reader to the next node.
    /// </summary>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><exception cref="T:System.Xml.XmlException"><see cref="M:System.Xml.XmlDictionaryReader.IsStartElement(System.Xml.XmlDictionaryString,System.Xml.XmlDictionaryString)"/> returns false.</exception>
    [__DynamicallyInvokable]
    public virtual void ReadFullStartElement(string localName, string namespaceUri)
    {
      this.MoveToStartElement(localName, namespaceUri);
      if (this.IsEmptyElement)
        XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
      this.Read();
    }

    /// <summary>
    /// Checks whether the current node is an element with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> and advances the reader to the next node.
    /// </summary>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><exception cref="T:System.Xml.XmlException"><see cref="M:System.Xml.XmlDictionaryReader.IsStartElement(System.Xml.XmlDictionaryString,System.Xml.XmlDictionaryString)"/> returns false.</exception>
    [__DynamicallyInvokable]
    public virtual void ReadFullStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      this.MoveToStartElement(localName, namespaceUri);
      if (this.IsEmptyElement)
        XmlExceptionHelper.ThrowFullStartElementExpected(this, localName, namespaceUri);
      this.Read();
    }

    /// <summary>
    /// Checks whether the current node is an element with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> and advances the reader to the next node.
    /// </summary>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual void ReadStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      this.MoveToStartElement(localName, namespaceUri);
      this.Read();
    }

    /// <summary>
    /// Tests whether the first tag is a start tag or empty element tag and if the local name and namespace URI match those of the current node.
    /// </summary>
    /// 
    /// <returns>
    /// true if the first tag in the array is a start tag or empty element tag and matches <paramref name="localName"/> and <paramref name="namespaceUri"/>; otherwise false.
    /// </returns>
    /// <param name="localName">An <see cref="T:System.Xml.XmlDictionaryString"/> that represents the local name of the attribute.</param><param name="namespaceUri">An <see cref="T:System.Xml.XmlDictionaryString"/> that represents the namespace of the attribute.</param>
    [__DynamicallyInvokable]
    public virtual bool IsStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return base.IsStartElement(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
    }

    /// <summary>
    /// Gets the index of the local name of the current node within an array of names.
    /// </summary>
    /// 
    /// <returns>
    /// The index of the local name of the current node within an array of names.
    /// </returns>
    /// <param name="localNames">The string array of local names to be searched.</param><param name="namespaceUri">The namespace of current node.</param><exception cref="T:System.ArgumentNullException"><paramref name="localNames"/> or any of the names in the array is null.</exception><exception cref="T:System.ArgumentNullException"><paramref name="namespaceUri"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual int IndexOfLocalName(string[] localNames, string namespaceUri)
    {
      if (localNames == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
      if (namespaceUri == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
      if (this.NamespaceURI == namespaceUri)
      {
        string localName = this.LocalName;
        for (int index = 0; index < localNames.Length; ++index)
        {
          string str = localNames[index];
          if (str == null)
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "localNames[{0}]", new object[1]
            {
              (object) index
            }));
          else if (localName == str)
            return index;
        }
      }
      return -1;
    }

    /// <summary>
    /// Gets the index of the local name of the current node within an array of names.
    /// </summary>
    /// 
    /// <returns>
    /// The index of the local name of the current node within an array of names.
    /// </returns>
    /// <param name="localNames">The <see cref="T:System.Xml.XmlDictionaryString"/> array of local names to be searched.</param><param name="namespaceUri">The namespace of current node.</param><exception cref="T:System.ArgumentNullException"><paramref name="localNames"/> or any of the names in the array is null.</exception><exception cref="T:System.ArgumentNullException"><paramref name="namespaceUri"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual int IndexOfLocalName(XmlDictionaryString[] localNames, XmlDictionaryString namespaceUri)
    {
      if (localNames == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("localNames");
      if (namespaceUri == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
      if (this.NamespaceURI == namespaceUri.Value)
      {
        string localName = this.LocalName;
        for (int index = 0; index < localNames.Length; ++index)
        {
          XmlDictionaryString dictionaryString = localNames[index];
          if (dictionaryString == null)
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "localNames[{0}]", new object[1]
            {
              (object) index
            }));
          else if (localName == dictionaryString.Value)
            return index;
        }
      }
      return -1;
    }

    /// <summary>
    /// When overridden in a derived class, gets the value of an attribute.
    /// </summary>
    /// 
    /// <returns>
    /// The value of the attribute.
    /// </returns>
    /// <param name="localName">An <see cref="T:System.Xml.XmlDictionaryString"/> that represents the local name of the attribute.</param><param name="namespaceUri">An <see cref="T:System.Xml.XmlDictionaryString"/> that represents the namespace of the attribute.</param>
    [__DynamicallyInvokable]
    public virtual string GetAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return base.GetAttribute(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
    }

    /// <summary>
    /// Not implemented in this class (it always returns false). May be overridden in derived classes.
    /// </summary>
    /// 
    /// <returns>
    /// false, unless overridden in a derived class.
    /// </returns>
    /// <param name="length">Returns 0, unless overridden in a derived class.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual bool TryGetBase64ContentLength(out int length)
    {
      length = 0;
      return false;
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// 
    /// <returns>
    /// Not implemented.
    /// </returns>
    /// <param name="buffer">The buffer from which to read.</param><param name="offset">The starting position from which to read in <paramref name="buffer"/>.</param><param name="count">The number of bytes that can be read from <paramref name="buffer"/>.</param><exception cref="T:System.NotSupportedException">Always.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadValueAsBase64(byte[] buffer, int offset, int count)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    /// <summary>
    /// Reads the content and returns the Base64 decoded binary bytes.
    /// </summary>
    /// 
    /// <returns>
    /// A byte array that contains the Base64 decoded binary bytes.
    /// </returns>
    /// <exception cref="T:System.Xml.XmlException">The array size is greater than the MaxArrayLength quota for this reader.</exception>
    [__DynamicallyInvokable]
    public virtual byte[] ReadContentAsBase64()
    {
      return this.ReadContentAsBase64(this.Quotas.MaxArrayLength, (int) ushort.MaxValue);
    }

    internal byte[] ReadContentAsBase64(int maxByteArrayContentLength, int maxInitialCount)
    {
      int length;
      if (this.TryGetBase64ContentLength(out length))
      {
        if (length > maxByteArrayContentLength)
          XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
        if (length <= maxInitialCount)
        {
          byte[] buffer = new byte[length];
          int index = 0;
          while (index < length)
          {
            int num = base.ReadContentAsBase64(buffer, index, length - index);
            if (num == 0)
              XmlExceptionHelper.ThrowBase64DataExpected(this);
            index += num;
          }
          return buffer;
        }
      }
      return this.ReadContentAsBytes(true, maxByteArrayContentLength);
    }

    /// <summary>
    /// Converts a node's content to a string.
    /// </summary>
    /// 
    /// <returns>
    /// The node content in a string representation.
    /// </returns>
    [__DynamicallyInvokable]
    public override string ReadContentAsString()
    {
      return this.ReadContentAsString(this.Quotas.MaxStringContentLength);
    }

    /// <summary>
    /// Converts a node's content to a string.
    /// </summary>
    /// 
    /// <returns>
    /// Node content in string representation.
    /// </returns>
    /// <param name="maxStringContentLength">The maximum string length.</param>
    [__DynamicallyInvokable]
    protected string ReadContentAsString(int maxStringContentLength)
    {
      StringBuilder stringBuilder = (StringBuilder) null;
      string str1 = string.Empty;
      bool flag = false;
      while (true)
      {
        switch (this.NodeType)
        {
          case XmlNodeType.Attribute:
            str1 = this.Value;
            goto case XmlNodeType.ProcessingInstruction;
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            string str2 = this.Value;
            if (str1.Length == 0)
            {
              str1 = str2;
              goto case XmlNodeType.ProcessingInstruction;
            }
            else
            {
              if (stringBuilder == null)
                stringBuilder = new StringBuilder(str1);
              if (stringBuilder.Length > maxStringContentLength - str2.Length)
                XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
              stringBuilder.Append(str2);
              goto case XmlNodeType.ProcessingInstruction;
            }
          case XmlNodeType.EntityReference:
            if (this.CanResolveEntity)
            {
              this.ResolveEntity();
              goto case XmlNodeType.ProcessingInstruction;
            }
            else
              break;
          case XmlNodeType.ProcessingInstruction:
          case XmlNodeType.Comment:
          case XmlNodeType.EndEntity:
            if (!flag)
            {
              if (this.AttributeCount != 0)
              {
                this.ReadAttributeValue();
                continue;
              }
              else
              {
                this.Read();
                continue;
              }
            }
            else
              goto label_17;
        }
        flag = true;
        goto case XmlNodeType.ProcessingInstruction;
      }
label_17:
      if (stringBuilder != null)
        str1 = ((object) stringBuilder).ToString();
      if (str1.Length > maxStringContentLength)
        XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
      return str1;
    }

    /// <summary>
    /// Reads the contents of the current node into a string.
    /// </summary>
    /// 
    /// <returns>
    /// A string that contains the contents of the current node.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">Unable to read the contents of the current node.</exception><exception cref="T:System.Xml.XmlException">Maximum allowed string length exceeded.</exception>
    public override string ReadString()
    {
      return this.ReadString(this.Quotas.MaxStringContentLength);
    }

    /// <summary>
    /// Reads the contents of the current node into a string with a given maximum length.
    /// </summary>
    /// 
    /// <returns>
    /// A string that contains the contents of the current node.
    /// </returns>
    /// <param name="maxStringContentLength">Maximum allowed string length.</param><exception cref="T:System.InvalidOperationException">Unable to read the contents of the current node.</exception><exception cref="T:System.Xml.XmlException">Maximum allowed string length exceeded.</exception>
    protected string ReadString(int maxStringContentLength)
    {
      if (this.ReadState != ReadState.Interactive)
        return string.Empty;
      if (this.NodeType != XmlNodeType.Element)
        this.MoveToElement();
      if (this.NodeType == XmlNodeType.Element)
      {
        if (this.IsEmptyElement)
          return string.Empty;
        if (!this.Read())
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.Runtime.Serialization.SR.GetString("XmlInvalidOperation")));
        if (this.NodeType == XmlNodeType.EndElement)
          return string.Empty;
      }
      StringBuilder stringBuilder = (StringBuilder) null;
      string str1 = string.Empty;
      while (this.IsTextNode(this.NodeType))
      {
        string str2 = this.Value;
        if (str1.Length == 0)
        {
          str1 = str2;
        }
        else
        {
          if (stringBuilder == null)
            stringBuilder = new StringBuilder(str1);
          if (stringBuilder.Length > maxStringContentLength - str2.Length)
            XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
          stringBuilder.Append(str2);
        }
        if (!this.Read())
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.Runtime.Serialization.SR.GetString("XmlInvalidOperation")));
      }
      if (stringBuilder != null)
        str1 = ((object) stringBuilder).ToString();
      if (str1.Length > maxStringContentLength)
        XmlExceptionHelper.ThrowMaxStringContentLengthExceeded(this, maxStringContentLength);
      return str1;
    }

    /// <summary>
    /// Reads the content and returns the BinHex decoded binary bytes.
    /// </summary>
    /// 
    /// <returns>
    /// A byte array that contains the BinHex decoded binary bytes.
    /// </returns>
    /// <exception cref="T:System.Xml.XmlException">The array size is greater than <see cref="F:System.Int32.MaxValue"/>.</exception>
    [__DynamicallyInvokable]
    public virtual byte[] ReadContentAsBinHex()
    {
      return this.ReadContentAsBinHex(this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the content and returns the BinHex decoded binary bytes.
    /// </summary>
    /// 
    /// <returns>
    /// A byte array that contains the BinHex decoded binary bytes.
    /// </returns>
    /// <param name="maxByteArrayContentLength">The maximum array length.</param><exception cref="T:System.Xml.XmlException">The array size is greater than <paramref name="maxByteArrayContentLength"/>.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected byte[] ReadContentAsBinHex(int maxByteArrayContentLength)
    {
      return this.ReadContentAsBytes(false, maxByteArrayContentLength);
    }

    private byte[] ReadContentAsBytes(bool base64, int maxByteArrayContentLength)
    {
      byte[][] numArray1 = new byte[32][];
      int length1 = 384;
      int num1 = 0;
      int length2 = 0;
      while (true)
      {
        byte[] buffer = new byte[length1];
        numArray1[num1++] = buffer;
        int index = 0;
        while (index < buffer.Length)
        {
          int num2 = !base64 ? base.ReadContentAsBinHex(buffer, index, buffer.Length - index) : base.ReadContentAsBase64(buffer, index, buffer.Length - index);
          if (num2 != 0)
            index += num2;
          else
            break;
        }
        if (length2 > maxByteArrayContentLength - index)
          XmlExceptionHelper.ThrowMaxArrayLengthExceeded(this, maxByteArrayContentLength);
        length2 += index;
        if (index >= buffer.Length)
          length1 *= 2;
        else
          break;
      }
      byte[] numArray2 = new byte[length2];
      int dstOffset = 0;
      for (int index = 0; index < num1 - 1; ++index)
      {
        Buffer.BlockCopy((Array) numArray1[index], 0, (Array) numArray2, dstOffset, numArray1[index].Length);
        dstOffset += numArray1[index].Length;
      }
      Buffer.BlockCopy((Array) numArray1[num1 - 1], 0, (Array) numArray2, dstOffset, length2 - dstOffset);
      return numArray2;
    }

    /// <summary>
    /// Tests whether the current node is a text node.
    /// </summary>
    /// 
    /// <returns>
    /// true if the node type is <see cref="F:System.Xml.XmlNodetype.Text"/>, <see cref="F:System.Xml.XmlNodetype.Whitespace"/>, <see cref="F:System.Xml.XmlNodetype.SignificantWhitespace"/>, <see cref="F:System.Xml.XmlNodetype.CDATA"/>, or <see cref="F:System.Xml.XmlNodetype.Attribute"/>; otherwise false.
    /// </returns>
    /// <param name="nodeType">Type of the node being tested.</param>
    [__DynamicallyInvokable]
    protected bool IsTextNode(XmlNodeType nodeType)
    {
      if (nodeType != XmlNodeType.Text && nodeType != XmlNodeType.Whitespace && (nodeType != XmlNodeType.SignificantWhitespace && nodeType != XmlNodeType.CDATA))
        return nodeType == XmlNodeType.Attribute;
      else
        return true;
    }

    /// <summary>
    /// Reads the content into a char array.
    /// </summary>
    /// 
    /// <returns>
    /// Number of characters read.
    /// </returns>
    /// <param name="chars">The array into which the characters are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of characters to put in the array.</param>
    [__DynamicallyInvokable]
    public virtual int ReadContentAsChars(char[] chars, int offset, int count)
    {
      int num = 0;
      do
      {
        XmlNodeType nodeType = this.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
          case XmlNodeType.EndElement:
            goto label_5;
          default:
            if (this.IsTextNode(nodeType))
            {
              num = this.ReadValueChunk(chars, offset, count);
              if (num > 0 || nodeType == XmlNodeType.Attribute || !this.Read())
                goto label_5;
              else
                continue;
            }
            else
              continue;
        }
      }
      while (this.Read());
label_5:
      return num;
    }

    /// <summary>
    /// Converts a node's content to a specified type.
    /// </summary>
    /// 
    /// <returns>
    /// The concatenated text content or attribute value converted to the requested type.
    /// </returns>
    /// <param name="type">The <see cref="T:System.Type"/> of the value to be returned.</param><param name="namespaceResolver">An <see cref="T:System.Xml.IXmlNamespaceResolver"/> object that is used to resolve any namespace prefixes related to type conversion. For example, this can be used when converting an <see cref="T:System.Xml.XmlQualifiedName"/> object to an xs:string. This value can be a null reference.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
    {
      if (type == typeof (Guid[]))
      {
        string[] strArray = (string[]) this.ReadContentAs(typeof (string[]), namespaceResolver);
        Guid[] guidArray = new Guid[strArray.Length];
        for (int index = 0; index < strArray.Length; ++index)
          guidArray[index] = XmlConverter.ToGuid(strArray[index]);
        return (object) guidArray;
      }
      else
      {
        if (!(type == typeof (UniqueId[])))
          return base.ReadContentAs(type, namespaceResolver);
        string[] strArray = (string[]) this.ReadContentAs(typeof (string[]), namespaceResolver);
        UniqueId[] uniqueIdArray = new UniqueId[strArray.Length];
        for (int index = 0; index < strArray.Length; ++index)
          uniqueIdArray[index] = XmlConverter.ToUniqueId(strArray[index]);
        return (object) uniqueIdArray;
      }
    }

    /// <summary>
    /// Converts a node's content to a string.
    /// </summary>
    /// 
    /// <returns>
    /// The node content in a string representation.
    /// </returns>
    /// <param name="strings">The array of strings to match content against.</param><param name="index">The index of the entry in <paramref name="strings"/> that matches the content.</param><exception cref="T:System.ArgumentNullException"><paramref name="strings"/> is null.</exception><exception cref="T:System.ArgumentNullException">An entry in<paramref name=" strings"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual string ReadContentAsString(string[] strings, out int index)
    {
      if (strings == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
      string str1 = base.ReadContentAsString();
      index = -1;
      for (int index1 = 0; index1 < strings.Length; ++index1)
      {
        string str2 = strings[index1];
        if (str2 == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "strings[{0}]", new object[1]
          {
            (object) index1
          }));
        else if (str2 == str1)
        {
          index = index1;
          return str2;
        }
      }
      return str1;
    }

    /// <summary>
    /// Converts a node's content to a string.
    /// </summary>
    /// 
    /// <returns>
    /// The node content in a string representation.
    /// </returns>
    /// <param name="strings">The array of <see cref="T:System.Xml.XmlDictionaryString"/> objects to match content against.</param><param name="index">The index of the entry in <paramref name="strings"/> that matches the content.</param><exception cref="T:System.ArgumentNullException"><paramref name="strings"/> is null.</exception><exception cref="T:System.ArgumentNullException">An entry in<paramref name=" strings"/> is null.</exception>
    [__DynamicallyInvokable]
    public virtual string ReadContentAsString(XmlDictionaryString[] strings, out int index)
    {
      if (strings == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("strings");
      string str = base.ReadContentAsString();
      index = -1;
      for (int index1 = 0; index1 < strings.Length; ++index1)
      {
        XmlDictionaryString dictionaryString = strings[index1];
        if (dictionaryString == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "strings[{0}]", new object[1]
          {
            (object) index1
          }));
        else if (dictionaryString.Value == str)
        {
          index = index1;
          return dictionaryString.Value;
        }
      }
      return str;
    }

    /// <summary>
    /// Converts a node's content to decimal.
    /// </summary>
    /// 
    /// <returns>
    /// The decimal representation of node's content.
    /// </returns>
    [__DynamicallyInvokable]
    public override Decimal ReadContentAsDecimal()
    {
      return XmlConverter.ToDecimal(base.ReadContentAsString());
    }

    /// <summary>
    /// Converts a node's content to float.
    /// </summary>
    /// 
    /// <returns>
    /// The float representation of node's content.
    /// </returns>
    [__DynamicallyInvokable]
    public override float ReadContentAsFloat()
    {
      return XmlConverter.ToSingle(base.ReadContentAsString());
    }

    /// <summary>
    /// Converts a node's content to a unique identifier.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a unique identifier.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual UniqueId ReadContentAsUniqueId()
    {
      return XmlConverter.ToUniqueId(base.ReadContentAsString());
    }

    /// <summary>
    /// Converts a node's content to guid.
    /// </summary>
    /// 
    /// <returns>
    /// The guid representation of node's content.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual Guid ReadContentAsGuid()
    {
      return XmlConverter.ToGuid(base.ReadContentAsString());
    }

    /// <summary>
    /// Converts a node's content to <see cref="T:System.TimeSpan"/>.
    /// </summary>
    /// 
    /// <returns>
    /// <see cref="T:System.TimeSpan"/> representation of node's content.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual TimeSpan ReadContentAsTimeSpan()
    {
      return XmlConverter.ToTimeSpan(base.ReadContentAsString());
    }

    /// <summary>
    /// Converts a node's content to a qualified name representation.
    /// </summary>
    /// <param name="localName">The <see cref="P:System.Xml.XmlReader.LocalName"/> part of the qualified name (out parameter).</param><param name="namespaceUri">The <see cref="P:System.Xml.XmlReader.NamespaceURI"/> part of the qualified name (out parameter).</param>
    [__DynamicallyInvokable]
    public virtual void ReadContentAsQualifiedName(out string localName, out string namespaceUri)
    {
      string prefix;
      XmlConverter.ToQualifiedName(base.ReadContentAsString(), out prefix, out localName);
      namespaceUri = this.LookupNamespace(prefix);
      if (namespaceUri != null)
        return;
      XmlExceptionHelper.ThrowUndefinedPrefix(this, prefix);
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.String"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.String"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public override string ReadElementContentAsString()
    {
      string str;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        str = string.Empty;
      }
      else
      {
        base.ReadStartElement();
        str = base.ReadContentAsString();
        this.ReadEndElement();
      }
      return str;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.Boolean"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Boolean"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public override bool ReadElementContentAsBoolean()
    {
      bool flag;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        flag = XmlConverter.ToBoolean(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        flag = this.ReadContentAsBoolean();
        this.ReadEndElement();
      }
      return flag;
    }

    /// <summary>
    /// Converts an element's content to an integer (<see cref="T:System.Int32"/>).
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as an integer (<see cref="T:System.Int32"/>).
    /// </returns>
    [__DynamicallyInvokable]
    public override int ReadElementContentAsInt()
    {
      int num;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        num = XmlConverter.ToInt32(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        num = this.ReadContentAsInt();
        this.ReadEndElement();
      }
      return num;
    }

    /// <summary>
    /// Converts an element's content to a long integer (<see cref="T:System.Int64"/>).
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a long integer (<see cref="T:System.Int64"/>).
    /// </returns>
    [__DynamicallyInvokable]
    public override long ReadElementContentAsLong()
    {
      long num;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        num = XmlConverter.ToInt64(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        num = this.ReadContentAsLong();
        this.ReadEndElement();
      }
      return num;
    }

    /// <summary>
    /// Converts an element's content to a floating point number (<see cref="T:System.Single"/>).
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a floating point number (<see cref="T:System.Single"/>).
    /// </returns>
    [__DynamicallyInvokable]
    public override float ReadElementContentAsFloat()
    {
      float num;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        num = XmlConverter.ToSingle(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        num = this.ReadContentAsFloat();
        this.ReadEndElement();
      }
      return num;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.Double"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Double"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public override double ReadElementContentAsDouble()
    {
      double num;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        num = XmlConverter.ToDouble(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        num = this.ReadContentAsDouble();
        this.ReadEndElement();
      }
      return num;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.Decimal"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Decimal"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public override Decimal ReadElementContentAsDecimal()
    {
      Decimal num;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        num = XmlConverter.ToDecimal(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        num = this.ReadContentAsDecimal();
        this.ReadEndElement();
      }
      return num;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.DateTime"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.DateTime"/>.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">The element is not in valid format.</exception><exception cref="T:System.FormatException">The element is not in valid format.</exception>
    public override DateTime ReadElementContentAsDateTime()
    {
      DateTime dateTime;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        try
        {
          dateTime = DateTime.Parse(string.Empty, (IFormatProvider) NumberFormatInfo.InvariantInfo);
        }
        catch (ArgumentException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", (Exception) ex));
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "DateTime", (Exception) ex));
        }
      }
      else
      {
        base.ReadStartElement();
        dateTime = this.ReadContentAsDateTime();
        this.ReadEndElement();
      }
      return dateTime;
    }

    /// <summary>
    /// Converts an element's content to a unique identifier.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a unique identifier.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">The element is not in valid format.</exception><exception cref="T:System.FormatException">The element is not in valid format.</exception>
    [__DynamicallyInvokable]
    public virtual UniqueId ReadElementContentAsUniqueId()
    {
      UniqueId uniqueId;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        try
        {
          uniqueId = new UniqueId(string.Empty);
        }
        catch (ArgumentException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", (Exception) ex));
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "UniqueId", (Exception) ex));
        }
      }
      else
      {
        base.ReadStartElement();
        uniqueId = this.ReadContentAsUniqueId();
        this.ReadEndElement();
      }
      return uniqueId;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.Guid"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Guid"/>.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">The element is not in valid format.</exception><exception cref="T:System.FormatException">The element is not in valid format.</exception>
    [__DynamicallyInvokable]
    public virtual Guid ReadElementContentAsGuid()
    {
      Guid guid;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        try
        {
          guid = Guid.Empty;
        }
        catch (ArgumentException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", (Exception) ex));
        }
        catch (FormatException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", (Exception) ex));
        }
        catch (OverflowException ex)
        {
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlExceptionHelper.CreateConversionException(string.Empty, "Guid", (Exception) ex));
        }
      }
      else
      {
        base.ReadStartElement();
        guid = this.ReadContentAsGuid();
        this.ReadEndElement();
      }
      return guid;
    }

    /// <summary>
    /// Converts an element's content to a <see cref="T:System.TimeSpan"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.TimeSpan"/>.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual TimeSpan ReadElementContentAsTimeSpan()
    {
      TimeSpan timeSpan;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        timeSpan = XmlConverter.ToTimeSpan(string.Empty);
      }
      else
      {
        base.ReadStartElement();
        timeSpan = this.ReadContentAsTimeSpan();
        this.ReadEndElement();
      }
      return timeSpan;
    }

    /// <summary>
    /// Converts a node's content to a array of Base64 bytes.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as an array of Base64 bytes.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual byte[] ReadElementContentAsBase64()
    {
      byte[] numArray;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        numArray = new byte[0];
      }
      else
      {
        base.ReadStartElement();
        numArray = this.ReadContentAsBase64();
        this.ReadEndElement();
      }
      return numArray;
    }

    /// <summary>
    /// Converts a node's content to an array of BinHex bytes.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as an array of BinHex bytes.
    /// </returns>
    [__DynamicallyInvokable]
    public virtual byte[] ReadElementContentAsBinHex()
    {
      byte[] numArray;
      if (base.IsStartElement() && this.IsEmptyElement)
      {
        this.Read();
        numArray = new byte[0];
      }
      else
      {
        base.ReadStartElement();
        numArray = this.ReadContentAsBinHex();
        this.ReadEndElement();
      }
      return numArray;
    }

    /// <summary>
    /// Gets non-atomized names.
    /// </summary>
    /// <param name="localName">The local name.</param><param name="namespaceUri">The namespace for the local <paramref name="localName"/>.</param>
    public virtual void GetNonAtomizedNames(out string localName, out string namespaceUri)
    {
      localName = this.LocalName;
      namespaceUri = this.NamespaceURI;
    }

    /// <summary>
    /// Not implemented in this class (it always returns false). May be overridden in derived classes.
    /// </summary>
    /// 
    /// <returns>
    /// false, unless overridden in a derived class.
    /// </returns>
    /// <param name="localName">Returns null, unless overridden in a derived class. .</param>
    [__DynamicallyInvokable]
    public virtual bool TryGetLocalNameAsDictionaryString(out XmlDictionaryString localName)
    {
      localName = (XmlDictionaryString) null;
      return false;
    }

    /// <summary>
    /// Not implemented in this class (it always returns false). May be overridden in derived classes.
    /// </summary>
    /// 
    /// <returns>
    /// false, unless overridden in a derived class.
    /// </returns>
    /// <param name="namespaceUri">Returns null, unless overridden in a derived class.</param>
    [__DynamicallyInvokable]
    public virtual bool TryGetNamespaceUriAsDictionaryString(out XmlDictionaryString namespaceUri)
    {
      namespaceUri = (XmlDictionaryString) null;
      return false;
    }

    /// <summary>
    /// Not implemented in this class (it always returns false). May be overridden in derived classes.
    /// </summary>
    /// 
    /// <returns>
    /// false, unless overridden in a derived class.
    /// </returns>
    /// <param name="value">Returns null, unless overridden in a derived class.</param>
    [__DynamicallyInvokable]
    public virtual bool TryGetValueAsDictionaryString(out XmlDictionaryString value)
    {
      value = (XmlDictionaryString) null;
      return false;
    }

    /// <summary>
    /// Checks whether the reader is positioned at the start of an array. This class returns false, but derived classes that have the concept of arrays might return true.
    /// </summary>
    /// 
    /// <returns>
    /// true if the reader is positioned at the start of an array node; otherwise false.
    /// </returns>
    /// <param name="type">Type of the node, if a valid node; otherwise null.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual bool IsStartArray(out Type type)
    {
      type = (Type) null;
      return false;
    }

    /// <summary>
    /// Not implemented in this class (it always returns false). May be overridden in derived classes.
    /// </summary>
    /// 
    /// <returns>
    /// false, unless overridden in a derived class.
    /// </returns>
    /// <param name="count">Returns 0, unless overridden in a derived class.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual bool TryGetArrayLength(out int count)
    {
      count = 0;
      return false;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Boolean"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.Boolean"/> array of the <see cref="T:System.Boolean"/> nodes.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual bool[] ReadBooleanArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, bool>) BooleanArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Boolean"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.Boolean"/> array of the <see cref="T:System.Boolean"/> nodes.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual bool[] ReadBooleanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, bool>) BooleanArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Boolean"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The local name of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, bool[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsBoolean();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Boolean"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of short integers (<see cref="T:System.Int16"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of short integers (<see cref="T:System.Int16"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual short[] ReadInt16Array(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, short>) Int16ArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of short integers (<see cref="T:System.Int16"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of short integers (<see cref="T:System.Int16"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual short[] ReadInt16Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, short>) Int16ArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of short integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, short[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num1;
      for (num1 = 0; num1 < count && base.IsStartElement(localName, namespaceUri); ++num1)
      {
        int num2 = this.ReadElementContentAsInt();
        if (num2 < (int) short.MinValue || num2 > (int) short.MaxValue)
          XmlExceptionHelper.ThrowConversionOverflow(this, num2.ToString((IFormatProvider) NumberFormatInfo.CurrentInfo), "Int16");
        array[offset + num1] = (short) num2;
      }
      return num1;
    }

    /// <summary>
    /// Reads repeated occurrences of short integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of integers (<see cref="T:System.Int32"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of integers (<see cref="T:System.Int32"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual int[] ReadInt32Array(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, int>) Int32ArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of integers (<see cref="T:System.Int32"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of integers (<see cref="T:System.Int32"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual int[] ReadInt32Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, int>) Int32ArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, int[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsInt();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of long integers (<see cref="T:System.Int64"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of long integers (<see cref="T:System.Int64"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual long[] ReadInt64Array(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, long>) Int64ArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of long integers (<see cref="T:System.Int64"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of long integers (<see cref="T:System.Int64"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual long[] ReadInt64Array(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, long>) Int64ArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of long integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, long[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsLong();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of long integers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of integers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the integers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of integers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of float numbers (<see cref="T:System.Single"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of float numbers (<see cref="T:System.Single"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual float[] ReadSingleArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, float>) SingleArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of float numbers (<see cref="T:System.Single"/>).
    /// </summary>
    /// 
    /// <returns>
    /// An array of float numbers (<see cref="T:System.Single"/>).
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual float[] ReadSingleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, float>) SingleArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of float numbers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The umber of float numbers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the float numbers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of float numbers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, float[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsFloat();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of float numbers into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of float numbers put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the float numbers are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of float numbers to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Double"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual double[] ReadDoubleArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, double>) DoubleArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Double"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual double[] ReadDoubleArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, double>) DoubleArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Double"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, double[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsDouble();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Double"/> nodes type into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Decimal"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual Decimal[] ReadDecimalArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, Decimal>) DecimalArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.Decimal"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual Decimal[] ReadDecimalArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, Decimal>) DecimalArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Decimal"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, Decimal[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsDecimal();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Decimal"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, Decimal[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.DateTime"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual DateTime[] ReadDateTimeArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, DateTime>) DateTimeArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Converts a node's content to a <see cref="T:System.DateTime"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// The node's content represented as a <see cref="T:System.DateTime"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual DateTime[] ReadDateTimeArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, DateTime>) DateTimeArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.DateTime"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, DateTime[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsDateTime();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.DateTime"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of <see cref="T:System.Guid"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An array of <see cref="T:System.Guid"/>.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual Guid[] ReadGuidArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, Guid>) GuidArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into an array of <see cref="T:System.Guid"/>.
    /// </summary>
    /// 
    /// <returns>
    /// An array of <see cref="T:System.Guid"/>.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual Guid[] ReadGuidArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, Guid>) GuidArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Guid"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, Guid[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsGuid();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.Guid"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into a <see cref="T:System.TimeSpan"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.TimeSpan"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual TimeSpan[] ReadTimeSpanArray(string localName, string namespaceUri)
    {
      return ((ArrayHelper<string, TimeSpan>) TimeSpanArrayHelperWithString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads the contents of a series of nodes with the given <paramref name="localName"/> and <paramref name="namespaceUri"/> into a <see cref="T:System.TimeSpan"/> array.
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.TimeSpan"/> array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param>
    [__DynamicallyInvokable]
    public virtual TimeSpan[] ReadTimeSpanArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
    {
      return ((ArrayHelper<XmlDictionaryString, TimeSpan>) TimeSpanArrayHelperWithDictionaryString.Instance).ReadArray(this, localName, namespaceUri, this.Quotas.MaxArrayLength);
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.TimeSpan"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
    {
      this.CheckArray((Array) array, offset, count);
      int num;
      for (num = 0; num < count && base.IsStartElement(localName, namespaceUri); ++num)
        array[offset + num] = this.ReadElementContentAsTimeSpan();
      return num;
    }

    /// <summary>
    /// Reads repeated occurrences of <see cref="T:System.TimeSpan"/> nodes into a typed array.
    /// </summary>
    /// 
    /// <returns>
    /// The number of nodes put in the array.
    /// </returns>
    /// <param name="localName">The local name of the element.</param><param name="namespaceUri">The namespace URI of the element.</param><param name="array">The array into which the nodes are put.</param><param name="offset">The starting index in the array.</param><param name="count">The number of nodes to put in the array.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> is &lt; 0 or &gt; <paramref name="array"/> length.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is &lt; 0 or &gt; <paramref name="array"/> length minus <paramref name="offset"/>.</exception>
    [__DynamicallyInvokable]
    public virtual int ReadArray(XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
    {
      return this.ReadArray(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
    }

    private void CheckArray(Array array, int offset, int count)
    {
      if (array == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException("array"));
      if (offset < 0)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("offset", System.Runtime.Serialization.SR.GetString("ValueMustBeNonNegative")));
      if (offset > array.Length)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("offset", System.Runtime.Serialization.SR.GetString("OffsetExceedsBufferSize", new object[1]
        {
          (object) array.Length
        })));
      }
      else
      {
        if (count < 0)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("count", System.Runtime.Serialization.SR.GetString("ValueMustBeNonNegative")));
        if (count <= array.Length - offset)
          return;
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("count", System.Runtime.Serialization.SR.GetString("SizeExceedsRemainingBufferSpace", new object[1]
        {
          (object) (array.Length - offset)
        })));
      }
    }

    private class XmlWrappedReader : XmlDictionaryReader, IXmlLineInfo
    {
      private XmlReader reader;
      private XmlNamespaceManager nsMgr;

      public override int AttributeCount
      {
        get
        {
          return this.reader.AttributeCount;
        }
      }

      public override string BaseURI
      {
        get
        {
          return this.reader.BaseURI;
        }
      }

      public override bool CanReadBinaryContent
      {
        get
        {
          return this.reader.CanReadBinaryContent;
        }
      }

      public override bool CanReadValueChunk
      {
        get
        {
          return this.reader.CanReadValueChunk;
        }
      }

      public override int Depth
      {
        get
        {
          return this.reader.Depth;
        }
      }

      public override bool EOF
      {
        get
        {
          return this.reader.EOF;
        }
      }

      public override bool HasValue
      {
        get
        {
          return this.reader.HasValue;
        }
      }

      public override bool IsDefault
      {
        get
        {
          return this.reader.IsDefault;
        }
      }

      public override bool IsEmptyElement
      {
        get
        {
          return this.reader.IsEmptyElement;
        }
      }

      public override string LocalName
      {
        get
        {
          return this.reader.LocalName;
        }
      }

      public override string Name
      {
        get
        {
          return this.reader.Name;
        }
      }

      public override string NamespaceURI
      {
        get
        {
          return this.reader.NamespaceURI;
        }
      }

      public override XmlNameTable NameTable
      {
        get
        {
          return this.reader.NameTable;
        }
      }

      public override XmlNodeType NodeType
      {
        get
        {
          return this.reader.NodeType;
        }
      }

      public override string Prefix
      {
        get
        {
          return this.reader.Prefix;
        }
      }

      public override char QuoteChar
      {
        get
        {
          return this.reader.QuoteChar;
        }
      }

      public override ReadState ReadState
      {
        get
        {
          return this.reader.ReadState;
        }
      }

      public override string this[int index]
      {
        get
        {
          return this.reader[index];
        }
      }

      public override string this[string name]
      {
        get
        {
          return this.reader[name];
        }
      }

      public override string this[string name, string namespaceUri]
      {
        get
        {
          return this.reader[name, namespaceUri];
        }
      }

      public override string Value
      {
        get
        {
          return this.reader.Value;
        }
      }

      public override string XmlLang
      {
        get
        {
          return this.reader.XmlLang;
        }
      }

      public override XmlSpace XmlSpace
      {
        get
        {
          return this.reader.XmlSpace;
        }
      }

      public override Type ValueType
      {
        get
        {
          return this.reader.ValueType;
        }
      }

      public int LineNumber
      {
        get
        {
          IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
          if (xmlLineInfo == null)
            return 1;
          else
            return xmlLineInfo.LineNumber;
        }
      }

      public int LinePosition
      {
        get
        {
          IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
          if (xmlLineInfo == null)
            return 1;
          else
            return xmlLineInfo.LinePosition;
        }
      }

      public XmlWrappedReader(XmlReader reader, XmlNamespaceManager nsMgr)
      {
        this.reader = reader;
        this.nsMgr = nsMgr;
      }

      public override void Close()
      {
        this.reader.Close();
        this.nsMgr = (XmlNamespaceManager) null;
      }

      public override string GetAttribute(int index)
      {
        return this.reader.GetAttribute(index);
      }

      public override string GetAttribute(string name)
      {
        return this.reader.GetAttribute(name);
      }

      public override string GetAttribute(string name, string namespaceUri)
      {
        return this.reader.GetAttribute(name, namespaceUri);
      }

      public override bool IsStartElement(string name)
      {
        return this.reader.IsStartElement(name);
      }

      public override bool IsStartElement(string localName, string namespaceUri)
      {
        return this.reader.IsStartElement(localName, namespaceUri);
      }

      public override string LookupNamespace(string namespaceUri)
      {
        return this.reader.LookupNamespace(namespaceUri);
      }

      public override void MoveToAttribute(int index)
      {
        this.reader.MoveToAttribute(index);
      }

      public override bool MoveToAttribute(string name)
      {
        return this.reader.MoveToAttribute(name);
      }

      public override bool MoveToAttribute(string name, string namespaceUri)
      {
        return this.reader.MoveToAttribute(name, namespaceUri);
      }

      public override bool MoveToElement()
      {
        return this.reader.MoveToElement();
      }

      public override bool MoveToFirstAttribute()
      {
        return this.reader.MoveToFirstAttribute();
      }

      public override bool MoveToNextAttribute()
      {
        return this.reader.MoveToNextAttribute();
      }

      public override bool Read()
      {
        return this.reader.Read();
      }

      public override bool ReadAttributeValue()
      {
        return this.reader.ReadAttributeValue();
      }

      public override string ReadElementString(string name)
      {
        return this.reader.ReadElementString(name);
      }

      public override string ReadElementString(string localName, string namespaceUri)
      {
        return this.reader.ReadElementString(localName, namespaceUri);
      }

      public override string ReadInnerXml()
      {
        return this.reader.ReadInnerXml();
      }

      public override string ReadOuterXml()
      {
        return this.reader.ReadOuterXml();
      }

      public override void ReadStartElement(string name)
      {
        this.reader.ReadStartElement(name);
      }

      public override void ReadStartElement(string localName, string namespaceUri)
      {
        this.reader.ReadStartElement(localName, namespaceUri);
      }

      public override void ReadEndElement()
      {
        this.reader.ReadEndElement();
      }

      public override string ReadString()
      {
        return this.reader.ReadString();
      }

      public override void ResolveEntity()
      {
        this.reader.ResolveEntity();
      }

      public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
      {
        return this.reader.ReadElementContentAsBase64(buffer, offset, count);
      }

      public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
      {
        return this.reader.ReadContentAsBase64(buffer, offset, count);
      }

      public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
      {
        return this.reader.ReadElementContentAsBinHex(buffer, offset, count);
      }

      public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
      {
        return this.reader.ReadContentAsBinHex(buffer, offset, count);
      }

      public override int ReadValueChunk(char[] chars, int offset, int count)
      {
        return this.reader.ReadValueChunk(chars, offset, count);
      }

      public override bool ReadContentAsBoolean()
      {
        return this.reader.ReadContentAsBoolean();
      }

      public override DateTime ReadContentAsDateTime()
      {
        return this.reader.ReadContentAsDateTime();
      }

      public override Decimal ReadContentAsDecimal()
      {
        return (Decimal) this.reader.ReadContentAs(typeof (Decimal), (IXmlNamespaceResolver) null);
      }

      public override double ReadContentAsDouble()
      {
        return this.reader.ReadContentAsDouble();
      }

      public override int ReadContentAsInt()
      {
        return this.reader.ReadContentAsInt();
      }

      public override long ReadContentAsLong()
      {
        return this.reader.ReadContentAsLong();
      }

      public override float ReadContentAsFloat()
      {
        return this.reader.ReadContentAsFloat();
      }

      public override string ReadContentAsString()
      {
        return this.reader.ReadContentAsString();
      }

      public override object ReadContentAs(Type type, IXmlNamespaceResolver namespaceResolver)
      {
        return this.reader.ReadContentAs(type, namespaceResolver);
      }

      public bool HasLineInfo()
      {
        IXmlLineInfo xmlLineInfo = this.reader as IXmlLineInfo;
        if (xmlLineInfo == null)
          return false;
        else
          return xmlLineInfo.HasLineInfo();
      }
    }
  }
}
