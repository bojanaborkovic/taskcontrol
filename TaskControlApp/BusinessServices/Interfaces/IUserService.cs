using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices
{
    public interface IUserService
    {
			UserEntity GetUserById(long UserId);
			IEnumerable<UserEntity> GetAllUsers();
			long CreateUser(UserEntity user);
    }
}
