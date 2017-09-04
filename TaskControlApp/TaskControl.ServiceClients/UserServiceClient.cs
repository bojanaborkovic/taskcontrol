using BusinessServices.Interfaces;
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
using BusinessServices.Interfaces.Responses;
using BussinesService.Interfaces.Responses.User;

namespace TaskControl.ServiceClients
{
  public class UserServiceClient : BaseRestClient, IUserService
  {
    public UserServiceClient(string repoName)
    {
      endpoint = string.Empty;
      BaseUri = new Uri(string.Format("{0}{1}", ConfigurationManager.AppSettings["TaskControlApiURL"], repoName));
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

    public BaseUserReturn GetUserById(long UserId)
    {
      return ExecuteGet<BaseUserReturn>(string.Format("/{0}?userId={1}", "getbyid", UserId));
    }

    public BaseUserReturn GetUserByUsername(string username)
    {
      return ExecuteGet<BaseUserReturn>(string.Format("/{0}?username={1}", "get", username));
    }

    public SearchUsersReturn GetAllUsers()
    {
      return Get<SearchUsersReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "all")));
    }

    public BaseUserReturn CreateUser(UserEntity user)
    {
      return ExecutePost<BaseUserReturn>(string.Format("{0}/{1}", "user", "create"), user);
    }

    public BasicReturn UpdateUser(UserEntity user)
    {
      return ExecutePost<BasicReturn>(string.Format("{0}/{1}", "user", "update"), user);
    }

    public SearchRolesReturn GetAllRoles()
    {
      return Get<SearchRolesReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "all")));
    }

    public BasicReturn AddUserToRole(UserInRoleEntity userInRole)
    {
      return ExecutePost<BasicReturn>(string.Format("{0}/{1}", "roles", "adduser"), userInRole);
    }

    public RoleReturn AddNewRole(RoleEntity role)
    {
      return ExecutePost<RoleReturn>(string.Format("{0}/{1}", "roles", "new"), role);
    }

    public SearchUsersReturn SearchUsers()
    {
      return Get<SearchUsersReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "search")));
    }

  }
}
