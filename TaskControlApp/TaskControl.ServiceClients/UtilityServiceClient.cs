using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TaskControl.ServiceClients
{
	public class UtilityServiceClient : BaseRestClient
	{
		public UtilityServiceClient(string repoName)
		{
			endpoint = string.Empty;
			endpoint = string.Format("{0}{1}", ConfigurationManager.AppSettings["TaskControlApiURL"], repoName);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public string GetAllIssueTypes()
		{
			string action = "/all";
			address = action;
			Method = HttpVerb.GET;
			var json = MakeRequest();
			return json;
		}

		public string GetAllStatuses()
		{
			string action = "/all";
			address = action;
			Method = HttpVerb.GET;
			var json = MakeRequest();
			return json;
		}

		public string GetPriorities()
		{
			string action = "/all";
			address = action;
			Method = HttpVerb.GET;
			var json = MakeRequest();
			return json;
		}
	}
}
