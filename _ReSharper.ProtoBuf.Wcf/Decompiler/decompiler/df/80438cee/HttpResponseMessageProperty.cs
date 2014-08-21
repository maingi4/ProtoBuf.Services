// Type: System.ServiceModel.Channels.HttpResponseMessageProperty
// Assembly: System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.ServiceModel.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime;
using System.ServiceModel;

namespace System.ServiceModel.Channels
{
  /// <summary>
  /// Provides access to the HTTP response in order to access and respond to the additional information made available for requests over the HTTP protocol.
  /// </summary>
  [__DynamicallyInvokable]
  public sealed class HttpResponseMessageProperty : IMessageProperty, IMergeEnabledMessageProperty
  {
    private HttpResponseMessageProperty.TraditionalHttpResponseMessageProperty traditionalProperty;
    private HttpResponseMessageProperty.HttpResponseMessageBackedProperty httpBackedProperty;
    private bool useHttpBackedProperty;
    private bool initialCopyPerformed;

    /// <summary>
    /// Gets the name of the message property associated with the <see cref="T:System.ServiceModel.Channels.HttpResponseMessageProperty"/> class.
    /// </summary>
    /// 
    /// <returns>
    /// Returns "httpResponse".
    /// </returns>
    [__DynamicallyInvokable]
    public static string Name
    {
      [__DynamicallyInvokable] get
      {
        return "httpResponse";
      }
    }

