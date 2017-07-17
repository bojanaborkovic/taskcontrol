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
		public string Asignee { get; set; }
		public string Reporter { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }
		public string Project { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime? DateCreated { get; set; }
		public string Priority { get; set; }
	}
}
