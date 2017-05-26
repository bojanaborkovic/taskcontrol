using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskControlDTOs;

namespace TaskControl.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

	public class CustomUserRole : IdentityUserRole<long> { }
	public class CustomUserClaim : IdentityUserClaim<long> { }
	public class CustomUserLogin : IdentityUserLogin<long> { }

	public class CustomRole : IdentityRole<long, CustomUserRole>
	{
		public CustomRole() { }
		public CustomRole(string name) { Name = name; }
	}

	public class CustomUserStore : UserStore<ApplicationUser, CustomRole, long,
			CustomUserLogin, CustomUserRole, CustomUserClaim>
	{
		public CustomUserStore(ApplicationDbContext context)
				: base(context)
		{
		}
	}

	public class CustomRoleStore : RoleStore<CustomRole, long, CustomUserRole>
	{
		public CustomRoleStore(ApplicationDbContext context)
				: base(context)
		{
		}
	}


	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
		long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }



}