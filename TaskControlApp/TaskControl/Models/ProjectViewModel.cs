using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public string Owner { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
	}
}