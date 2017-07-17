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

		public long CreateProject(ProjectEntity projectEntity)
		{
			using (var scope = new TransactionScope())
			{
				var project = new Project()
				{
					Name = projectEntity.Name,
					OwnerId = projectEntity.OwnerId
				};

				_unitOfWork.ProjectRepository.Insert(project);
				_unitOfWork.Save();
				scope.Complete();

				return project.Id;
			}
		}

		public ProjectEntity GetProjectById(long Id)
		{
			var project = _unitOfWork.ProjectRepository.GetByID(Id);
			if (project != null)
			{
        var config = new MapperConfiguration(cfg => {
          cfg.CreateMap<Project, ProjectEntity>();
        });
        IMapper mapper = config.CreateMapper();
        var mappedProject = mapper.Map<Project, ProjectEntity>(project);
				return mappedProject;
			}
			return null;
		}


		public IEnumerable<TaskControlDTOs.ProjectEntity> GetAllProjects()
		{
			_log.DebugFormat("GetAllProjects invoked");
			try
			{
				var projects = _unitOfWork.ProjectRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
				if (projects.Any())
				{
					var config = new MapperConfiguration(cfg => {
						cfg.CreateMap<Project, ProjectEntity>();
					});

					IMapper mapper = config.CreateMapper();
					var projectsMapped = mapper.Map<List<Project>,List<ProjectEntity>>(projects.ToList());
					_log.DebugFormat("GetAllProjects finished with : {0}", projects.ToString());
					return projectsMapped;
				}
			}
			catch(Exception ex)
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

		public List<ProjectEntity> GetProjectsWithOwner()
		{
			_log.DebugFormat("GetProjectsWithOwner invoked");
			try
			{
				var projects = _unitOfWork.GetProjectsWithOwner();
				List<ProjectEntity> listOfProjects = new List<ProjectEntity>();
				listOfProjects = MapProjectsList(projects);
				return listOfProjects;

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

			foreach(var item in projects)
			{
				projectsMapped.Add(new ProjectEntity() { Id = item.ProjectId, Name = item.Name, OwnerId = item.OwnerId, Owner = item.Owner });
			}

			return projectsMapped;
		}
	}
}
