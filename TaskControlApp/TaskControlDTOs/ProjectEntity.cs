using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
	public class ProjectEntity
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public long OwnerId { get; set; }
		public string Description { get; set; }
		public string Owner { get; set; }
	}
}
