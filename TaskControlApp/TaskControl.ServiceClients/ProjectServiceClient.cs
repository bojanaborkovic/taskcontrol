using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TaskControl.ServiceClients
{
	public class ProjectServiceClient : BaseRestClient
	{
		public ProjectServiceClient() 
		{
			endpoint = ConfigurationManager.AppSettings["TaskControlApiURL"];
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}


		public string GetAllProjects()
		{
			string address = string.Format("{0}{1}", endpoint, "projects/all");

			endpoint = address;
			Method = HttpVerb.GET;
			var json = MakeRequest();

			return json;
		}

		
	}

}