    /// <summary>
    /// Gets the HTTP headers from the HTTP response.
    /// </summary>
    /// 
    /// <returns>
    /// Returns a <see cref="T:System.Net.WebHeaderCollection"/> that contains the HTTP headers in the HTTP response.
    /// </returns>
    [__DynamicallyInvokable]
    public WebHeaderCollection Headers
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.Headers;
        else
          return this.httpBackedProperty.Headers;
      }
    }

    /// <summary>
    /// Gets or sets the status code of the current HTTP response to which this property is attached.
    /// </summary>
    /// 
    /// <returns>
    /// Returns the <see cref="P:System.ServiceModel.Channels.HttpResponseMessageProperty.StatusCode"/> to send on the HTTP response.
    /// </returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is set to less than 100 or greater than 599.</exception>
    [__DynamicallyInvokable]
    public HttpStatusCode StatusCode
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.StatusCode;
        else
          return this.httpBackedProperty.StatusCode;
      }
      [__DynamicallyInvokable] set
      {
        int num = (int) value;
        if (num < 100 || num > 599)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperError((Exception) new ArgumentOutOfRangeException("value", (object) value, System.ServiceModel.SR.GetString("ValueMustBeInRange", (object) 100, (object) 599)));
        else if (this.useHttpBackedProperty)
          this.httpBackedProperty.StatusCode = value;
        else
          this.traditionalProperty.StatusCode = value;
      }
    }

    internal bool HasStatusCodeBeenSet
    {
      get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.HasStatusCodeBeenSet;
        else
          return true;
      }
    }

    /// <summary>
    /// Gets or sets the description of the status code of the current HTTP response to which this property is attached.
    /// </summary>
    /// 
    /// <returns>
    /// Returns the <see cref="P:System.ServiceModel.Channels.HttpResponseMessageProperty.StatusDescription"/> to send for the HTTP response.
    /// </returns>
    [__DynamicallyInvokable]
    public string StatusDescription
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.StatusDescription;
        else
          return this.httpBackedProperty.StatusDescription;
      }
      [__DynamicallyInvokable] set
      {
        if (this.useHttpBackedProperty)
          this.httpBackedProperty.StatusDescription = value;
        else
          this.traditionalProperty.StatusDescription = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the body of the message is ignored and an empty message is sent.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message body is suppressed; otherwise, false. The default is false.
    /// </returns>
    public bool SuppressEntityBody
    {
      get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.SuppressEntityBody;
        else
          return this.httpBackedProperty.SuppressEntityBody;
      }
      set
      {
        if (this.useHttpBackedProperty)
          this.httpBackedProperty.SuppressEntityBody = value;
        else
          this.traditionalProperty.SuppressEntityBody = value;
      }
    }

    /// <summary>
    /// Gets or sets whether the message preamble is suppressed.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message preamble is suppressed; otherwise, false.
    /// </returns>
    public bool SuppressPreamble
    {
      get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.SuppressPreamble;
        else
          return false;
      }
      set
      {
        if (this.useHttpBackedProperty)
          return;
        this.traditionalProperty.SuppressPreamble = value;
      }
    }

    HttpResponseMessage HttpResponseMessage
    {
      private get
      {
        if (this.useHttpBackedProperty)
          return this.httpBackedProperty.HttpResponseMessage;
        else
          return (HttpResponseMessage) null;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.HttpResponseMessageProperty"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public HttpResponseMessageProperty()
      : this((WebHeaderCollection) null)
    {
    }

    internal HttpResponseMessageProperty(WebHeaderCollection originalHeaders)
    {
      this.traditionalProperty = new HttpResponseMessageProperty.TraditionalHttpResponseMessageProperty(originalHeaders);
      this.useHttpBackedProperty = false;
    }

    internal HttpResponseMessageProperty(HttpResponseMessage httpResponseMessage)
    {
      this.httpBackedProperty = new HttpResponseMessageProperty.HttpResponseMessageBackedProperty(httpResponseMessage);
      this.useHttpBackedProperty = true;
    }

    internal static HttpResponseMessage GetHttpResponseMessageFromMessage(Message message)
    {
      HttpResponseMessage httpResponseMessage = (HttpResponseMessage) null;
      HttpResponseMessageProperty responseMessageProperty = message.Properties.GetValue<HttpResponseMessageProperty>(HttpResponseMessageProperty.Name);
      if (responseMessageProperty != null)
      {
        httpResponseMessage = responseMessageProperty.HttpResponseMessage;
        if (httpResponseMessage != null)
        {
          HttpResponseMessageExtensionMethods.CopyPropertiesFromMessage(httpResponseMessage, message);
          message.EnsureReadMessageState();
        }
      }
      return httpResponseMessage;
    }

    [__DynamicallyInvokable]
    IMessageProperty IMessageProperty.CreateCopy()
    {
      if (this.useHttpBackedProperty && this.initialCopyPerformed)
        return (IMessageProperty) this.httpBackedProperty.CreateTraditionalResponseMessageProperty();
      this.initialCopyPerformed = true;
      return (IMessageProperty) this;
    }

    bool IMergeEnabledMessageProperty.TryMergeWithProperty(object propertyToMerge)
    {
      if (this.useHttpBackedProperty)
      {
        HttpResponseMessageProperty responseMessageProperty = propertyToMerge as HttpResponseMessageProperty;
        if (responseMessageProperty != null)
        {
          if (!responseMessageProperty.useHttpBackedProperty)
          {
            this.httpBackedProperty.MergeWithTraditionalProperty(responseMessageProperty.traditionalProperty);
            responseMessageProperty.traditionalProperty = (HttpResponseMessageProperty.TraditionalHttpResponseMessageProperty) null;
            responseMessageProperty.httpBackedProperty = this.httpBackedProperty;
            responseMessageProperty.useHttpBackedProperty = true;
          }
          return true;
        }
      }
      return false;
    }

    private class TraditionalHttpResponseMessageProperty
    {
      private WebHeaderCollection headers;
      private WebHeaderCollection originalHeaders;
      private HttpStatusCode statusCode;
      public const HttpStatusCode DefaultStatusCode = HttpStatusCode.OK;
      public const string DefaultStatusDescription = null;

      public WebHeaderCollection Headers
      {
        get
        {
          if (this.headers == null)
          {
            this.headers = new WebHeaderCollection();
            if (this.originalHeaders != null)
            {
              ((NameValueCollection) this.headers).Add((NameValueCollection) this.originalHeaders);
              this.originalHeaders = (WebHeaderCollection) null;
            }
          }
          return this.headers;
        }
      }

      public HttpStatusCode StatusCode
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.statusCode;
        }
        set
        {
          this.statusCode = value;
          this.HasStatusCodeBeenSet = true;
        }
      }

      public bool HasStatusCodeBeenSet { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; private set; }

      public string StatusDescription { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

      public bool SuppressEntityBody { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

      public bool SuppressPreamble { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

      public TraditionalHttpResponseMessageProperty(WebHeaderCollection originalHeaders)
      {
        this.originalHeaders = originalHeaders;
        this.statusCode = HttpStatusCode.OK;
        this.StatusDescription = (string) null;
      }
    }

    private class HttpResponseMessageBackedProperty
    {
      private HttpHeadersWebHeaderCollection headers;

      public HttpResponseMessage HttpResponseMessage { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; private set; }

      public WebHeaderCollection Headers
      {
        get
        {
          if (this.headers == null)
            this.headers = new HttpHeadersWebHeaderCollection(this.HttpResponseMessage);
          return (WebHeaderCollection) this.headers;
        }
      }

      public HttpStatusCode StatusCode
      {
        get
        {
          return this.HttpResponseMessage.StatusCode;
        }
        set
        {
          this.HttpResponseMessage.StatusCode = value;
        }
      }

      public string StatusDescription
      {
        get
        {
          return this.HttpResponseMessage.ReasonPhrase;
        }
        set
        {
          this.HttpResponseMessage.ReasonPhrase = value;
        }
      }

      public bool SuppressEntityBody
      {
        get
        {
          HttpContent content = this.HttpResponseMessage.Content;
          if (content != null)
          {
            long? contentLength = content.Headers.ContentLength;
            if (!contentLength.HasValue || contentLength.HasValue && contentLength.Value > 0L)
              return false;
          }
          return true;
        }
        set
        {
          HttpContent content = this.HttpResponseMessage.Content;
          if (value && content != null && (!content.Headers.ContentLength.HasValue || content.Headers.ContentLength.Value > 0L))
          {
            HttpContent httpContent = (HttpContent) new ByteArrayContent(EmptyArray<byte>.Instance);
            foreach (KeyValuePair<string, IEnumerable<string>> header in (HttpHeaders) content.Headers)
              HttpRequestMessageExtensionMethods.AddHeaderWithoutValidation((HttpHeaders) httpContent.Headers, header);
            this.HttpResponseMessage.Content = httpContent;
            content.Dispose();
          }
          else
          {
            if (value || content != null)
              return;
            this.HttpResponseMessage.Content = (HttpContent) new ByteArrayContent(EmptyArray<byte>.Instance);
          }
        }
      }

      public HttpResponseMessageBackedProperty(HttpResponseMessage httpResponseMessage)
      {
        this.HttpResponseMessage = httpResponseMessage;
      }

      public HttpResponseMessageProperty CreateTraditionalResponseMessageProperty()
      {
        HttpResponseMessageProperty responseMessageProperty = new HttpResponseMessageProperty();
        ((NameValueCollection) responseMessageProperty.Headers).Add((NameValueCollection) this.Headers);
        if (this.StatusCode != HttpStatusCode.OK)
          responseMessageProperty.StatusCode = this.StatusCode;
        responseMessageProperty.StatusDescription = this.StatusDescription;
        responseMessageProperty.SuppressEntityBody = this.SuppressEntityBody;
        return responseMessageProperty;
      }

      public void MergeWithTraditionalProperty(HttpResponseMessageProperty.TraditionalHttpResponseMessageProperty propertyToMerge)
      {
        if (propertyToMerge.HasStatusCodeBeenSet)
          this.StatusCode = propertyToMerge.StatusCode;
        if (propertyToMerge.StatusDescription != (string) null)
          this.StatusDescription = propertyToMerge.StatusDescription;
        this.SuppressEntityBody = propertyToMerge.SuppressEntityBody;
        WebHeaderCollection headers = propertyToMerge.Headers;
        foreach (string index in headers.AllKeys)
          ((NameValueCollection) this.Headers)[index] = ((NameValueCollection) headers)[index];
      }
    }
  }
}
