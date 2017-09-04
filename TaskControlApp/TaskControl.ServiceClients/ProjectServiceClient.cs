using BusinessServices.Interfaces;
using BussinesService.Interfaces.Responses.Project;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TaskControlDTOs;

namespace TaskControl.ServiceClients
{
  public class ProjectServiceClient : BaseRestClient, IProjectService
  {

    public ProjectServiceClient()
    {
      DoSerialize = true;
      BaseUri = new Uri(ConfigurationManager.AppSettings["TaskControlApiURL"]);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }


    public BaseProjectReturn CreateProject(ProjectEntity project)
    {
      return ExecutePost<BaseProjectReturn>(string.Format("{0}/{1}", "projects", "create"), project);
    }

    public GetProjectReturn GetAllProjects()
    {
      return Get<GetProjectReturn>(new Uri(string.Format("{0}{1}{2}", BaseUri.ToString(), "projects/", "/all")));
    }

    public BaseProjectReturn GetProjectById(long Id)
    {
      return ExecuteGet<BaseProjectReturn>(string.Format("{0}/{1}?projectId={2}", "projects", "get", Id));
    }

    public BaseProjectReturn UpdateProject(ProjectEntity project)
    {
      return ExecutePost<BaseProjectReturn>(string.Format("{0}/{1}", "projects", "update"), project);
    }

    public GetProjectReturn GetProjectsWithOwner()
    {
      throw new NotImplementedException();
    }
  }

}
