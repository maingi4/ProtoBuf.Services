// Type: System.ServiceModel.ChannelFactory
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Configuration;
using System.Runtime;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;
using System.ServiceModel.Diagnostics.Application;
using System.ServiceModel.Security;

namespace System.ServiceModel
{
  /// <summary>
  /// Creates and manages the channels that are used by clients to send messages to service endpoints.
  /// </summary>
  [__DynamicallyInvokable]
  public abstract class ChannelFactory : CommunicationObject, IChannelFactory, ICommunicationObject, IDisposable
  {
    private object openLock = new object();
    private string configurationName;
    private IChannelFactory innerFactory;
    private ServiceEndpoint serviceEndpoint;
    private ClientCredentials readOnlyClientCredentials;

    /// <summary>
    /// Gets the credentials used by clients to communicate a service endpoint over the channels produced by the factory.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Description.ClientCredentials"/> used by clients if they are configured for the factory or if the endpoint is non-null and is in either the created or opening communication state; otherwise null.
    /// </returns>
    [__DynamicallyInvokable]
    public ClientCredentials Credentials
    {
      [__DynamicallyInvokable] get
      {
        if (this.Endpoint == null)
          return (ClientCredentials) null;
        if (this.State == CommunicationState.Created || this.State == CommunicationState.Opening)
          return this.EnsureCredentials(this.Endpoint);
        if (this.readOnlyClientCredentials == null)
        {
          ClientCredentials clientCredentials = new ClientCredentials();
          clientCredentials.MakeReadOnly();
          this.readOnlyClientCredentials = clientCredentials;
        }
        return this.readOnlyClientCredentials;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for a close operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the close operation has to complete before timing out.
    /// </returns>
    [__DynamicallyInvokable]
    protected override TimeSpan DefaultCloseTimeout
    {
      [__DynamicallyInvokable] get
      {
        if (this.Endpoint != null && this.Endpoint.Binding != null)
          return this.Endpoint.Binding.CloseTimeout;
        else
          return ServiceDefaults.CloseTimeout;
      }
    }

    /// <summary>
    /// Gets the default interval of time provided for an open operation to complete.
    /// </summary>
    /// 
    /// <returns>
    /// The default <see cref="T:System.Timespan"/> that specifies how long the open operation has to complete before timing out.
    /// </returns>
    [__DynamicallyInvokable]
    protected override TimeSpan DefaultOpenTimeout
    {
      [__DynamicallyInvokable] get
      {
        if (this.Endpoint != null && this.Endpoint.Binding != null)
          return this.Endpoint.Binding.OpenTimeout;
        else
          return ServiceDefaults.OpenTimeout;
      }
    }

    /// <summary>
    /// Gets the service endpoint to which the channels produced by the factory connect.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Description.ServiceEndpoint"/> to which the channels produced by the factory connect.
    /// </returns>
    [__DynamicallyInvokable]
    public ServiceEndpoint Endpoint
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.serviceEndpoint;
      }
    }

