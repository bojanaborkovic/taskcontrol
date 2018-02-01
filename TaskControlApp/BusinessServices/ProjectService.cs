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

    #region constructors
    public ProjectService()
    {
      _unitOfWork = new UnitOfWork();
    }
    #endregion


    #region IProjectService members
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

          ret.Id = mappedProject.Id;

          //assign project to existing role of the user
          var role = _unitOfWork.UserInRoleRepository.Get().Where(x => x.UserId == mappedProject.OwnerId).Single();

          if (role != null)
          {
            _unitOfWork.RoleClaimsRepository.Insert(new RoleClaimsOnProject()
            {
              ProjectId = mappedProject.Id,
              RoleId = role.RoleId,
              HaveAcess = true
            });

            _unitOfWork.Save();
          }

          scope.Complete();

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

    public GetProjectReturn GetAllProjects(long? userId)
    {
      _log.DebugFormat("GetAllProjects invoked");
      GetProjectReturn ret = new GetProjectReturn();

      try
      {
        List<Project> projects = new List<Project>();
        if (userId != null)
        {
          List<long> projectAcess = CheckProjectAccessForUser((long)userId);
          projects = _unitOfWork.ProjectRepository.Get(orderBy: q => q.OrderBy(d => d.Name)).Where(t => projectAcess.Contains(t.Id)).ToList();

        }
        else
        {
          projects = _unitOfWork.ProjectRepository.Get(orderBy: q => q.OrderBy(d => d.Name)).ToList();
        }
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

    public GetProjectReturn GetProjectsWithOwner(long? userId)
    {
      _log.DebugFormat("GetProjectsWithOwner invoked");
      GetProjectReturn ret = new GetProjectReturn();

      try
      {
        List<ProjectResult> projects = new List<ProjectResult>();
        if (userId != null)
        {
          List<long> projectAcess = CheckProjectAccessForUser((long)userId);
          projects = _unitOfWork.GetProjectsWithOwner().Where(t => projectAcess.Contains(t.ProjectId)).ToList();

          List<ProjectEntity> listOfProjects = new List<ProjectEntity>();
          listOfProjects = MapProjectsList(projects);
          ret.Projects = listOfProjects;
          ret.RecordCount = listOfProjects.Count;
          return ret;
        }
        else
        {
          var projectsEntities = _unitOfWork.ProjectRepository.GetAll().ToList();

          List<ProjectEntity> listOfProjects = new List<ProjectEntity>();
          listOfProjects = MapProjects(projectsEntities);
          ret.Projects = listOfProjects;
          ret.RecordCount = listOfProjects.Count;
          return ret;
        }


      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error search users... {0}", ex.Message);
      }
      return null;
    }

    private List<ProjectEntity> MapProjects(List<Project> projectsEntities)
    {
      List<ProjectEntity> projectsMapped = new List<ProjectEntity>();

      foreach (var item in projectsEntities)
      {
        projectsMapped.Add(new ProjectEntity() { Id = item.Id, Name = item.Name, OwnerId = item.OwnerId.HasValue ? item.OwnerId.Value : 0, Owner = GetAuthorById(item.OwnerId.Value) });
      }

      return projectsMapped;
    }

    public ProjectNotesReturn GetProjectNotes(long projectId)
    {
      _log.DebugFormat("GetProjectNotes invoked for project : {0}", projectId);
      ProjectNotesReturn ret = new ProjectNotesReturn();

      try
      {
        var projectNotes = _unitOfWork.NoteRepository.GetAll().Where(x => x.ProjectId == projectId).ToList();
        List<BussinesService.Interfaces.Responses.Project.Note> listOfNotes = new List<BussinesService.Interfaces.Responses.Project.Note>();
        listOfNotes = MapProjectNotes(projectNotes);
        ret.Notes = listOfNotes;
        ret.RecordCount = listOfNotes.Count;
        return ret;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error getting project notes... {0}", ex.Message);
      }

      return ret;
    }

    public ProjectNotesReturn AddNewNote(BussinesService.Interfaces.Responses.Project.Note note)
    {
      ProjectNotesReturn ret = new ProjectNotesReturn();
      try
      {

        _log.DebugFormat("Creating note : {0}", note.ToString());
        DataModel.Note mappedNote = new DataModel.Note();
        mappedNote.DateCreated = note.DateCreated;
        mappedNote.ProjectId = note.ProjectId;
        mappedNote.Text = note.Content;
        mappedNote.Author = note.AuthorId;
        _unitOfWork.NoteRepository.Insert(mappedNote);
        _unitOfWork.Save();

        var projectNotes = _unitOfWork.NoteRepository.GetAll().Where(x => x.ProjectId == note.ProjectId).ToList();
        ret.Notes = MapProjectNotes(projectNotes);
        ret.RecordCount = projectNotes.Count;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error creating new  note... {0}", ex.Message);
        ret.StatusCode = "Error";
        ret.ErrorMessage = ex.Message;
      }
      return ret;
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

    #endregion


    #region mappers and helpers
    private List<ProjectEntity> MapProjectsList(List<ProjectResult> projects)
    {
      List<ProjectEntity> projectsMapped = new List<ProjectEntity>();

      foreach (var item in projects)
      {
        projectsMapped.Add(new ProjectEntity() { Id = item.ProjectId, Name = item.Name, OwnerId = item.OwnerId, Owner = item.Owner });
      }

      return projectsMapped;
    }

    private BaseProjectReturn MapProject(Project project)
    {
      BaseProjectReturn ret = new BaseProjectReturn();
      ret.Id = project.Id;
      ret.Name = project.Name;
      ret.Owner = project.OwnerId != null ? (long)project.OwnerId : 0;
      return ret;
    }

    private List<long> CheckProjectAccessForUser(long userId)
    {
      List<long> projectIDs = new List<long>();
      var role = _unitOfWork.UserInRoleRepository.Get().Where(x => x.UserId == userId).SingleOrDefault();
      if (role != null)
      {
        long roleId = role.RoleId;
        var projectAccess = _unitOfWork.RoleClaimsRepository.GetAll().Where(x => x.RoleId == roleId).ToList();
        if (projectAccess != null && projectAccess.Count > 0)
        {
          foreach (var access in projectAccess)
          {
            projectIDs.Add(access.ProjectId);
          }
        }
      }

      return projectIDs;
    }

    private List<BussinesService.Interfaces.Responses.Project.Note> MapProjectNotes(List<DataModel.Note> projectNotes)
    {
      List<BussinesService.Interfaces.Responses.Project.Note> notes = new List<BussinesService.Interfaces.Responses.Project.Note>();
      if (projectNotes != null && projectNotes.Count > 0)
      {
        foreach (var note in projectNotes)
        {
          notes.Add(new BussinesService.Interfaces.Responses.Project.Note()
          {
            AuthorId = (long)note.Author,
            Content = note.Text,
            DateCreated = (DateTime)note.DateCreated,
            NoteId = note.Id,
            ProjectId = note.ProjectId,
            ProjectName = GetProjectNameById(note.ProjectId),
            AuthorName = GetAuthorById(note.Author)
          });
        }
      }

      return notes;
    }

    private string GetAuthorById(long? author)
    {
      string authorName = string.Empty;
      if (author != null)
      {
        var user = _unitOfWork.UserRepository.Get(x => x.Id == author).FirstOrDefault();
        if (user != null)
        {
          authorName = user.UserName;
        }
      }
      return authorName;
    }

    private string GetProjectNameById(long projectId)
    {
      string projectName = string.Empty;
      var project = _unitOfWork.ProjectRepository.Get(x => x.Id == projectId).FirstOrDefault();
      if (project != null)
      {
        projectName = project.Name;
      }

      return projectName;
    }

    #endregion

  }
}
