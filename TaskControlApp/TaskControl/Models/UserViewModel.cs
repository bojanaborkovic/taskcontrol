using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
    public class UserViewModel
    {
		public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
    }
}