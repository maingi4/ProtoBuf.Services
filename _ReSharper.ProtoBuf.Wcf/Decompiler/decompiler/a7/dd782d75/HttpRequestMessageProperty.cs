// Type: System.ServiceModel.Channels.HttpRequestMessageProperty
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
  /// Provides access to the HTTP request to access and respond to the additional information made available for requests over the HTTP protocol.
  /// </summary>
  [__DynamicallyInvokable]
  public sealed class HttpRequestMessageProperty : IMessageProperty, IMergeEnabledMessageProperty
  {
    private HttpRequestMessageProperty.TraditionalHttpRequestMessageProperty traditionalProperty;
    private HttpRequestMessageProperty.HttpRequestMessageBackedProperty httpBackedProperty;
    private bool initialCopyPerformed;
    private bool useHttpBackedProperty;

    /// <summary>
    /// Gets the name of the message property associated with the <see cref="T:System.ServiceModel.Channels.HttpRequestMessageProperty"/> class.
    /// </summary>
    /// 
    /// <returns>
    /// The value "httpRequest".
    /// </returns>
    [__DynamicallyInvokable]
    public static string Name
    {
      [__DynamicallyInvokable] get
      {
        return "httpRequest";
      }
    }

    /// <summary>
    /// Gets the HTTP headers from the HTTP request.
    /// </summary>
    /// 
    /// <returns>
    /// Returns a <see cref="T:System.Net.WebHeaderCollection"/> that contains the HTTP headers in the HTTP request.
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
    /// Gets or sets the HTTP verb for the HTTP request.
    /// </summary>
    /// 
    /// <returns>
    /// The HTTP verb for the HTTP request.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The value is set to null.</exception>
    [__DynamicallyInvokable]
    public string Method
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.Method;
        else
          return this.httpBackedProperty.Method;
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        if (this.useHttpBackedProperty)
          this.httpBackedProperty.Method = value;
        else
          this.traditionalProperty.Method = value;
      }
    }

    /// <summary>
    /// Gets or sets the query string for the HTTP request.
    /// </summary>
    /// 
    /// <returns>
    /// The query string from the HTTP request.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The value is set to null.</exception>
    [__DynamicallyInvokable]
    public string QueryString
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.QueryString;
        else
          return this.httpBackedProperty.QueryString;
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
        if (this.useHttpBackedProperty)
          this.httpBackedProperty.QueryString = value;
        else
          this.traditionalProperty.QueryString = value;
      }
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the body of the message is ignored and only the headers are sent.
    /// </summary>
    /// 
    /// <returns>
    /// true if the message body is suppressed; otherwise, false. The default is false.
    /// </returns>
    [__DynamicallyInvokable]
    public bool SuppressEntityBody
    {
      [__DynamicallyInvokable] get
      {
        if (!this.useHttpBackedProperty)
          return this.traditionalProperty.SuppressEntityBody;
        else
          return this.httpBackedProperty.SuppressEntityBody;
      }
      [__DynamicallyInvokable] set
      {
        if (this.useHttpBackedProperty)
          this.httpBackedProperty.SuppressEntityBody = value;
        else
          this.traditionalProperty.SuppressEntityBody = value;
      }
    }

    HttpRequestMessage HttpRequestMessage
    {
      private get
      {
        if (this.useHttpBackedProperty)
          return this.httpBackedProperty.HttpRequestMessage;
        else
          return (HttpRequestMessage) null;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.ServiceModel.Channels.HttpRequestMessageProperty"/> class.
    /// </summary>
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public HttpRequestMessageProperty()
      : this((HttpRequestMessageProperty.IHttpHeaderProvider) null)
    {
    }

    internal HttpRequestMessageProperty(HttpRequestMessageProperty.IHttpHeaderProvider httpHeaderProvider)
    {
      this.traditionalProperty = new HttpRequestMessageProperty.TraditionalHttpRequestMessageProperty(httpHeaderProvider);
      this.useHttpBackedProperty = false;
    }

    internal HttpRequestMessageProperty(HttpRequestMessage httpRequestMessage)
    {
      this.httpBackedProperty = new HttpRequestMessageProperty.HttpRequestMessageBackedProperty(httpRequestMessage);
      this.useHttpBackedProperty = true;
    }

    internal static HttpRequestMessage GetHttpRequestMessageFromMessage(Message message)
    {
      HttpRequestMessage httpRequestMessage = (HttpRequestMessage) null;
      HttpRequestMessageProperty requestMessageProperty = message.Properties.GetValue<HttpRequestMessageProperty>(HttpRequestMessageProperty.Name);
      if (requestMessageProperty != null)
      {
        httpRequestMessage = requestMessageProperty.HttpRequestMessage;
        if (httpRequestMessage != null)
        {
          HttpRequestMessageExtensionMethods.CopyPropertiesFromMessage(httpRequestMessage, message);
          message.EnsureReadMessageState();
        }
      }
      return httpRequestMessage;
    }

    [__DynamicallyInvokable]
    IMessageProperty IMessageProperty.CreateCopy()
    {
      if (this.useHttpBackedProperty && this.initialCopyPerformed)
        return (IMessageProperty) this.httpBackedProperty.CreateTraditionalRequestMessageProperty();
      this.initialCopyPerformed = true;
      return (IMessageProperty) this;
    }

    bool IMergeEnabledMessageProperty.TryMergeWithProperty(object propertyToMerge)
    {
      if (this.useHttpBackedProperty)
      {
        HttpRequestMessageProperty requestMessageProperty = propertyToMerge as HttpRequestMessageProperty;
        if (requestMessageProperty != null)
        {
          if (!requestMessageProperty.useHttpBackedProperty)
          {
            this.httpBackedProperty.MergeWithTraditionalProperty(requestMessageProperty.traditionalProperty);
            requestMessageProperty.traditionalProperty = (HttpRequestMessageProperty.TraditionalHttpRequestMessageProperty) null;
            requestMessageProperty.httpBackedProperty = this.httpBackedProperty;
            requestMessageProperty.useHttpBackedProperty = true;
          }
          return true;
        }
      }
      return false;
    }

    internal interface IHttpHeaderProvider
    {
      void CopyHeaders(WebHeaderCollection headers);
    }

    private class TraditionalHttpRequestMessageProperty
    {
      private WebHeaderCollection headers;
      private HttpRequestMessageProperty.IHttpHeaderProvider httpHeaderProvider;
      private string method;
      public const string DefaultMethod = "POST";
      public const string DefaultQueryString = "";

      public WebHeaderCollection Headers
      {
        get
        {
          if (this.headers == null)
          {
            this.headers = new WebHeaderCollection();
            if (this.httpHeaderProvider != null)
            {
              this.httpHeaderProvider.CopyHeaders(this.headers);
              this.httpHeaderProvider = (HttpRequestMessageProperty.IHttpHeaderProvider) null;
            }
          }
          return this.headers;
        }
      }

      public string Method
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.method;
        }
        set
        {
          this.method = value;
          this.HasMethodBeenSet = true;
        }
      }

      public bool HasMethodBeenSet { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; private set; }

      public string QueryString { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

      public bool SuppressEntityBody { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set; }

      public TraditionalHttpRequestMessageProperty(HttpRequestMessageProperty.IHttpHeaderProvider httpHeaderProvider)
      {
        this.httpHeaderProvider = httpHeaderProvider;
        this.method = "POST";
        this.QueryString = "";
      }
    }

    private class HttpRequestMessageBackedProperty
    {
      private HttpHeadersWebHeaderCollection headers;

      public HttpRequestMessage HttpRequestMessage { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get; private set; }

      public WebHeaderCollection Headers
      {
        get
        {
          if (this.headers == null)
            this.headers = new HttpHeadersWebHeaderCollection(this.HttpRequestMessage);
          return (WebHeaderCollection) this.headers;
        }
      }

      public string Method
      {
        get
        {
          return this.HttpRequestMessage.Method.Method;
        }
        set
        {
          this.HttpRequestMessage.Method = new HttpMethod(value);
        }
      }

      public string QueryString
      {
        get
        {
          string query = this.HttpRequestMessage.RequestUri.Query;
          if (query.Length <= 0)
            return string.Empty;
          else
            return query.Substring(1);
        }
        set
        {
          this.HttpRequestMessage.RequestUri = new UriBuilder(this.HttpRequestMessage.RequestUri)
          {
            Query = value
          }.Uri;
        }
      }

      public bool SuppressEntityBody
      {
        get
        {
          HttpContent content = this.HttpRequestMessage.Content;
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
          HttpContent content = this.HttpRequestMessage.Content;
          if (value && content != null && (!content.Headers.ContentLength.HasValue || content.Headers.ContentLength.Value > 0L))
          {
            HttpContent httpContent = (HttpContent) new ByteArrayContent(EmptyArray<byte>.Instance);
            foreach (KeyValuePair<string, IEnumerable<string>> header in (HttpHeaders) content.Headers)
              HttpRequestMessageExtensionMethods.AddHeaderWithoutValidation((HttpHeaders) httpContent.Headers, header);
            this.HttpRequestMessage.Content = httpContent;
            content.Dispose();
          }
          else
          {
            if (value || content != null)
              return;
            this.HttpRequestMessage.Content = (HttpContent) new ByteArrayContent(EmptyArray<byte>.Instance);
          }
        }
      }

      public HttpRequestMessageBackedProperty(HttpRequestMessage httpRequestMessage)
      {
        this.HttpRequestMessage = httpRequestMessage;
      }

      public HttpRequestMessageProperty CreateTraditionalRequestMessageProperty()
      {
        HttpRequestMessageProperty requestMessageProperty = new HttpRequestMessageProperty();
        ((NameValueCollection) requestMessageProperty.Headers).Add((NameValueCollection) this.Headers);
        if (this.Method != "POST")
          requestMessageProperty.Method = this.Method;
        requestMessageProperty.QueryString = this.QueryString;
        requestMessageProperty.SuppressEntityBody = this.SuppressEntityBody;
        return requestMessageProperty;
      }

      public void MergeWithTraditionalProperty(HttpRequestMessageProperty.TraditionalHttpRequestMessageProperty propertyToMerge)
      {
        if (propertyToMerge.HasMethodBeenSet)
          this.Method = propertyToMerge.Method;
        if (propertyToMerge.QueryString != "")
          this.QueryString = propertyToMerge.QueryString;
        this.SuppressEntityBody = propertyToMerge.SuppressEntityBody;
        WebHeaderCollection headers = propertyToMerge.Headers;
        foreach (string index in headers.AllKeys)
          ((NameValueCollection) this.Headers)[index] = ((NameValueCollection) headers)[index];
      }
    }
  }
}
