using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
	public interface IUtilityService
	{
		IEnumerable<IssueTypeEntity> GetIssueTypes();
		IEnumerable<StatusEntity> GetAllStatuses();
		IEnumerable<PriorityEntity> GetPriorities();
	}
}
