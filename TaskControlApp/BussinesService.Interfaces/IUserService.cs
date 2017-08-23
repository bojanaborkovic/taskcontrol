using BusinessServices.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
  public interface IUserService
  {
    UserEntity GetUserById(long UserId);
    UserEntity GetUserByUsername(string username);
    IEnumerable<UserEntity> GetAllUsers();
    long CreateUser(UserEntity user);
    bool UpdateUser(UserEntity user);
    IEnumerable<RoleEntity> GetAllRoles();
    BasicReturn AddUserToRole(long roleId, long userId);
    RoleReturn AddNewRole(RoleEntity role);
    List<UserEntity> SearchUsers();
  }
}
