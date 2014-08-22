// Type: System.ServiceModel.Dispatcher.ProxyOperationRuntime
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics.Application;

namespace System.ServiceModel.Dispatcher
{
  internal class ProxyOperationRuntime
  {
    internal static readonly ParameterInfo[] NoParams = new ParameterInfo[0];
    internal static readonly object[] EmptyArray = new object[0];
    private readonly IClientMessageFormatter formatter;
    private readonly bool isInitiating;
    private readonly bool isOneWay;
    private readonly bool isTerminating;
    private readonly bool isSessionOpenNotificationEnabled;
    private readonly string name;
    private readonly IParameterInspector[] parameterInspectors;
    private readonly IClientFaultFormatter faultFormatter;
    private readonly ImmutableClientRuntime parent;
    private bool serializeRequest;
    private bool deserializeReply;
    private string action;
    private string replyAction;
    private MethodInfo beginMethod;
    private MethodInfo syncMethod;
    private MethodInfo taskMethod;
    private ParameterInfo[] inParams;
    private ParameterInfo[] outParams;
    private ParameterInfo[] endOutParams;
    private ParameterInfo returnParam;

    internal string Action
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.action;
      }
    }

    internal IClientFaultFormatter FaultFormatter
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.faultFormatter;
      }
    }

    internal bool IsInitiating
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isInitiating;
      }
    }

    internal bool IsOneWay
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isOneWay;
      }
    }

    internal bool IsTerminating
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isTerminating;
      }
    }

    internal bool IsSessionOpenNotificationEnabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isSessionOpenNotificationEnabled;
      }
    }

    internal string Name
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.name;
      }
    }

    internal ImmutableClientRuntime Parent
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.parent;
      }
    }

    internal string ReplyAction
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.replyAction;
      }
    }

    internal bool DeserializeReply
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.deserializeReply;
      }
    }

    internal bool SerializeRequest
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.serializeRequest;
      }
    }

    internal System.Type TaskTResult { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

    static ProxyOperationRuntime()
    {
    }

    internal ProxyOperationRuntime(ClientOperation operation, ImmutableClientRuntime parent)
    {
      if (operation == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("operation");
      if (parent == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("parent");
      this.parent = parent;
      this.formatter = operation.Formatter;
      this.isInitiating = operation.IsInitiating;
      this.isOneWay = operation.IsOneWay;
      this.isTerminating = operation.IsTerminating;
      this.isSessionOpenNotificationEnabled = operation.IsSessionOpenNotificationEnabled;
      this.name = operation.Name;
      this.parameterInspectors = EmptyArray<IParameterInspector>.ToArray(operation.ParameterInspectors);
      this.faultFormatter = operation.FaultFormatter;
      this.serializeRequest = operation.SerializeRequest;
      this.deserializeReply = operation.DeserializeReply;
      this.action = operation.Action;
      this.replyAction = operation.ReplyAction;
      this.beginMethod = operation.BeginMethod;
      this.syncMethod = operation.SyncMethod;
      this.taskMethod = operation.TaskMethod;
      this.TaskTResult = operation.TaskTResult;
      if (this.beginMethod != (MethodInfo) null)
      {
        this.inParams = ServiceReflector.GetInputParameters(this.beginMethod, true);
        this.outParams = !(this.syncMethod != (MethodInfo) null) ? ProxyOperationRuntime.NoParams : ServiceReflector.GetOutputParameters(this.syncMethod, false);
        this.endOutParams = ServiceReflector.GetOutputParameters(operation.EndMethod, true);
        this.returnParam = operation.EndMethod.ReturnParameter;
      }
      else if (this.syncMethod != (MethodInfo) null)
      {
        this.inParams = ServiceReflector.GetInputParameters(this.syncMethod, false);
        this.outParams = ServiceReflector.GetOutputParameters(this.syncMethod, false);
        this.returnParam = this.syncMethod.ReturnParameter;
      }
      if (this.formatter != null || !this.serializeRequest && !this.deserializeReply)
        return;
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("ClientRuntimeRequiresFormatter0", new object[1]
      {
        (object) this.name
      })));
    }

    internal void AfterReply(ref ProxyRpc rpc)
    {
      if (this.isOneWay)
        return;
      System.ServiceModel.Channels.Message message = rpc.Reply;
      if (this.deserializeReply)
      {
        if (TD.ClientFormatterDeserializeReplyStartIsEnabled())
          TD.ClientFormatterDeserializeReplyStart(rpc.EventTraceActivity);
        rpc.ReturnValue = this.formatter.DeserializeReply(message, rpc.OutputParameters);
        if (TD.ClientFormatterDeserializeReplyStopIsEnabled())
          TD.ClientFormatterDeserializeReplyStop(rpc.EventTraceActivity);
      }
      else
        rpc.ReturnValue = (object) message;
      int correlationOffset = this.parent.ParameterInspectorCorrelationOffset;
      try
      {
        for (int index = this.parameterInspectors.Length - 1; index >= 0; --index)
        {
          this.parameterInspectors[index].AfterCall(this.name, rpc.OutputParameters, rpc.ReturnValue, rpc.Correlation[correlationOffset + index]);
          if (TD.ClientParameterInspectorAfterCallInvokedIsEnabled())
            TD.ClientParameterInspectorAfterCallInvoked(rpc.EventTraceActivity, this.parameterInspectors[index].GetType().FullName);
        }
      }
      catch (Exception ex)
      {
        if (Fx.IsFatal(ex))
        {
          throw;
        }
        else
        {
          if (!System.ServiceModel.Dispatcher.ErrorBehavior.ShouldRethrowClientSideExceptionAsIs(ex))
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(ex);
          throw;
        }
      }
      if (!this.parent.ValidateMustUnderstand)
        return;
      Collection<MessageHeaderInfo> headersNotUnderstood = message.Headers.GetHeadersNotUnderstood();
      if (headersNotUnderstood == null || headersNotUnderstood.Count <= 0)
        return;
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ProtocolException(System.ServiceModel.SR.GetString("SFxHeaderNotUnderstood", (object) headersNotUnderstood[0].Name, (object) headersNotUnderstood[0].Namespace)));
    }

    internal void BeforeRequest(ref ProxyRpc rpc)
    {
      int correlationOffset = this.parent.ParameterInspectorCorrelationOffset;
      try
      {
        for (int index = 0; index < this.parameterInspectors.Length; ++index)
        {
          rpc.Correlation[correlationOffset + index] = this.parameterInspectors[index].BeforeCall(this.name, rpc.InputParameters);
          if (TD.ClientParameterInspectorBeforeCallInvokedIsEnabled())
            TD.ClientParameterInspectorBeforeCallInvoked(rpc.EventTraceActivity, this.parameterInspectors[index].GetType().FullName);
        }
      }
      catch (Exception ex)
      {
        if (Fx.IsFatal(ex))
        {
          throw;
        }
        else
        {
          if (!System.ServiceModel.Dispatcher.ErrorBehavior.ShouldRethrowClientSideExceptionAsIs(ex))
            throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(ex);
          throw;
        }
      }
      if (this.serializeRequest)
      {
        if (TD.ClientFormatterSerializeRequestStartIsEnabled())
          TD.ClientFormatterSerializeRequestStart(rpc.EventTraceActivity);
        rpc.Request = this.formatter.SerializeRequest(rpc.MessageVersion, rpc.InputParameters);
        if (!TD.ClientFormatterSerializeRequestStopIsEnabled())
          return;
        TD.ClientFormatterSerializeRequestStop(rpc.EventTraceActivity);
      }
      else if (rpc.InputParameters[0] == null)
      {
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxProxyRuntimeMessageCannotBeNull", new object[1]
        {
          (object) this.name
        })));
      }
      else
      {
        rpc.Request = (System.ServiceModel.Channels.Message) rpc.InputParameters[0];
        if (ProxyOperationRuntime.IsValidAction(rpc.Request, this.Action))
          return;
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxInvalidRequestAction", (object) this.Name, (object) (rpc.Request.Headers.Action ?? "{NULL}"), (object) this.Action)));
      }
    }

    internal static object GetDefaultParameterValue(System.Type parameterType)
    {
      if (!parameterType.IsValueType || !(parameterType != typeof (void)))
        return (object) null;
      else
        return Activator.CreateInstance(parameterType);
    }

    [SecurityCritical]
    internal bool IsSyncCall(IMethodCallMessage methodCall)
    {
      if (this.syncMethod == (MethodInfo) null)
        return false;
      else
        return methodCall.MethodBase.MethodHandle == this.syncMethod.MethodHandle;
    }

    [SecurityCritical]
    internal bool IsBeginCall(IMethodCallMessage methodCall)
    {
      if (this.beginMethod == (MethodInfo) null)
        return false;
      else
        return methodCall.MethodBase.MethodHandle == this.beginMethod.MethodHandle;
    }

    [SecurityCritical]
    internal bool IsTaskCall(IMethodCallMessage methodCall)
    {
      if (this.taskMethod == (MethodInfo) null)
        return false;
      else
        return methodCall.MethodBase.MethodHandle == this.taskMethod.MethodHandle;
    }

    [SecurityCritical]
    internal object[] MapSyncInputs(IMethodCallMessage methodCall, out object[] outs)
    {
      outs = this.outParams.Length != 0 ? new object[this.outParams.Length] : ProxyOperationRuntime.EmptyArray;
      if (this.inParams.Length == 0)
        return ProxyOperationRuntime.EmptyArray;
      else
        return methodCall.InArgs;
    }

    [SecurityCritical]
    internal object[] MapAsyncBeginInputs(IMethodCallMessage methodCall, out AsyncCallback callback, out object asyncState)
    {
      object[] objArray = this.inParams.Length != 0 ? new object[this.inParams.Length] : ProxyOperationRuntime.EmptyArray;
      object[] args = methodCall.Args;
      for (int index = 0; index < objArray.Length; ++index)
        objArray[index] = args[this.inParams[index].Position];
      callback = args[methodCall.ArgCount - 2] as AsyncCallback;
      asyncState = args[methodCall.ArgCount - 1];
      return objArray;
    }

    [SecurityCritical]
    internal void MapAsyncEndInputs(IMethodCallMessage methodCall, out IAsyncResult result, out object[] outs)
    {
      outs = new object[this.endOutParams.Length];
      result = methodCall.Args[methodCall.ArgCount - 1] as IAsyncResult;
    }

    [SecurityCritical]
    internal object[] MapSyncOutputs(IMethodCallMessage methodCall, object[] outs, ref object ret)
    {
      return this.MapOutputs(this.outParams, methodCall, outs, ref ret);
    }

    [SecurityCritical]
    internal object[] MapAsyncOutputs(IMethodCallMessage methodCall, object[] outs, ref object ret)
    {
      return this.MapOutputs(this.endOutParams, methodCall, outs, ref ret);
    }

    internal static bool IsValidAction(System.ServiceModel.Channels.Message message, string action)
    {
      if (message == null)
        return false;
      if (message.IsFault || action == "*")
        return true;
      else
        return string.CompareOrdinal(message.Headers.Action, action) == 0;
    }

    [SecurityCritical]
    private object[] MapOutputs(ParameterInfo[] parameters, IMethodCallMessage methodCall, object[] outs, ref object ret)
    {
      if (ret == null && this.returnParam != null)
        ret = ProxyOperationRuntime.GetDefaultParameterValue(TypeLoader.GetParameterType(this.returnParam));
      if (parameters.Length == 0)
        return (object[]) null;
      object[] args = methodCall.Args;
      for (int index = 0; index < parameters.Length; ++index)
        args[parameters[index].Position] = outs[index] != null ? outs[index] : ProxyOperationRuntime.GetDefaultParameterValue(TypeLoader.GetParameterType(parameters[index]));
      return args;
    }
  }
}
