using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
	public interface IProjectService
	{
		ProjectEntity GetProjectById(long Id);
		IEnumerable<ProjectEntity> GetAllProjects();
		long CreateProject(ProjectEntity project);
		bool UpdateProject(ProjectEntity project);
		List<ProjectEntity> GetProjectsWithOwner();

	}
}
