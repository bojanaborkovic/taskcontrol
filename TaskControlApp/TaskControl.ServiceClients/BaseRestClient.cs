using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TaskControl.ServiceClients
{
  public class BaseRestClient
  {
    protected System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(BaseRestClient));

    public HttpVerb Method { get; set; }
    public string ContentType { get; set; }
    public string PostData { get; set; }

    public bool DoSerialize { get; set; }

    public Dictionary<string, string> CustomHeaders { get; set; }

    public int? Timeout { get; set; }

    public string endpoint;
    public string address;
    public Uri BaseUri { get; set; }

    public BaseRestClient(Uri baseUri)
    {
      this.BaseUri = baseUri;
    }

    public BaseRestClient(string baseUrl)
      : this(new Uri(baseUrl))
    {
      CustomHeaders = null;
      DoSerialize = true;
    }

    public BaseRestClient()
    {
      endpoint = "";
      Method = HttpVerb.GET;
      ContentType = "application/json";
      PostData = "";
      log4net.Config.XmlConfigurator.Configure();
    }

    public BaseRestClient(string endpoint, HttpVerb method)
    {
      this.endpoint = endpoint;
      this.Method = method;
      this.ContentType = "application/json";
      this.PostData = "";
      log4net.Config.XmlConfigurator.Configure();
    }

    public string MakeRequest()
    {
      return MakeRequest("");
    }

    public string MakeRequest(string parameters)
    {
      string uri = endpoint + address;
      var request = (HttpWebRequest)WebRequest.Create(uri + parameters);
      if (_log.IsDebugEnabled)
      {
        _log.DebugFormat("Creating request with endpoint {0} and parameters {1}", endpoint, parameters);
      }


      request.Method = Method.ToString();
      request.ContentLength = 0;
      request.ContentType = ContentType;

      if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
      {
        var encoding = new UTF8Encoding();
        var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
        request.ContentLength = bytes.Length;

        using (var writeStream = request.GetRequestStream())
        {
          writeStream.Write(bytes, 0, bytes.Length);
        }

      }

      #region grabing response
      try
      {
        using (var response = (HttpWebResponse)request.GetResponse())
        {
          var responseValue = string.Empty;

          if (response.StatusCode != HttpStatusCode.OK)
          {
            var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
            _log.ErrorFormat(message);
            throw new ApplicationException(message);
          }

          // grab the response
          using (var responseStream = response.GetResponseStream())
          {
            if (responseStream != null)
              using (var reader = new StreamReader(responseStream))
              {
                responseValue = reader.ReadToEnd();
              }
          }

          _log.DebugFormat("Request finishd with status code : {0} and response value : {1}", response.StatusCode, responseValue);

          return responseValue;

        }

      }
      catch (Exception httpEx)
      {
        _log.ErrorFormat("Error during getting reponse : {0}", httpEx.Message);
      }

      return null;
      #endregion
    }

    public T Execute<T>(Uri uri, HttpVerb method, object input)
    {
      string inputData = String.Empty;
      if (DoSerialize)
      {
        inputData = input != null ? Serialize(input) : String.Empty;
      }
      else
      {
        inputData = (input as string).Replace("\"", "");
      }

      var request = (HttpWebRequest)WebRequest.Create(uri);

      request.Method = method.ToString();
      request.ContentLength = 0;
      request.ContentType = ContentType;

      if (Timeout.HasValue)
      {
        request.Timeout = Timeout.Value;
      }
      else
      {
        request.Timeout = 60000; //1 minute wait before timeout
      }

      if (CustomHeaders == null)
      {
        request.ContentType = "application/json";
      }

      if (_log.IsDebugEnabled)
      {
        _log.DebugFormat("Request:\n{0} {1} HTTP/1.1 \n{2}{3}\n", method, uri, request.Headers.ToString(), inputData);
      }

      if (!string.IsNullOrEmpty(inputData) && method == HttpVerb.POST)
      {
        var encoding = new UTF8Encoding();
        byte[] bytes = Encoding.UTF8.GetBytes(inputData);
        request.ContentLength = bytes.Length;

        using (var writeStream = request.GetRequestStream())
        {
          writeStream.Write(bytes, 0, bytes.Length);
        }

      }

      #region grabing response
      try
      {
        using (var response = (HttpWebResponse)request.GetResponse())
        {
          var responseValue = string.Empty;

          if (response.StatusCode != HttpStatusCode.OK)
          {
            var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
            _log.ErrorFormat(message);
            throw new ApplicationException(message);
          }

          // grab the response
          using (var responseStream = response.GetResponseStream())
          {
            if (responseStream != null)
              using (var reader = new StreamReader(responseStream))
              {
                responseValue = reader.ReadToEnd();
              }
          }

          _log.DebugFormat("Request finishd with status code : {0} and response value : {1}", response.StatusCode, responseValue);

          return Deserialize<T>(responseValue);

        }

      }
      catch (Exception httpEx)
      {
        _log.ErrorFormat("Error during getting reponse : {0}", httpEx.Message);
      }

      return default(T);
      #endregion
    }

    public T Get<T>(Uri uri)
    {
      return Execute<T>(uri, HttpVerb.GET, null);
    }

    public T Post<T>(Uri uri, object input)
    {
      return Execute<T>(uri, HttpVerb.POST, input);
    }

    public T Put<T>(Uri uri, object input)
    {
      return Execute<T>(uri, HttpVerb.PUT, input);
    }

    public T Delete<T>(Uri uri, object input)
    {
      return Execute<T>(uri, HttpVerb.DELETE, input);
    }

    public T ExecuteGet<T>(string action)
    {
      return Get<T>(new Uri(this.BaseUri, action));
    }

    public T ExecutePost<T>(string action, object input)
    {
      return Post<T>(new Uri(this.BaseUri, action), input);
    }

    public T ExecutePut<T>(string action, object input)
    {
      return Put<T>(new Uri(this.BaseUri, action), input);
    }

    public T ExecuteDelete<T>(string action, object input)
    {
      return Delete<T>(new Uri(this.BaseUri, action), input);
    }

    public Dictionary<string, string> GetSessionProperties()
    {
      var clientIP = HttpContext.Current.Request.UserHostAddress;
      Dictionary<string, string> properties = new Dictionary<string, string>();
      properties.Add("ClientIP", clientIP);
      return properties;
    }

    #region serialize/deserialize methods
    protected static string Serialize(object input)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(input.GetType());
        MemoryStream stream1 = new MemoryStream();

        StreamReader sr = new StreamReader(stream1);

        serializer.WriteObject(stream1, input);
        stream1.Position = 0;
        return sr.ReadToEnd();

      }
    }

    protected static T Deserialize<T>(string content)
    {
      using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        return (T)serializer.ReadObject(ms);
      }
    }
    #endregion
  }

  public enum HttpVerb
  {
    GET,
    POST,
    PUT,
    DELETE
  }
}
