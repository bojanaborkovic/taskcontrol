using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
	public class TaskSearchViewModel
	{
		public long TaskId { get; set; }

		public string Title { get; set; }

		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public string Asignee { get; set; }

		public DateTime? DateCreated { get; set; }

		public string IssueType { get; set; }

		public string Status { get; set; }

		public string Reporter { get; set; }

		public string Priority { get; set; }

		public string Project { get; set; }

		[DataType(DataType.Date)]
		public DateTime? DueDate { get; set; }
	}		
}