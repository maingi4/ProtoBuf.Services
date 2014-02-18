// Type: System.ServiceModel.Channels.RequestContext
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Runtime;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Provides a reply that is correlated to an incoming request.
  /// </summary>
  [__DynamicallyInvokable]
  public abstract class RequestContext : IDisposable
  {
    /// <summary>
    /// When overridden in a derived class, gets the message that contains the request.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.Message"/> that contains the request.
    /// </returns>
    [__DynamicallyInvokable]
    public abstract Message RequestMessage { [__DynamicallyInvokable] get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.RequestContext"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected RequestContext()
    {
    }

    /// <summary>
    /// When overridden in a derived class, aborts processing the request associated with the context.
    /// </summary>
    [__DynamicallyInvokable]
    public abstract void Abort();

    /// <summary>
    /// When overridden in a derived class, closes the operation that is replying to the request context associated with the current context.
    /// </summary>
    [__DynamicallyInvokable]
    public abstract void Close();

    /// <summary>
    /// When overridden in a derived class, closes the operation that is replying to the request context associated with the current context within a specified interval of time.
    /// </summary>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies the interval of time within which the reply operation associated with the current context must close.</param>
    [__DynamicallyInvokable]
    public abstract void Close(TimeSpan timeout);

    /// <summary>
    /// When overridden in a derived class, replies to a request message.
    /// </summary>
    /// <param name="message">The incoming <see cref="T:System.ServiceModel.Channels.Message"/> that contains the request.</param>
    [__DynamicallyInvokable]
    public abstract void Reply(Message message);

    /// <summary>
    /// When overridden in a derived class, replies to a request message within a specified interval of time.
    /// </summary>
    /// <param name="message">The incoming <see cref="T:System.ServiceModel.Channels.Message"/> that contains the request.</param><param name="timeout">The <see cref="T:System.Timespan"/> that specifies the interval of time to wait for the reply to a request.</param>
    [__DynamicallyInvokable]
    public abstract void Reply(Message message, TimeSpan timeout);

    /// <summary>
    /// When overridden in a derived class, begins an asynchronous operation to reply to the request associated with the current context.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous reply operation.
    /// </returns>
    /// <param name="message">The incoming <see cref="T:System.ServiceModel.Channels.Message"/> that contains the request.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous reply operation completion.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous reply operation.</param>
    [__DynamicallyInvokable]
    public abstract IAsyncResult BeginReply(Message message, AsyncCallback callback, object state);

    /// <summary>
    /// When overridden in a derived class, begins an asynchronous operation to reply to the request associated with the current context within a specified interval of time.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous reply operation.
    /// </returns>
    /// <param name="message">The incoming <see cref="T:System.ServiceModel.Channels.Message"/> that contains the request.</param><param name="timeout">The <see cref="T:System.Timespan"/> that specifies the interval of time to wait for the reply to an available request.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous reply operation completion.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous reply operation.</param>
    [__DynamicallyInvokable]
    public abstract IAsyncResult BeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// When overridden in a derived class, completes an asynchronous operation to reply to a request message.
    /// </summary>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to one of the <see cref="Overload:System.ServiceModel.Channels.RequestContext.BeginReply"/> methods.</param>
    [__DynamicallyInvokable]
    public abstract void EndReply(IAsyncResult result);

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    void IDisposable.Dispose()
    {
      this.Dispose(true);
    }

    /// <summary>
    /// Releases resources associated with the context.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
    [__DynamicallyInvokable]
    protected virtual void Dispose(bool disposing)
    {
    }
  }
}
