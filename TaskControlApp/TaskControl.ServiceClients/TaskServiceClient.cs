using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TaskControl.ServiceClients
{
	public class TaskServiceClient : BaseRestClient
	{
		public TaskServiceClient(string repoName)
		{
			endpoint = string.Empty;
			endpoint = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings["TaskControlApiURL"], repoName);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public string GetAllTasks()
		{
			string action = "/all";
			address = action;
			Method = HttpVerb.GET;
			var json = MakeRequest();
			return json;
		}

		public string GetAllTasksDetails()
		{
			string action = "/details";
			address = action;
			Method = HttpVerb.GET;
			var json = MakeRequest();
			return json;
		}
	}
}
