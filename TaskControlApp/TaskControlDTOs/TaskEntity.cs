using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
	public class TaskEntity
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public long IssueType { get; set; }
		public long Asignee { get; set; }
		public int Status { get; set; }
		public string Description { get; set; }
		public long ProjectId { get; set; }
		public DateTime DueDate { get; set; }
		public int Priority { get; set; }
	}
}
