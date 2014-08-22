// Type: System.Runtime.Serialization.XmlObjectSerializer
// Assembly: System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Runtime.Serialization.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
  /// <summary>
  /// Provides the base class used to serialize objects as XML streams or documents. This class is abstract.
  /// </summary>
  /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized. </exception><filterpriority>2</filterpriority>
  [__DynamicallyInvokable]
  public abstract class XmlObjectSerializer
  {
    [SecurityCritical]
    private static IFormatterConverter formatterConverter;

    internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
    {
      get
      {
        return (Dictionary<XmlQualifiedName, DataContract>) null;
      }
    }

    internal static IFormatterConverter FormatterConverter
    {
      [SecuritySafeCritical] get
      {
        if (XmlObjectSerializer.formatterConverter == null)
          XmlObjectSerializer.formatterConverter = (IFormatterConverter) new FormatterConverter();
        return XmlObjectSerializer.formatterConverter;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected XmlObjectSerializer()
    {
    }

    /// <summary>
    /// Writes the start of the object's data as an opening XML element using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document.</param><param name="graph">The object to serialize.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public abstract void WriteStartObject(XmlDictionaryWriter writer, object graph);

    /// <summary>
    /// Writes only the content of the object to the XML document or stream using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param><param name="graph">The object that contains the content to write.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public abstract void WriteObjectContent(XmlDictionaryWriter writer, object graph);

    /// <summary>
    /// Writes the end of the object data as a closing XML element to the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public abstract void WriteEndObject(XmlDictionaryWriter writer);

    /// <summary>
    /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.IO.Stream"/>.
    /// </summary>
    /// <param name="stream">A <see cref="T:System.IO.Stream"/> used to write the XML document or stream.</param><param name="graph">The object that contains the data to write to the stream.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
    [__DynamicallyInvokable]
    public virtual void WriteObject(Stream stream, object graph)
    {
      XmlObjectSerializer.CheckNull((object) stream, "stream");
      XmlDictionaryWriter textWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, false);
      this.WriteObject(textWriter, graph);
      textWriter.Flush();
    }

    /// <summary>
    /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param><param name="graph">The object that contains the content to write.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
    [__DynamicallyInvokable]
    public virtual void WriteObject(XmlWriter writer, object graph)
    {
      XmlObjectSerializer.CheckNull((object) writer, "writer");
      this.WriteObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
    }

    /// <summary>
    /// Writes the start of the object's data as an opening XML element using the specified <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document.</param><param name="graph">The object to serialize.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual void WriteStartObject(XmlWriter writer, object graph)
    {
      XmlObjectSerializer.CheckNull((object) writer, "writer");
      this.WriteStartObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
    }

    /// <summary>
    /// Writes only the content of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param><param name="graph">The object that contains the content to write.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual void WriteObjectContent(XmlWriter writer, object graph)
    {
      XmlObjectSerializer.CheckNull((object) writer, "writer");
      this.WriteObjectContent(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
    }

    /// <summary>
    /// Writes the end of the object data as a closing XML element to the XML document or stream with an <see cref="T:System.Xml.XmlWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual void WriteEndObject(XmlWriter writer)
    {
      XmlObjectSerializer.CheckNull((object) writer, "writer");
      this.WriteEndObject(XmlDictionaryWriter.CreateDictionaryWriter(writer));
    }

    /// <summary>
    /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
    /// </summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the content to the XML document or stream.</param><param name="graph">The object that contains the content to write.</param><exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception><exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
    [__DynamicallyInvokable]
    public virtual void WriteObject(XmlDictionaryWriter writer, object graph)
    {
      this.WriteObjectHandleExceptions(new XmlWriterDelegator((XmlWriter) writer), graph);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph)
    {
      this.WriteObjectHandleExceptions(writer, graph, (DataContractResolver) null);
    }

    internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) writer, "writer");
        if (DiagnosticUtility.ShouldTraceInformation)
        {
          TraceUtility.Trace(TraceEventType.Information, 196609, System.Runtime.Serialization.SR.GetString("TraceCodeWriteObjectBegin"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
          this.InternalWriteObject(writer, graph, dataContractResolver);
          TraceUtility.Trace(TraceEventType.Information, 196610, System.Runtime.Serialization.SR.GetString("TraceCodeWriteObjectEnd"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
        }
        else
          this.InternalWriteObject(writer, graph, dataContractResolver);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorSerializing", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorSerializing", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
    }

    internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph)
    {
      this.WriteStartObject(writer.Writer, graph);
      this.WriteObjectContent(writer.Writer, graph);
      this.WriteEndObject(writer.Writer);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
    {
      this.InternalWriteObject(writer, graph);
    }

    internal virtual void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    internal virtual void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    internal virtual void InternalWriteEndObject(XmlWriterDelegator writer)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    internal void WriteStartObjectHandleExceptions(XmlWriterDelegator writer, object graph)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) writer, "writer");
        this.InternalWriteStartObject(writer, graph);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorWriteStartObject", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorWriteStartObject", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
    }

    internal void WriteObjectContentHandleExceptions(XmlWriterDelegator writer, object graph)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) writer, "writer");
        if (DiagnosticUtility.ShouldTraceInformation)
        {
          TraceUtility.Trace(TraceEventType.Information, 196611, System.Runtime.Serialization.SR.GetString("TraceCodeWriteObjectContentBegin"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
          if (writer.WriteState != WriteState.Element)
          {
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(System.Runtime.Serialization.SR.GetString("XmlWriterMustBeInElement", new object[1]
            {
              (object) writer.WriteState
            })));
          }
          else
          {
            this.InternalWriteObjectContent(writer, graph);
            TraceUtility.Trace(TraceEventType.Information, 196612, System.Runtime.Serialization.SR.GetString("TraceCodeWriteObjectContentEnd"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetSerializeType(graph))));
          }
        }
        else if (writer.WriteState != WriteState.Element)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(System.Runtime.Serialization.SR.GetString("XmlWriterMustBeInElement", new object[1]
          {
            (object) writer.WriteState
          })));
        else
          this.InternalWriteObjectContent(writer, graph);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorSerializing", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorSerializing", this.GetSerializeType(graph), (Exception) ex), (Exception) ex));
      }
    }

    internal void WriteEndObjectHandleExceptions(XmlWriterDelegator writer)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) writer, "writer");
        this.InternalWriteEndObject(writer);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorWriteEndObject", (Type) null, (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorWriteEndObject", (Type) null, (Exception) ex), (Exception) ex));
      }
    }

    internal void WriteRootElement(XmlWriterDelegator writer, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns, bool needsContractNsAtRoot)
    {
      if (name == null)
      {
        if (!contract.HasRoot)
          return;
        contract.WriteRootElement(writer, contract.TopLevelElementName, contract.TopLevelElementNamespace);
      }
      else
      {
        contract.WriteRootElement(writer, name, ns);
        if (!needsContractNsAtRoot)
          return;
        writer.WriteNamespaceDecl(contract.Namespace);
      }
    }

    internal bool CheckIfNeedsContractNsAtRoot(XmlDictionaryString name, XmlDictionaryString ns, DataContract contract)
    {
      if (name == null || contract.IsBuiltInDataContract || (!contract.CanContainReferences || contract.IsISerializable))
        return false;
      string @string = XmlDictionaryString.GetString(contract.Namespace);
      return !string.IsNullOrEmpty(@string) && !(@string == XmlDictionaryString.GetString(ns));
    }

    internal static void WriteNull(XmlWriterDelegator writer)
    {
      writer.WriteAttributeBool("i", DictionaryGlobals.XsiNilLocalName, DictionaryGlobals.SchemaInstanceNamespace, true);
    }

    internal static bool IsContractDeclared(DataContract contract, DataContract declaredContract)
    {
      if (object.ReferenceEquals((object) contract.Name, (object) declaredContract.Name) && object.ReferenceEquals((object) contract.Namespace, (object) declaredContract.Namespace))
        return true;
      if (contract.Name.Value == declaredContract.Name.Value)
        return contract.Namespace.Value == declaredContract.Namespace.Value;
      else
        return false;
    }

    /// <summary>
    /// Reads the XML stream or document with a <see cref="T:System.IO.Stream"/> and returns the deserialized object.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object.
    /// </returns>
    /// <param name="stream">A <see cref="T:System.IO.Stream"/> used to read the XML stream or document.</param>
    [__DynamicallyInvokable]
    public virtual object ReadObject(Stream stream)
    {
      XmlObjectSerializer.CheckNull((object) stream, "stream");
      return this.ReadObject(XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max));
    }

    /// <summary>
    /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML stream or document.</param>
    [__DynamicallyInvokable]
    public virtual object ReadObject(XmlReader reader)
    {
      XmlObjectSerializer.CheckNull((object) reader, "reader");
      return this.ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader));
    }

    /// <summary>
    /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
    [__DynamicallyInvokable]
    public virtual object ReadObject(XmlDictionaryReader reader)
    {
      return this.ReadObjectHandleExceptions(new XmlReaderDelegator((XmlReader) reader), true);
    }

    /// <summary>
    /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML document or stream.</param><param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; false to skip the verification.</param>
    [__DynamicallyInvokable]
    public virtual object ReadObject(XmlReader reader, bool verifyObjectName)
    {
      XmlObjectSerializer.CheckNull((object) reader, "reader");
      return this.ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader), verifyObjectName);
    }

    /// <summary>
    /// Reads the XML stream or document with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param><param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; otherwise, false to skip the verification.</param>
    [__DynamicallyInvokable]
    public abstract object ReadObject(XmlDictionaryReader reader, bool verifyObjectName);

    /// <summary>
    /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlReader"/> is positioned over an XML element that can be read.
    /// </summary>
    /// 
    /// <returns>
    /// true if the reader is positioned over the starting element; otherwise, false.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML stream or document.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public virtual bool IsStartObject(XmlReader reader)
    {
      XmlObjectSerializer.CheckNull((object) reader, "reader");
      return this.IsStartObject(XmlDictionaryReader.CreateDictionaryReader(reader));
    }

    /// <summary>
    /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlDictionaryReader"/> is positioned over an XML element that can be read.
    /// </summary>
    /// 
    /// <returns>
    /// true if the reader can read the data; otherwise, false.
    /// </returns>
    /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML stream or document.</param><filterpriority>2</filterpriority>
    [__DynamicallyInvokable]
    public abstract bool IsStartObject(XmlDictionaryReader reader);

    internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName)
    {
      return this.ReadObject(reader.UnderlyingReader, verifyObjectName);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
    {
      return this.InternalReadObject(reader, verifyObjectName);
    }

    internal virtual bool InternalIsStartObject(XmlReaderDelegator reader)
    {
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new NotSupportedException());
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName)
    {
      return this.ReadObjectHandleExceptions(reader, verifyObjectName, (DataContractResolver) null);
    }

    internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) reader, "reader");
        if (!DiagnosticUtility.ShouldTraceInformation)
          return this.InternalReadObject(reader, verifyObjectName, dataContractResolver);
        TraceUtility.Trace(TraceEventType.Information, 196613, System.Runtime.Serialization.SR.GetString("TraceCodeReadObjectBegin"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetDeserializeType())));
        object obj = this.InternalReadObject(reader, verifyObjectName, dataContractResolver);
        TraceUtility.Trace(TraceEventType.Information, 196614, System.Runtime.Serialization.SR.GetString("TraceCodeReadObjectEnd"), (TraceRecord) new StringTraceRecord("Type", XmlObjectSerializer.GetTypeInfo(this.GetDeserializeType())));
        return obj;
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorDeserializing", this.GetDeserializeType(), (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorDeserializing", this.GetDeserializeType(), (Exception) ex), (Exception) ex));
      }
    }

    internal bool IsStartObjectHandleExceptions(XmlReaderDelegator reader)
    {
      try
      {
        XmlObjectSerializer.CheckNull((object) reader, "reader");
        return this.InternalIsStartObject(reader);
      }
      catch (XmlException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorIsStartObject", this.GetDeserializeType(), (Exception) ex), (Exception) ex));
      }
      catch (FormatException ex)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.GetTypeInfoError("ErrorIsStartObject", this.GetDeserializeType(), (Exception) ex), (Exception) ex));
      }
    }

    internal bool IsRootXmlAny(XmlDictionaryString rootName, DataContract contract)
    {
      if (rootName == null)
        return !contract.HasRoot;
      else
        return false;
    }

    internal bool IsStartElement(XmlReaderDelegator reader)
    {
      if (!reader.MoveToElement())
        return reader.IsStartElement();
      else
        return true;
    }

    internal bool IsRootElement(XmlReaderDelegator reader, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns)
    {
      reader.MoveToElement();
      if (name != null)
        return reader.IsStartElement(name, ns);
      if (!contract.HasRoot)
        return reader.IsStartElement();
      if (reader.IsStartElement(contract.TopLevelElementName, contract.TopLevelElementNamespace))
        return true;
      ClassDataContract classDataContract = contract as ClassDataContract;
      if (classDataContract != null)
        classDataContract = classDataContract.BaseContract;
      for (; classDataContract != null; classDataContract = classDataContract.BaseContract)
      {
        if (reader.IsStartElement(classDataContract.TopLevelElementName, classDataContract.TopLevelElementNamespace))
          return true;
      }
      if (classDataContract == null)
      {
        DataContract dataContract = (DataContract) PrimitiveDataContract.GetPrimitiveDataContract(Globals.TypeOfObject);
        if (reader.IsStartElement(dataContract.TopLevelElementName, dataContract.TopLevelElementNamespace))
          return true;
      }
      return false;
    }

    internal static void CheckNull(object obj, string name)
    {
      if (obj == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentNullException(name));
    }

    internal static string TryAddLineInfo(XmlReaderDelegator reader, string errorMessage)
    {
      if (!reader.HasLineInfo())
        return errorMessage;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", new object[2]
      {
        (object) System.Runtime.Serialization.SR.GetString("ErrorInLine", (object) reader.LineNumber, (object) reader.LinePosition),
        (object) errorMessage
      });
    }

    internal static Exception CreateSerializationExceptionWithReaderDetails(string errorMessage, XmlReaderDelegator reader)
    {
      return (Exception) XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(reader, System.Runtime.Serialization.SR.GetString("EncounteredWithNameNamespace", (object) errorMessage, (object) reader.NodeType, (object) reader.LocalName, (object) reader.NamespaceURI)));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal static SerializationException CreateSerializationException(string errorMessage)
    {
      return XmlObjectSerializer.CreateSerializationException(errorMessage, (Exception) null);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static SerializationException CreateSerializationException(string errorMessage, Exception innerException)
    {
      return new SerializationException(errorMessage, innerException);
    }

    internal virtual Type GetSerializeType(object graph)
    {
      if (graph != null)
        return graph.GetType();
      else
        return (Type) null;
    }

    internal virtual Type GetDeserializeType()
    {
      return (Type) null;
    }

    private static string GetTypeInfo(Type type)
    {
      if (!(type == (Type) null))
        return DataContract.GetClrTypeFullName(type);
      else
        return string.Empty;
    }

    private static string GetTypeInfoError(string errorMessage, Type type, Exception innerException)
    {
      string str1;
      if (!(type == (Type) null))
        str1 = System.Runtime.Serialization.SR.GetString("ErrorTypeInfo", new object[1]
        {
          (object) DataContract.GetClrTypeFullName(type)
        });
      else
        str1 = string.Empty;
      string str2 = str1;
      string str3 = innerException == null ? string.Empty : innerException.Message;
      return System.Runtime.Serialization.SR.GetString(errorMessage, (object) str2, (object) str3);
    }
  }
}
