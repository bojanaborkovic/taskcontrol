using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskControlDTOs;

namespace TaskControl.Models
{
	public class ProjectViewModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public long OwnerId { get; set; }
		public string Username { get; set; }
		public string Description { get; set; }
	}
}