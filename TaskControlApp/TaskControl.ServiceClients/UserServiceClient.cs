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
    public UserServiceClient(string repoName)
    {
			endpoint = string.Empty;
      endpoint = string.Format("{0}{1}", ConfigurationManager.AppSettings["TaskControlApiURL"], repoName);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public string GetAllUsers()
    { 
			string action = "/all";
      address = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
      return json;
    }


		public string SearchUsers()
    {
      string action ="/search";
			address = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
			return json;
    }

    public string CreateUser(UserEntity newUser)
    {
      string action = "/create";
			address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(newUser);
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
			return json;
    }

		public string GetUserById(long userId)
		{
			string action = "/getbyid";
			address = action;
			Method = HttpVerb.GET;
			string paramts = string.Format("?userId={0}", userId);
			var json = MakeRequest(paramts);
			//ClearEndpoint(endpoint);
			return json;
		}


		public string GetUserByUsername(string username)
		{
			string action = "/get";
			address = action;
			Method = HttpVerb.GET;
			string paramts = string.Format("?username={0}", username);
			var json = MakeRequest(paramts);
			//ClearEndpoint(endpoint);
			return json;
		}

    public string CreateAccount(UserEntity newUser)
    {
      string action = string.Format("{0}{1}", endpoint, "account/create");
			address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(newUser);
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
			return json;
    }

    public string UpdateUser(UserEntity userUpdate)
    {
      string action = "/update";
			address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(userUpdate);
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
			return json;

    }

    public string GetAllRoles()
    {
      string action = string.Format("{0}{1}", endpoint, "/all");
      //string address = string.Format("{0}{1}", endpoint, "/search")
      endpoint = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
			//ClearEndpoint(endpoint);
			return json;

    }

    public string AddUserToRole(long roleId, long userId)
    {
      string action = string.Format("{0}{1}", endpoint, "users/adduser");
			address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(new { roleId, userId });
      string paramts = string.Format("?roleId={0}&userId={1}", roleId, userId);

      var json = MakeRequest(paramts);
			//ClearEndpoint(endpoint);
			return json;
    }

		public string AddNewRole(RoleEntity role)
		{
			string action = "/new";
			address = action;
			Method = HttpVerb.POST;
			PostData = new JavaScriptSerializer().Serialize(role);
			var json = MakeRequest();
			return json;
		}


		#region helper methods
		//private void ClearEndpoint(string endpoint)
		//{
		//	endpoint = endpoint.Substring(0, endpoint.LastIndexOf('/'));
		//}
		#endregion
	}
}
