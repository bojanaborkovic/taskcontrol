using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
	public class TaskEntity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
		public string Title { get; set; }
		public int IssueType { get; set; }
		public long Asignee { get; set; }
		public long Reporter { get; set; }
		public int Status { get; set; }
		public string Description { get; set; }
		public long ProjectId { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime DateCreated { get; set; }
		public int Priority { get; set; }
	}
}
