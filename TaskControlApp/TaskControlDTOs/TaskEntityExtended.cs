using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
	public class TaskEntityExtended
	{
		public long TaskId { get; set; }
		public string Title { get; set; }
		public string IssueType { get; set; }
		public int IssueTypeId { get; set; }
		public string Asignee { get; set; }
		public long AsigneeId { get; set; }
		public string Reporter { get; set; }
		public long ReporterId { get; set; }
		public string Status { get; set; }
		public long StatusId { get; set; }
		public string Description { get; set; }
		public string Project { get; set; }
		public long ProjectId { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime? DateCreated { get; set; }
		public string Priority { get; set; }
		public int PriorityId { get; set; }
	}
}
