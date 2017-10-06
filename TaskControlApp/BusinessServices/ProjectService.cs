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
          ret.Id = mappedProject.Id;

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

    public BaseProjectReturn GetProjectById(long Id)
    {
      BaseProjectReturn ret = new BaseProjectReturn();
      var project = _unitOfWork.ProjectRepository.GetByID(Id);
      if (project != null)
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<Project, BaseProjectReturn>();
        });
        IMapper mapper = config.CreateMapper();
        var mappedProject = mapper.Map<Project, BaseProjectReturn>(project);
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

    public ProjectStatisticsReturn GetProjectStatistics(long projectId)
    {
      _log.DebugFormat("GetAllProjects invoked");
      ProjectStatisticsReturn ret = new ProjectStatisticsReturn();
      try
      {
        var projectStatistics = _unitOfWork.GetProjectStatistics(projectId);
        if (projectStatistics.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<GetProjectStatistics_Result, TaskEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var tasksMapped = mapper.Map<List<GetProjectStatistics_Result>, List<TaskEntity>>(projectStatistics.ToList());
          _log.DebugFormat("GetAllProjects finished with : {0}", projectStatistics.ToString());
          ret.Tasks = tasksMapped;
          ret.RecordCount = tasksMapped.Count;
          return ret;

        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching projects... {0}", ex.Message);
      }
      return null;
    }

    public BaseProjectReturn UpdateProject(ProjectEntity project)
    {
      _log.DebugFormat("UpdateProject invoked");
      BaseProjectReturn ret = new BaseProjectReturn();

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
        ret.Id = projectToUpdate.Id;
        ret.Name = projectToUpdate.Name;
        ret.OwnerId = (long)projectToUpdate.OwnerId;
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during update project... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "UpdateError";
      }

      return ret;

    }

    public GetProjectReturn GetProjectsByOwner(long ownerId)
    {
      _log.DebugFormat("GetProjectsByOwner invoked for owner: {0}", ownerId);
      GetProjectReturn ret = new GetProjectReturn();

      try
      {
        var projects = _unitOfWork.ProjectRepository.Get(p => p.OwnerId == ownerId).ToList();
        if (projects.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Project, ProjectEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var projectsMapped = mapper.Map<List<Project>, List<ProjectEntity>>(projects.ToList());
          _log.DebugFormat("GetProjectsByOwner finished with : {0}", projects.ToString());
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

    public BaseProjectReturn GetProjectByName(string projectName)
    {
      _log.DebugFormat("GetProjectByName invoked");
      BaseProjectReturn ret = new BaseProjectReturn();

      try
      {
        var project = _unitOfWork.ProjectRepository.Get().Where(x => x.Name == projectName).FirstOrDefault();
        ret = MapProject(project);
        return ret;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error search users... {0}", ex.Message);
      }
      return null;
    }

    private BaseProjectReturn MapProject(Project project)
    {
      BaseProjectReturn ret = new BaseProjectReturn();
      ret.Id = project.Id;
      ret.Name = project.Name;
      ret.Owner =  project.OwnerId != null ? (long)project.OwnerId : 0;
      return ret;
    }

  }
}
