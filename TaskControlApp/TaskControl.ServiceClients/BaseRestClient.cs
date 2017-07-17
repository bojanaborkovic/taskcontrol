using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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

		public string endpoint;
		public string address;

		public BaseRestClient()
		{
			endpoint = "";
			Method = HttpVerb.GET;
			ContentType = "text/json";
			PostData = "";
			log4net.Config.XmlConfigurator.Configure();
		}


		public BaseRestClient(string endpoint)
		{
			this.endpoint = endpoint;
			Method = HttpVerb.GET;
			ContentType = "text/json";
			PostData = "";
			log4net.Config.XmlConfigurator.Configure();
		}


		public BaseRestClient(string endpoint, HttpVerb method)
		{
			this.endpoint = endpoint;
			this.Method = method;
			this.ContentType = "text/json";
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
			if(_log.IsDebugEnabled)
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
			catch(Exception httpEx)
			{
				_log.ErrorFormat("Error during getting reponse : {0}", httpEx.Message);
			}

			return null;
			#endregion
		}

		public Dictionary<string ,string> GetSessionProperties()
		{
			var clientIP = HttpContext.Current.Request.UserHostAddress;
			Dictionary<string, string> properties = new Dictionary<string, string>();
			properties.Add("ClientIP", clientIP);
			return properties;
		}
	}

	public enum HttpVerb
	{
		GET,
		POST,
		PUT,
		DELETE
	}
}
