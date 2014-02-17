// Type: System.ServiceModel.Channels.IRequestChannel
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Defines the contract that a channel must implement to be on the requesting side of a request-reply communication between messaging endpoints.
  /// </summary>
  [__DynamicallyInvokable]
  public interface IRequestChannel : IChannel, ICommunicationObject
  {
    /// <summary>
    /// Gets the remote address to which the request channel sends messages.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.EndpointAddress"/> to which the request channel sends messages.
    /// </returns>
    [__DynamicallyInvokable]
    EndpointAddress RemoteAddress { [__DynamicallyInvokable] get; }

    /// <summary>
    /// Gets the transport address to which the request is send.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.Uri"/> that contains the transport address to which the message is sent.
    /// </returns>
    [__DynamicallyInvokable]
    Uri Via { [__DynamicallyInvokable] get; }

    /// <summary>
    /// Sends a message-based request and returns the correlated message-based response.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.Message"/> received in response to the request.
    /// </returns>
    /// <param name="message">The request <see cref="T:System.ServiceModel.Channels.Message"/> to be transmitted.</param>
    [__DynamicallyInvokable]
    Message Request(Message message);

    /// <summary>
    /// Sends a message-based request and returns the correlated message-based response within a specified interval of time.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.Message"/> received in response to the request.
    /// </returns>
    /// <param name="message">The request <see cref="T:System.ServiceModel.Channels.Message"/> to be transmitted.</param><param name="timeout">The <see cref="T:System.TimeSpan"/> that specifies the interval of time within which a response must be received.</param>
    [__DynamicallyInvokable]
    Message Request(Message message, TimeSpan timeout);

    /// <summary>
    /// Begins an asynchronous operation to transmit a request message to the reply side of a request-reply message exchange.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous message transmission.
    /// </returns>
    /// <param name="message">The request <see cref="T:System.ServiceModel.Channels.Message"/> to be transmitted.</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the completion of the asynchronous operation transmitting a request message.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous operation transmitting a request message.</param>
    [__DynamicallyInvokable]
    IAsyncResult BeginRequest(Message message, AsyncCallback callback, object state);

    /// <summary>
    /// Begins an asynchronous operation to transmit a request message to the reply side of a request-reply message exchange within a specified interval of time.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.IAsyncResult"/> that references the asynchronous message transmission.
    /// </returns>
    /// <param name="message">The request <see cref="T:System.ServiceModel.Channels.Message"/> to be transmitted.</param><param name="timeout">The <see cref="T:System.TimeSpan"/> that specifies the interval of time within which a response must be received. (For defaults, see the Remarks section.)</param><param name="callback">The <see cref="T:System.AsyncCallback"/> delegate that receives the notification of the completion of the asynchronous operation transmitting a request message.</param><param name="state">An object, specified by the application, that contains state information associated with the asynchronous operation transmitting a request message.</param>
    [__DynamicallyInvokable]
    IAsyncResult BeginRequest(Message message, TimeSpan timeout, AsyncCallback callback, object state);

    /// <summary>
    /// Completes an asynchronous operation to return a message-based response to a transmitted request.
    /// </summary>
    /// 
    /// <returns>
    /// The <see cref="T:System.ServiceModel.Channels.Message"/> received in response to the request.
    /// </returns>
    /// <param name="result">The <see cref="T:System.IAsyncResult"/> returned by a call to the <see cref="Overload:System.ServiceModel.Channels.IInputChannel.BeginReceive"/> method. </param>
    [__DynamicallyInvokable]
    Message EndRequest(IAsyncResult result);
  }
}
