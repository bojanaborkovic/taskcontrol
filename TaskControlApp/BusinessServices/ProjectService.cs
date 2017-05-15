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
		private readonly DataModel.UnitOfWork.UnitOfWork _unitOfWork;
		internal static readonly ILog _log = LogManager.GetLogger(typeof(UnitOfWork));
		
		public ProjectService()
		{
			_unitOfWork = new UnitOfWork();
		}

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
				var projectModel = Mapper.Map<DataModel.Project, ProjectEntity>(project);
				return projectModel;
			}
			return null;
		}


		public IEnumerable<ProjectEntity> GetAllProjects()
		{
			try
			{
				var projects = _unitOfWork.ProjectRepository.GetAll().ToList();
				if (projects.Any())
				{
					var projectsModel = Mapper.Map<List<Project>, List<ProjectEntity>>(projects);
					return projectsModel;
				}
			}
			catch(Exception ex)
			{
				_log.ErrorFormat("Error during fetching projects... {0}", ex.Message);
			}

			
			return null;
		}
	}
}
