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


    //public string CreateProject(ProjectEntity project)
    //{
    //  string address = string.Format("{0}{1}", endpoint, "projects/create");
    //  endpoint = address;
    //  Method = HttpVerb.POST;
    //  PostData = new JavaScriptSerializer().Serialize(project);
    //  var json = MakeRequest();

    //  return json;

    //}


    public string GetProjectById(long projectId)
    {
      string address = string.Format("{0}{1}", endpoint, "projects/get");
      endpoint = address;
      Method = HttpVerb.GET;
      string paramts = string.Format("?projectId={0}", projectId);
      var json = MakeRequest(paramts);

      return json;
    }

    public List<ProjectEntity> GetProjectsWithOwner()
    {
      throw new NotImplementedException();
    }

    public string UpdateProject(ProjectEntity project)
    {
      string address = string.Format("{0}{1}", endpoint, "projects/update");
      endpoint = address;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(project);
      var json = MakeRequest();

      return json;

    }

    public BaseProjectReturn CreateProject(ProjectEntity project)
    {
      return ExecutePost<BaseProjectReturn>(string.Format("{0}/{1}", "projects", "create"), project);
    }

    public GetProjectReturn GetAllProjects()
    {
      return ExecuteGet<GetProjectReturn>(string.Format("{0}/{1}", "projects", "all"));
    }

    ProjectEntity IProjectService.GetProjectById(long Id)
    {
      throw new NotImplementedException();
    }

    GetProjectReturn IProjectService.GetProjectsWithOwner()
    {
      throw new NotImplementedException();
    }

    //ProjectEntity IProjectService.GetProjectById(long Id)
    //{
    //  ExecuteGet<ProjectEntity>()

    //  return json;
    //}

    bool IProjectService.UpdateProject(ProjectEntity project)
    {
      throw new NotImplementedException();
    }


  }

}
