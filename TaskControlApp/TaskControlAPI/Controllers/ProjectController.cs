﻿using BusinessServices;
using BusinessServices.Interfaces;
using BussinesService.Interfaces.Responses.Project;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskControlDTOs;

namespace TaskControlAPI.Controllers
{
  public class ProjectController : ApiController
  {

    private readonly IProjectService _projectService;
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(ProjectController));
    public ProjectController()
    {
      _projectService = new ProjectService();
    }


    // GET: projects/all
    [HttpGet]
    [ActionName("GetAllProjects")]
    public HttpResponseMessage GetAllProjects()
    {
      _log.DebugFormat("GetAllProjects invoked...");
      //var projects = _projectService.GetAllProjects();
      var retProjects = _projectService.GetProjectsWithOwner();
      if (retProjects != null)
      {
        if (retProjects.Projects.Any())
        {
          _log.DebugFormat("GetAllProjects finished with : {0}", retProjects.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, retProjects);
        }

      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Projects not found");
    }

    // GET: projects/get/5
    [HttpGet]
    [ActionName("GetProjectById")]
    public HttpResponseMessage Get([FromUri] long projectId)
    {
      _log.DebugFormat("Get project with id {0}", projectId);
      var project = _projectService.GetProjectById(projectId);
      if (project != null)
      {
        _log.DebugFormat("Get project with finished with : {0}", project.ToString());
        return Request.CreateResponse(HttpStatusCode.OK, project);
      }

      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Projects not found");
    }


    [HttpPost]
    [ActionName("UpdateProject")]
    public HttpResponseMessage UpdateProject(ProjectEntity project)
    {
      _log.DebugFormat("UpdateProject with id {0}", project.Id);
      bool updated = _projectService.UpdateProject(project);
      if (!updated)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not update project");
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.OK);
      }

    }

    [HttpPost]
    [ActionName("CreateProject")]
    public HttpResponseMessage CreateProject(ProjectEntity project)
    {
      BaseProjectReturn createProjectRet = _projectService.CreateProject(project);
      if (createProjectRet != null && string.IsNullOrEmpty(createProjectRet.ErrorMessage))
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not update project");
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.OK);
      }

    }


    // POST: api/Project
    public void Post([FromBody]string value)
    {
    }

    // PUT: api/Project/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE: api/Project/5
    public void Delete(int id)
    {
    }
  }
}