    internal IChannelFactory InnerFactory
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.innerFactory;
      }
    }

    internal bool UseActiveAutoClose { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.ChannelFactory"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    protected ChannelFactory()
    {
      TraceUtility.SetEtwProviderId();
      this.TraceOpenAndClose = true;
    }

    /// <summary>
    /// Opens the current channel factory if it is not yet opened.
    /// </summary>
    /// <exception cref="T:System.ObjectDisposedException">The current factory is either closing or closed and so cannot be opened.</exception>
    [__DynamicallyInvokable]
    protected internal void EnsureOpened()
    {
      this.ThrowIfDisposed();
      if (this.State == CommunicationState.Opened)
        return;
      lock (this.openLock)
      {
        if (this.State == CommunicationState.Opened)
          return;
        this.Open();
      }
    }

    /// <summary>
    /// Initializes the channel factory with the behaviors provided by a specified configuration file and with those in the service endpoint of the channel factory.
    /// </summary>
    /// <param name="configurationName">The name of the configuration file.</param><exception cref="T:System.InvalidOperationException">The service endpoint of the channel factory is null.</exception>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected virtual void ApplyConfiguration(string configurationName)
    {
      this.ApplyConfiguration(configurationName, (Configuration) null);
    }

    private void ApplyConfiguration(string configurationName, Configuration configuration)
    {
      if (this.Endpoint == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxChannelFactoryCannotApplyConfigurationWithoutEndpoint")));
      if (this.Endpoint.IsFullyConfigured)
        return;
      ConfigLoader configLoader = configuration == null ? new ConfigLoader() : new ConfigLoader(configuration.EvaluationContext);
      if (configurationName == null)
        configLoader.LoadCommonClientBehaviors(this.Endpoint);
      else
        configLoader.LoadChannelBehaviors(this.Endpoint, configurationName);
    }

    /// <summary>
    /// When implemented in a derived class, creates a description of the service endpoint associated with the channel factory.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Description.ServiceEndpoint"/> associated with the channel factory.
    /// </returns>
    [__DynamicallyInvokable]
    protected abstract ServiceEndpoint CreateDescription();

    internal EndpointAddress CreateEndpointAddress(ServiceEndpoint endpoint)
    {
      if (endpoint.Address == (EndpointAddress) null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxChannelFactoryEndpointAddressUri")));
      else
        return endpoint.Address;
    }

    /// <summary>
    /// Builds the channel factory for the current endpoint of the factory.
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:System.ServiceModel.Channels.IChannelFactory"/> for the endpoint of the current factory.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The endpoint of the service that the factory channels connect to is null or the endpoint's binding is null or is missing the element with the configuration name specified.</exception>
    [__DynamicallyInvokable]
    protected virtual IChannelFactory CreateFactory()
    {
      if (this.Endpoint == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxChannelFactoryCannotCreateFactoryWithoutDescription")));
      if (this.Endpoint.Binding != null)
        return (IChannelFactory) ServiceChannelFactory.BuildChannelFactory(this.Endpoint, this.UseActiveAutoClose);
      if (this.configurationName == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxChannelFactoryNoBindingFoundInConfigOrCode")));
      throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("SFxChannelFactoryNoBindingFoundInConfig1", new object[1]
      {
        (object) this.configurationName
      })));
    }

    [__DynamicallyInvokable]
    void IDisposable.Dispose()
    {
      this.Close();
    }

    /// <summary>
    /// Returns the typed object requested, if present, from the appropriate layer in the channel stack, or null if not present.
    /// </summary>
    /// 
    /// <returns>
    /// The typed object <paramref name="T"/> requested if it is present or null if it is not.
    /// </returns>
    /// <typeparam name="T">The typed object for which the method is querying.</typeparam>
    [__DynamicallyInvokable]
    public T GetProperty<T>() where T : class
    {
      if (this.innerFactory != null)
        return this.innerFactory.GetProperty<T>();
      else
        return default (T);
    }

    internal bool HasDuplexOperations()
    {
      OperationDescriptionCollection operations = this.Endpoint.Contract.Operations;
      for (int index = 0; index < operations.Count; ++index)
      {
        if (operations[index].IsServerInitiated())
          return true;
      }
      return false;
    }

    /// <summary>
    /// Initializes the service endpoint of the channel factory with a specified address and configuration.
    /// </summary>
    /// <param name="configurationName">The name of the configuration file used to initialize the channel factory.</param><param name="address">The <see cref="T:System.ServiceModel.EndpointAddress"/> with which to initialize the channel factory.</param>
    [__DynamicallyInvokable]
    protected void InitializeEndpoint(string configurationName, EndpointAddress address)
    {
      this.serviceEndpoint = this.CreateDescription();
      ServiceEndpoint serviceEndpoint = (ServiceEndpoint) null;
      if (configurationName != null)
        serviceEndpoint = ConfigLoader.LookupEndpoint(configurationName, address, this.serviceEndpoint.Contract);
      if (serviceEndpoint != null)
      {
        this.serviceEndpoint = serviceEndpoint;
      }
      else
      {
        if (address != (EndpointAddress) null)
          this.Endpoint.Address = address;
        this.ApplyConfiguration(configurationName);
      }
      this.configurationName = configurationName;
      this.EnsureSecurityCredentialsManager(this.serviceEndpoint);
    }

    internal void InitializeEndpoint(string configurationName, EndpointAddress address, Configuration configuration)
    {
      this.serviceEndpoint = this.CreateDescription();
      ServiceEndpoint serviceEndpoint = (ServiceEndpoint) null;
      if (configurationName != null)
        serviceEndpoint = ConfigLoader.LookupEndpoint(configurationName, address, this.serviceEndpoint.Contract, configuration.EvaluationContext);
      if (serviceEndpoint != null)
      {
        this.serviceEndpoint = serviceEndpoint;
      }
      else
      {
        if (address != (EndpointAddress) null)
          this.Endpoint.Address = address;
        this.ApplyConfiguration(configurationName, configuration);
      }
      this.configurationName = configurationName;
      this.EnsureSecurityCredentialsManager(this.serviceEndpoint);
    }

    /// <summary>
    /// Initializes the service endpoint of the channel factory with a specified endpoint.
    /// </summary>
    /// <param name="endpoint">The <see cref="T:System.ServiceModel.Description.ServiceEndpoint"/> to initialize the channel factory with.</param><exception cref="T:System.ArgumentNullException"><paramref name="endpoint"/> is null.</exception>
    [__DynamicallyInvokable]
    protected void InitializeEndpoint(ServiceEndpoint endpoint)
    {
      if (endpoint == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("endpoint");
      this.serviceEndpoint = endpoint;
      this.ApplyConfiguration((string) null);
      this.EnsureSecurityCredentialsManager(this.serviceEndpoint);
    }

    /// <summary>
    /// Initializes the service endpoint of the channel factory with a specified binding and address.
    /// </summary>
    /// <param name="binding">The <see cref="T:System.ServiceModel.Channels.Binding"/> with which to initialize the channel factory.</param><param name="address">The <see cref="T:System.ServiceModel.EndpointAddress"/> with which to initialize the channel factory.</param>
    [__DynamicallyInvokable]
    protected void InitializeEndpoint(Binding binding, EndpointAddress address)
    {
      this.serviceEndpoint = this.CreateDescription();
      if (binding != null)
        this.Endpoint.Binding = binding;
      if (address != (EndpointAddress) null)
        this.Endpoint.Address = address;
      this.ApplyConfiguration((string) null);
      this.EnsureSecurityCredentialsManager(this.serviceEndpoint);
    }

    /// <summary>
    /// Initializes a read-only copy of the <see cref="T:System.ServiceModel.Description.ClientCredentials"/> object for the channel factory.
    /// </summary>
    [__DynamicallyInvokable]
    protected override void OnOpened()
    {
      if (this.Endpoint != null)
      {
        ClientCredentials clientCredentials1 = this.Endpoint.Behaviors.Find<ClientCredentials>();
        if (clientCredentials1 != null)
        {
          ClientCredentials clientCredentials2 = clientCredentials1.Clone();
          clientCredentials2.MakeReadOnly();
          this.readOnlyClientCredentials = clientCredentials2;
        }
      }
      base.OnOpened();
    }

    /// <summary>
    /// Terminates the inner channel factory of the current channel factory.
    /// </summary>
    [__DynamicallyInvokable]
    protected override void OnAbort()
    {
      if (this.innerFactory == null)
        return;
      this.innerFactory.Abort();
    }

    /// <summary>
    /// Begins an asynchronous close operation on the inner channel factory of the current channel factory that has a state object associated with it.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous operation.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the operation has to complete before timing out.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous operation completion.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous operation.</param>
    [__DynamicallyInvokable]
    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new ChannelFactory.CloseAsyncResult((ICommunicationObject) this.innerFactory, timeout, callback, state);
    }

    /// <summary>
    /// Begins an asynchronous open operation on the inner channel factory of the current channel factory that has a state object associated with it.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous operation.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the operation has to complete before timing out.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous operation completion.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous operation.</param>
    [__DynamicallyInvokable]
    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
    {
      return (IAsyncResult) new ChannelFactory.OpenAsyncResult((ICommunicationObject) this.innerFactory, timeout, callback, state);
    }

    /// <summary>
    /// Calls close on the inner channel factory with a specified time-out for the completion of the operation.
    /// </summary>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the operation has to complete before timing out.</param>
    [__DynamicallyInvokable]
    protected override void OnClose(TimeSpan timeout)
    {
      if (this.innerFactory == null)
        return;
      this.innerFactory.Close(timeout);
    }

    /// <summary>
    /// Completes an asynchronous close operation on the inner channel factory of the current channel factory.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="M:System.ServiceModel.ChannelFactory.OnBeginClose(System.TimeSpan,System.AsyncCallback,System.Object)"/> method.</param>
    [__DynamicallyInvokable]
    protected override void OnEndClose(IAsyncResult result)
    {
      ChannelFactory.CloseAsyncResult.End(result);
    }

    /// <summary>
    /// Completes an asynchronous open operation on the inner channel factory of the current channel factory.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="M:System.ServiceModel.ChannelFactory.OnBeginOpen(System.TimeSpan,System.AsyncCallback,System.Object)"/> method.</param>
    [__DynamicallyInvokable]
    protected override void OnEndOpen(IAsyncResult result)
    {
      ChannelFactory.OpenAsyncResult.End(result);
    }

    /// <summary>
    /// Calls open on the inner channel factory of the current channel factory with a specified time-out for the completion of the operation.
    /// </summary>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the open operation has to complete before timing out.</param><exception cref="T:System.InvalidOperationException">The inner channel of the current channel is null.</exception>
    [__DynamicallyInvokable]
    protected override void OnOpen(TimeSpan timeout)
    {
      this.innerFactory.Open(timeout);
    }

    /// <summary>
    /// Builds the inner channel factory for the current channel.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">The inner channel factory for the channel factory is null.</exception>
    [__DynamicallyInvokable]
    protected override void OnOpening()
    {
      base.OnOpening();
      this.innerFactory = this.CreateFactory();
      if (TD.ChannelFactoryCreatedIsEnabled())
        TD.ChannelFactoryCreated((object) this);
      if (this.innerFactory == null)
        throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new InvalidOperationException(System.ServiceModel.SR.GetString("InnerChannelFactoryWasNotSet")));
    }

    private void EnsureSecurityCredentialsManager(ServiceEndpoint endpoint)
    {
      if (endpoint.Behaviors.Find<SecurityCredentialsManager>() != null)
        return;
      endpoint.Behaviors.Add((IEndpointBehavior) new ClientCredentials());
    }

    private ClientCredentials EnsureCredentials(ServiceEndpoint endpoint)
    {
      ClientCredentials clientCredentials = endpoint.Behaviors.Find<ClientCredentials>();
      if (clientCredentials == null)
      {
        clientCredentials = new ClientCredentials();
        endpoint.Behaviors.Add((IEndpointBehavior) clientCredentials);
      }
      return clientCredentials;
    }

    private class OpenAsyncResult : AsyncResult
    {
      private static AsyncCallback onOpenComplete = Fx.ThunkCallback(new AsyncCallback(ChannelFactory.OpenAsyncResult.OnOpenComplete));
      private ICommunicationObject communicationObject;

      static OpenAsyncResult()
      {
      }

      public OpenAsyncResult(ICommunicationObject communicationObject, TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.communicationObject = communicationObject;
        if (this.communicationObject == null)
        {
          this.Complete(true);
        }
        else
        {
          IAsyncResult result = this.communicationObject.BeginOpen(timeout, ChannelFactory.OpenAsyncResult.onOpenComplete, (object) this);
          if (!result.CompletedSynchronously)
            return;
          this.communicationObject.EndOpen(result);
          this.Complete(true);
        }
      }

      private static void OnOpenComplete(IAsyncResult result)
      {
        if (result.CompletedSynchronously)
          return;
        ChannelFactory.OpenAsyncResult openAsyncResult = (ChannelFactory.OpenAsyncResult) result.AsyncState;
        Exception exception = (Exception) null;
        try
        {
          openAsyncResult.communicationObject.EndOpen(result);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        openAsyncResult.Complete(false, exception);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<ChannelFactory.OpenAsyncResult>(result);
      }
    }

    private class CloseAsyncResult : AsyncResult
    {
      private static AsyncCallback onCloseComplete = Fx.ThunkCallback(new AsyncCallback(ChannelFactory.CloseAsyncResult.OnCloseComplete));
      private ICommunicationObject communicationObject;

      static CloseAsyncResult()
      {
      }

      public CloseAsyncResult(ICommunicationObject communicationObject, TimeSpan timeout, AsyncCallback callback, object state)
        : base(callback, state)
      {
        this.communicationObject = communicationObject;
        if (this.communicationObject == null)
        {
          this.Complete(true);
        }
        else
        {
          IAsyncResult result = this.communicationObject.BeginClose(timeout, ChannelFactory.CloseAsyncResult.onCloseComplete, (object) this);
          if (!result.CompletedSynchronously)
            return;
          this.communicationObject.EndClose(result);
          this.Complete(true);
        }
      }

      private static void OnCloseComplete(IAsyncResult result)
      {
        if (result.CompletedSynchronously)
          return;
        ChannelFactory.CloseAsyncResult closeAsyncResult = (ChannelFactory.CloseAsyncResult) result.AsyncState;
        Exception exception = (Exception) null;
        try
        {
          closeAsyncResult.communicationObject.EndClose(result);
        }
        catch (Exception ex)
        {
          if (Fx.IsFatal(ex))
            throw;
          else
            exception = ex;
        }
        closeAsyncResult.Complete(false, exception);
      }

      public static void End(IAsyncResult result)
      {
        AsyncResult.End<ChannelFactory.CloseAsyncResult>(result);
      }
    }
  }
}
