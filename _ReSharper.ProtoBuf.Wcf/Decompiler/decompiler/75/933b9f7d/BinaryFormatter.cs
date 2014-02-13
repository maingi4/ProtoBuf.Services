// Type: System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
  /// <summary>
  /// Serializes and deserializes an object, or an entire graph of connected objects, in binary format.
  /// </summary>
  [ComVisible(true)]
  public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
  {
    private static Dictionary<Type, TypeInformation> typeNameCache = new Dictionary<Type, TypeInformation>();
    internal FormatterTypeStyle m_typeFormat = FormatterTypeStyle.TypesAlways;
    internal TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;
    internal ISurrogateSelector m_surrogates;
    internal StreamingContext m_context;
    internal SerializationBinder m_binder;
    internal FormatterAssemblyStyle m_assemblyFormat;
    internal object[] m_crossAppDomainArray;

    /// <summary>
    /// Gets or sets the format in which type descriptions are laid out in the serialized stream.
    /// </summary>
    /// 
    /// <returns>
    /// The style of type layouts to use.
    /// </returns>
    public FormatterTypeStyle TypeFormat
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_typeFormat;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_typeFormat = value;
      }
    }

    /// <summary>
    /// Gets or sets the behavior of the deserializer with regards to finding and loading assemblies.
    /// </summary>
    /// 
    /// <returns>
    /// One of the <see cref="T:System.Runtime.Serialization.Formatters.FormatterAssemblyStyle"/> values that specifies the deserializer behavior.
    /// </returns>
    public FormatterAssemblyStyle AssemblyFormat
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_assemblyFormat;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_assemblyFormat = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel"/> of automatic deserialization the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> performs.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel"/> that represents the current automatic deserialization level.
    /// </returns>
    public TypeFilterLevel FilterLevel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_securityLevel;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_securityLevel = value;
      }
    }

    /// <summary>
    /// Gets or sets a <see cref="T:System.Runtime.Serialization.ISurrogateSelector"/> that controls type substitution during serialization and deserialization.
    /// </summary>
    /// 
    /// <returns>
    /// The surrogate selector to use with this formatter.
    /// </returns>
    public ISurrogateSelector SurrogateSelector
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_surrogates;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_surrogates = value;
      }
    }

    /// <summary>
    /// Gets or sets an object of type <see cref="T:System.Runtime.Serialization.SerializationBinder"/> that controls the binding of a serialized object to a type.
    /// </summary>
    /// 
    /// <returns>
    /// The serialization binder to use with this formatter.
    /// </returns>
    public SerializationBinder Binder
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_binder;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_binder = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext"/> for this formatter.
    /// </summary>
    /// 
    /// <returns>
    /// The streaming context to use with this formatter.
    /// </returns>
    public StreamingContext Context
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_context;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_context = value;
      }
    }

    static BinaryFormatter()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> class with default values.
    /// </summary>
    public BinaryFormatter()
    {
      this.m_surrogates = (ISurrogateSelector) null;
      this.m_context = new StreamingContext(StreamingContextStates.All);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> class with a given surrogate selector and streaming context.
    /// </summary>
    /// <param name="selector">The <see cref="T:System.Runtime.Serialization.ISurrogateSelector"/> to use. Can be null. </param><param name="context">The source and destination for the serialized data. </param>
    public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
    {
      this.m_surrogates = selector;
      this.m_context = context;
    }

    /// <summary>
    /// Deserializes the specified stream into an object graph.
    /// </summary>
    /// 
    /// <returns>
    /// The top (root) of the object graph.
    /// </returns>
    /// <param name="serializationStream">The stream from which to deserialize the object graph. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream"/> supports seeking, but its length is 0. -or-The target type is a <see cref="T:System.Decimal"/>, but the value is out of range of the <see cref="T:System.Decimal"/> type.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object Deserialize(Stream serializationStream)
    {
      return this.Deserialize(serializationStream, (HeaderHandler) null);
    }

    [SecurityCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck)
    {
      return this.Deserialize(serializationStream, handler, fCheck, (IMethodCallMessage) null);
    }

    /// <summary>
    /// Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> handles any headers in that stream.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object or the top object (root) of the object graph.
    /// </returns>
    /// <param name="serializationStream">The stream from which to deserialize the object graph. </param><param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> that handles any headers in the <paramref name="serializationStream"/>. Can be null. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream"/> supports seeking, but its length is 0. -or-The target type is a <see cref="T:System.Decimal"/>, but the value is out of range of the <see cref="T:System.Decimal"/> type.</exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object Deserialize(Stream serializationStream, HeaderHandler handler)
    {
      return this.Deserialize(serializationStream, handler, true);
    }

    /// <summary>
    /// Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized response to the remote method call.
    /// </returns>
    /// <param name="serializationStream">The stream from which to deserialize the object graph. </param><param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> that handles any headers in the <paramref name="serializationStream"/>. Can be null. </param><param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage"/> that contains details about where the call came from. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream"/> supports seeking, but its length is 0. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence"/></PermissionSet>
    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
    {
      return this.Deserialize(serializationStream, handler, true, methodCallMessage);
    }

    /// <summary>
    /// Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> handles any headers in that stream.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized object or the top object (root) of the object graph.
    /// </returns>
    /// <param name="serializationStream">The stream from which to deserialize the object graph. </param><param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> that handles any headers in the <paramref name="serializationStream"/>. Can be null. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream"/> supports seeking, but its length is 0. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, SerializationFormatter"/></PermissionSet>
    [SecurityCritical]
    [ComVisible(false)]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
    {
      return this.Deserialize(serializationStream, handler, false);
    }

    /// <summary>
    /// Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream"/>.
    /// </summary>
    /// 
    /// <returns>
    /// The deserialized response to the remote method call.
    /// </returns>
    /// <param name="serializationStream">The stream from which to deserialize the object graph. </param><param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler"/> that handles any headers in the <paramref name="serializationStream"/>. Can be null. </param><param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage"/> that contains details about where the call came from. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream"/> supports seeking, but its length is 0. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, SerializationFormatter"/></PermissionSet>
    [SecurityCritical]
    [ComVisible(false)]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
    {
      return this.Deserialize(serializationStream, handler, false, methodCallMessage);
    }

    [SecurityCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, IMethodCallMessage methodCallMessage)
    {
      return this.Deserialize(serializationStream, handler, fCheck, false, methodCallMessage);
    }

    [SecurityCritical]
    internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
    {
      if (serializationStream == null)
      {
        throw new ArgumentNullException("serializationStream", Environment.GetResourceString("ArgumentNull_WithParamName", new object[1]
        {
          (object) serializationStream
        }));
      }
      else
      {
        if (serializationStream.CanSeek && serializationStream.Length == 0L)
          throw new SerializationException(Environment.GetResourceString("Serialization_Stream"));
        ObjectReader objectReader = new ObjectReader(serializationStream, this.m_surrogates, this.m_context, new InternalFE()
        {
          FEtypeFormat = this.m_typeFormat,
          FEserializerTypeEnum = InternalSerializerTypeE.Binary,
          FEassemblyFormat = this.m_assemblyFormat,
          FEsecurityLevel = this.m_securityLevel
        }, this.m_binder);
        objectReader.crossAppDomainArray = this.m_crossAppDomainArray;
        return objectReader.Deserialize(handler, new __BinaryParser(serializationStream, objectReader), fCheck, isCrossAppDomain, methodCallMessage);
      }
    }

    /// <summary>
    /// Serializes the object, or graph of objects with the specified top (root), to the given stream.
    /// </summary>
    /// <param name="serializationStream">The stream to which the graph is to be serialized. </param><param name="graph">The object at the root of the graph to serialize. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. -or-The <paramref name="graph"/> is null.</exception><exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph"/> parameter is not marked as serializable. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/></PermissionSet>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Serialize(Stream serializationStream, object graph)
    {
      this.Serialize(serializationStream, graph, (Header[]) null);
    }

    /// <summary>
    /// Serializes the object, or graph of objects with the specified top (root), to the given stream attaching the provided headers.
    /// </summary>
    /// <param name="serializationStream">The stream to which the object is to be serialized. </param><param name="graph">The object at the root of the graph to serialize. </param><param name="headers">Remoting headers to include in the serialization. Can be null. </param><exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream"/> is null. </exception><exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph"/> parameter is not marked as serializable. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><PermissionSet><IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess"/></PermissionSet>
    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Serialize(Stream serializationStream, object graph, Header[] headers)
    {
      this.Serialize(serializationStream, graph, headers, true);
    }

    [SecurityCritical]
    internal void Serialize(Stream serializationStream, object graph, Header[] headers, bool fCheck)
    {
      if (serializationStream == null)
      {
        throw new ArgumentNullException("serializationStream", Environment.GetResourceString("ArgumentNull_WithParamName", new object[1]
        {
          (object) serializationStream
        }));
      }
      else
      {
        ObjectWriter objectWriter = new ObjectWriter(this.m_surrogates, this.m_context, new InternalFE()
        {
          FEtypeFormat = this.m_typeFormat,
          FEserializerTypeEnum = InternalSerializerTypeE.Binary,
          FEassemblyFormat = this.m_assemblyFormat
        }, this.m_binder);
        __BinaryWriter serWriter = new __BinaryWriter(serializationStream, objectWriter, this.m_typeFormat);
        objectWriter.Serialize(graph, headers, serWriter, fCheck);
        this.m_crossAppDomainArray = objectWriter.crossAppDomainArray;
      }
    }

    internal static TypeInformation GetTypeInformation(Type type)
    {
      lock (BinaryFormatter.typeNameCache)
      {
        TypeInformation local_0 = (TypeInformation) null;
        if (!BinaryFormatter.typeNameCache.TryGetValue(type, out local_0))
        {
          bool local_1;
          string local_2 = FormatterServices.GetClrAssemblyName(type, out local_1);
          local_0 = new TypeInformation(FormatterServices.GetClrTypeFullName(type), local_2, local_1);
          BinaryFormatter.typeNameCache.Add(type, local_0);
        }
        return local_0;
      }
    }
  }
}
