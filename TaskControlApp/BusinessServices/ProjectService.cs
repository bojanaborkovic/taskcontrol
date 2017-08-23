using AutoMapper;
using BusinessServices.Interfaces;
using System.Transactions;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;
using log4net;
using BussinesService.Interfaces.Responses.Project;

namespace BusinessServices
{
  public class ProjectService : IProjectService
  {
    private readonly DataModel.UnitOfWork.UnitOfWork _unitOfWork = new UnitOfWork();
    internal static readonly ILog _log = LogManager.GetLogger(typeof(UnitOfWork));

    //public ProjectService()
    //{
    //	_unitOfWork = new UnitOfWork();
    //}

    public BaseProjectReturn CreateProject(ProjectEntity projectEntity)
    {

      BaseProjectReturn ret = new BaseProjectReturn();
      try
      {
        using (var scope = new TransactionScope())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<ProjectEntity, Project>();
          });
          IMapper mapper = config.CreateMapper();
          var mappedProject = mapper.Map<ProjectEntity, Project>(projectEntity);

          _log.DebugFormat("Creating project with : {0}", projectEntity.ToString());

          _unitOfWork.ProjectRepository.Insert(mappedProject);
          _unitOfWork.Save();
          scope.Complete();
          ret.ProjectId = mappedProject.Id;

        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error creating new  project... {0}", ex.Message);
        ret.StatusCode = "Error";
        ret.ErrorMessage = ex.Message;
      }
      return ret;
    }

    public ProjectEntity GetProjectById(long Id)
    {
      var project = _unitOfWork.ProjectRepository.GetByID(Id);
      if (project != null)
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<Project, ProjectEntity>();
        });
        IMapper mapper = config.CreateMapper();
        var mappedProject = mapper.Map<Project, ProjectEntity>(project);
        return mappedProject;
      }
      return null;
    }


    public GetProjectReturn GetAllProjects()
    {
      _log.DebugFormat("GetAllProjects invoked");
      GetProjectReturn ret = new GetProjectReturn();

      try
      {
        var projects = _unitOfWork.ProjectRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
        if (projects.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Project, ProjectEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var projectsMapped = mapper.Map<List<Project>, List<ProjectEntity>>(projects.ToList());
          _log.DebugFormat("GetAllProjects finished with : {0}", projects.ToString());
          ret.Projects = projectsMapped;
          ret.RecordCount = projectsMapped.Count;
          return ret;

        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching projects... {0}", ex.Message);
      }


      return null;
    }

    public bool UpdateProject(ProjectEntity project)
    {
      _log.DebugFormat("UpdateProject invoked");
      try
      {

        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<ProjectEntity, Project>();
        });

        IMapper mapper = config.CreateMapper();
        var projectToUpdate = mapper.Map<Project>(project);

        _unitOfWork.ProjectRepository.Update(projectToUpdate);
        _unitOfWork.Save();
        _log.DebugFormat("UpdateProject with Id:{0} finished", projectToUpdate.Id);
        return true;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during update project... {0}", ex.Message);
      }

      return false;

    }

    public GetProjectReturn GetProjectsWithOwner()
    {
      _log.DebugFormat("GetProjectsWithOwner invoked");
      GetProjectReturn ret = new GetProjectReturn();

      try
      {
        var projects = _unitOfWork.GetProjectsWithOwner();
        List<ProjectEntity> listOfProjects = new List<ProjectEntity>();
        listOfProjects = MapProjectsList(projects);
        ret.Projects = listOfProjects;
        ret.RecordCount = listOfProjects.Count;
        return ret;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error search users... {0}", ex.Message);
      }
      return null;
    }

    private List<ProjectEntity> MapProjectsList(List<ProjectResult> projects)
    {
      List<ProjectEntity> projectsMapped = new List<ProjectEntity>();

      foreach (var item in projects)
      {
        projectsMapped.Add(new ProjectEntity() { Id = item.ProjectId, Name = item.Name, OwnerId = item.OwnerId, Owner = item.Owner });
      }

      return projectsMapped;
    }
  }
}
