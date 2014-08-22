// Type: ProtoBuf.ServiceModel.ProtoBehaviorExtension
// Assembly: protobuf-net, Version=2.0.0.668, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// Assembly location: D:\Research\ProtoBuf.Wcf\packages\protobuf-net.2.0.0.668\lib\net40\protobuf-net.dll

using System;
using System.ServiceModel.Configuration;

namespace ProtoBuf.ServiceModel
{
  /// <summary>
  /// Configuration element to swap out DatatContractSerilaizer with the XmlProtoSerializer for a given endpoint.
  /// 
  /// </summary>
  /// <seealso cref="T:ProtoBuf.ServiceModel.ProtoEndpointBehavior"/>
  public class ProtoBehaviorExtension : BehaviorExtensionElement
  {
    /// <summary>
    /// Gets the type of behavior.
    /// 
    /// </summary>
    public override Type BehaviorType
    {
      get
      {
        return typeof (ProtoEndpointBehavior);
      }
    }

    /// <summary>
    /// Creates a behavior extension based on the current configuration settings.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The behavior extension.
    /// </returns>
    protected override object CreateBehavior()
    {
      return (object) new ProtoEndpointBehavior();
    }
  }
}
