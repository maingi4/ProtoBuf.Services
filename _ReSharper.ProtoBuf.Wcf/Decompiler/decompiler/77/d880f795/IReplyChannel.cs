// Type: System.ServiceModel.Channels.IReplyChannel
// Assembly: System.ServiceModel, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\v3.0\System.ServiceModel.dll

using System;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Defines the interface that a channel must implement to be on the receiving side of a request-reply communication between messaging endpoints.
  /// </summary>
  public interface IReplyChannel : IChannel, ICommunicationObject
  {
    /// <summary>
    /// Gets the address on which this reply channel receives messages.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.EndpointAddress"/> on which this reply channel receives messages.
    /// </returns>
    EndpointAddress LocalAddress { get; }

    /// <summary>
    /// Returns the context of the request received, if one is available. If a context is not available, waits until there is one available.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.RequestContext"/> used to construct replies.
    /// </returns>
    RequestContext ReceiveRequest();

    /// <summary>
    /// Returns the context of the request received, if one is available. If a context is not available, waits until there is one available.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.RequestContext"/> used to construct replies.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.TimeSpan"/> that specifies how long the receive of a request operation has to complete before timing out and returning false.</param>
    RequestContext ReceiveRequest(TimeSpan timeout);

    /// <summary>
    /// Begins an asynchronous operation to receive an available request with a default timeout.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous reception of the request.
    /// </returns>
    /// <param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous receive that a request operation completes.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous receive of a request operation.</param>
    IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state);

    /// <summary>
    /// Begins an asynchronous operation to receive an available request with a specified timeout.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous reception of the request.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies the interval of time to wait for the reception of an available request.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous receive that a request operation completes.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous receive of a request operation.</param>
    IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// Completes an asynchronous operation to receive an available request.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.RequestContext"/> used to construct a reply to the request.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="M:System.ServiceModel.Channels.IInputChannel.BeginReceive"/> method.</param>
    RequestContext EndReceiveRequest(IAsyncResult result);

    /// <summary>
    /// Returns a value that indicates whether a request is received before a specified interval of time elapses.
    /// </summary>
    /// 
    /// <returns>
    /// true if a request message is received before the specified interval of time elapses; otherwise false.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.TimeSpan"/> that specifies how long the receive of a request operation has to complete before timing out and returning false.</param><param name="context">The <see cref="T:System.ServiceModel.Channels.RequestContext"/> received.</param>
    bool TryReceiveRequest(TimeSpan timeout, out RequestContext context);

    /// <summary>
    /// Begins an asynchronous operation to receive a request message that has a specified time out and state object associated with it.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous receive request operation.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the receive request operation has to complete before timing out and returning false.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous receive that a request operation completes.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous receive of a request operation.</param>
    IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// Completes the specified asynchronous operation to receive a request message.
    /// </summary>
    /// 
    /// <returns>
    /// true if a request message is received before the specified interval of time elapses; otherwise false.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="M:System.ServiceModel.Channels.IReplyChannel.BeginTryReceiveRequest(System.TimeSpan,System.AsyncCallback,System.Object)"/> method.</param><param name="context">The <see cref="T:System.ServiceModel.Channels.RequestContext"/> received.</param>
    bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context);

    /// <summary>
    /// Returns a value that indicates whether a request message is received before a specified interval of time elapses.
    /// </summary>
    /// 
    /// <returns>
    /// true if a request is received before the specified interval of time elapses; otherwise false.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long a request operation has to complete before timing out and returning false.</param>
    bool WaitForRequest(TimeSpan timeout);

    /// <summary>
    /// Begins an asynchronous request operation that has a specified time out and state object associated with it.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous operation to wait for a request message to arrive.
    /// </returns>
    /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies the interval of time to wait for the reception of an available request.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the asynchronous receive that a request operation completes.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous receive of a request operation.</param>
    IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// Completes the specified asynchronous wait-for-a-request message operation.
    /// </summary>
    /// 
    /// <returns>
    /// true if a request is received before the specified interval of time elapses; otherwise false.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> that identifies the <see cref="M:System.ServiceModel.Channels.IReplyChannel.BeginWaitForRequest(System.TimeSpan,System.AsyncCallback,System.Object)"/> operation to finish, and from which to retrieve an end result.</param>
    bool EndWaitForRequest(IAsyncResult result);
  }
}
