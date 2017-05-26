using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TaskControlDTOs;

namespace TaskControl.ServiceClients
{
    public class UserServiceClient : BaseRestClient
    {
        public UserServiceClient()
        {
            endpoint = ConfigurationManager.AppSettings["TaskControlApiURL"];
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string GetAllUsers()
        {
            string address = string.Format("{0}{1}", endpoint, "users/all");
            endpoint = address;
            Method = HttpVerb.GET;
            var json = MakeRequest();

            return json;
        }

		public string CreateUser(UserEntity newUser)
		{
			string address = string.Format("{0}{1}", endpoint, "users/create");
			endpoint = address;
			Method = HttpVerb.POST;
			PostData = new JavaScriptSerializer().Serialize(newUser);
			var json = MakeRequest();
			return json;
		}

		public string CreateAccount(UserEntity newUser)
		{
			string address = string.Format("{0}{1}", endpoint, "account/create");
			endpoint = address;
			Method = HttpVerb.POST;
			PostData = new JavaScriptSerializer().Serialize(newUser);
			var json = MakeRequest();
			return json;
		}

		public string UpdateUser(UserEntity userUpdate)
		{
			string address = string.Format("{0}{1}", endpoint, "users/update");
			endpoint = address;
			Method = HttpVerb.POST;
			PostData = new JavaScriptSerializer().Serialize(userUpdate);
			var json = MakeRequest();
			return json;

		}

    }
}
